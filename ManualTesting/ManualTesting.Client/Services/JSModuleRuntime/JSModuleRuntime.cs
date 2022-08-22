namespace ManualTesting.Client.Services;

public class JSModuleRuntime : IJSModuleRuntime, IDisposable, IAsyncDisposable {
    private readonly Dictionary<string, Task<IJSObjectReference>> moduleList = new();
    private readonly CancellationTokenSource cancellationTokenSource = new();


    private readonly IJSRuntime _jsRuntime;


    public JSModuleRuntime(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

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


    /// <summary>
    /// <para>Retrieves loaded modules from the <see cref="moduleList">Dictionary</see>.</para>
    /// <para>If the module is loaded, it is returned as a completed task.<br />
    /// If the module is still loading, that task is returned.<br />
    /// If the module is not loaded, a new task that loads the module will be created, added to the <see cref="moduleList">Dictionary</see> and returned.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <returns></returns>
    private Task<IJSObjectReference> GetModule(string moduleUrl) {
        bool keyExists = moduleList.TryGetValue(moduleUrl, out Task<IJSObjectReference>? moduleTask);

        if (keyExists)
            return moduleTask!;
        else {
            Task<IJSObjectReference> importTask = _jsRuntime.InvokeAsync<IJSObjectReference>("import", cancellationTokenSource.Token, moduleUrl).AsTask();
            moduleList.Add(moduleUrl, importTask);
            return importTask;
        }
    }


    /// <summary>
    /// <para>Preloads the given js-file as javascript-module.</para>
    /// <para>If already loaded/loading, it does nothing.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Root.razor.js"</param>
    public void PreLoadModule(string moduleUrl) {
        _ = GetModule(moduleUrl);
    }


    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>false when the module is not loaded, otherwise true.</returns>
    public bool InvokeVoid(string moduleUrl, string identifier, params object?[]? args) {
        bool keyExists = moduleList.TryGetValue(moduleUrl, out Task<IJSObjectReference>? moduleTask);
        if (!keyExists || !moduleTask!.IsCompletedSuccessfully)
            return false;

        ((IJSInProcessObjectReference)moduleTask.Result).InvokeVoid(identifier, args);
        return true;
    }

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>default when the module is not loaded, otherwise result of the js-function.</returns>
    public TValue Invoke<TValue>(string moduleUrl, string identifier, params object?[]? args) => Invoke<TValue>(moduleUrl, identifier, out _, args);

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="success">false when the module is not loaded, otherwise true</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>default when the module is not loaded, otherwise result of the js-function</returns>
    public TValue Invoke<TValue>(string moduleUrl, string identifier, out bool success, params object?[]? args) {
        bool keyExists = moduleList.TryGetValue(moduleUrl, out Task<IJSObjectReference>? moduleTask);
        if (!keyExists || !moduleTask!.IsCompletedSuccessfully) {
            success = false;
            return default!;
        }

        success = true;
        return ((IJSInProcessObjectReference)moduleTask.Result).Invoke<TValue>(identifier, args);
    }


    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, params object?[]? args) => InvokeVoidTrySync(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetModule(moduleUrl);
        if (module is IJSInProcessObjectReference inProcessModule)
            inProcessModule.InvokeVoid(identifier, args);
        else
            await module.InvokeVoidAsync(identifier, cancellationToken, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeTrySync<TValue>(string moduleUrl, string identifier, params object?[]? args) => InvokeTrySync<TValue>(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask<TValue> InvokeTrySync<TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetModule(moduleUrl);
        if (module is IJSInProcessObjectReference inProcessModule)
            return inProcessModule.Invoke<TValue>(identifier, args);
        else
            return await module.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }


    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidAsync(string moduleUrl, string identifier, params object?[]? args) => InvokeVoidAsync(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidAsync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetModule(moduleUrl);
        await module.InvokeVoidAsync(identifier, cancellationToken, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeAsync<TValue>(string moduleUrl, string identifier, params object?[]? args) => InvokeAsync<TValue>(moduleUrl, identifier, default, args); 

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask<TValue> InvokeAsync<TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetModule(moduleUrl);
        return await module.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }


    #region non-module methods

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<TValue>(string identifier, params object?[]? args) => InvokeTrySync<TValue>(identifier, default(CancellationToken), args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args) {
        if (_jsRuntime is IJSInProcessRuntime jsInProcessRuntime)
            return ValueTask.FromResult(jsInProcessRuntime.Invoke<TValue>(identifier, args));
        else
            return _jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string identifier, params object?[]? args) => InvokeVoidTrySync(identifier, default(CancellationToken), args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string identifier, CancellationToken cancellationToken, params object?[]? args) {
        if (_jsRuntime is IJSInProcessRuntime jsInProcessRuntime) {
            jsInProcessRuntime.InvokeVoid(identifier, args);
            return ValueTask.CompletedTask;
        }
        else
            return _jsRuntime.InvokeVoidAsync(identifier, cancellationToken, args);
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => _jsRuntime.InvokeAsync<TValue>(identifier, args);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => _jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);

    #endregion
}
