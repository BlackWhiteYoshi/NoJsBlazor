namespace ManualTesting.Client.Services;

public class JSModuleRuntime : IJSModuleRuntime, IDisposable, IAsyncDisposable {
    #region construction

    private readonly IJSRuntime _jsRuntime;
    IJSRuntime IJSModuleRuntime.JsRuntime => _jsRuntime;

    public JSModuleRuntime(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    #endregion


    #region disposing

    private readonly CancellationTokenSource cancellationTokenSource = new();

    public void Dispose() {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();

        foreach ((_, Task<IJSObjectReference> moduleTask) in moduleList)
            if (moduleTask.IsCompletedSuccessfully)
                _ = moduleTask.Result.DisposeAsync().Preserve();

        moduleList.Clear();
    }

    public ValueTask DisposeAsync() {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();

        List<Task> taskList = new();
        foreach ((_, Task<IJSObjectReference> moduleTask) in moduleList) {
            if (moduleTask.IsCompletedSuccessfully) {
                ValueTask valueTask = moduleTask.Result.DisposeAsync();
                if (!valueTask.IsCompleted)
                    taskList.Add(valueTask.AsTask());
            }
        }

        moduleList.Clear();

        if (taskList.Count == 0)
            return ValueTask.CompletedTask;
        else
            return new ValueTask(Task.WhenAll(taskList));
    }

    #endregion


    #region moduleList

    private readonly Dictionary<string, Task<IJSObjectReference>> moduleList = new();

    /// <summary>
    /// <para>Retrieves loaded modules from the <see cref="moduleList">Dictionary</see>.</para>
    /// <para>If the module is loaded, it is returned as a completed task.<br />
    /// If the module is still loading, that task is returned.<br />
    /// If the module is not loaded, a new task that loads the module will be created, added to the <see cref="moduleList">Dictionary</see> and returned.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <returns></returns>
    Task<IJSObjectReference> IJSModuleRuntime.GetOrLoadModule(string moduleUrl) {
        bool keyExists = moduleList.TryGetValue(moduleUrl, out Task<IJSObjectReference>? moduleTask);

        if (keyExists)
            return moduleTask!;
        else {
            Task<IJSObjectReference> importTask = _jsRuntime.InvokeAsync<IJSObjectReference>("import", cancellationTokenSource.Token, moduleUrl).AsTask();
            moduleList.Add(moduleUrl, importTask);
            return importTask;
        }
    }

    #endregion
}
