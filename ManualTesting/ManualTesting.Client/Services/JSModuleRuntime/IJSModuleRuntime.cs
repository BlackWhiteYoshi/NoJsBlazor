using Microsoft.JSInterop.Infrastructure;

namespace ManualTesting.Client.Services;

public interface IJSModuleRuntime : IJSRuntime {
    protected IJSRuntime JsRuntime { get; }


    /// <summary>
    /// <para>Preloads the given js-file as javascript-module.</para>
    /// <para>If already loading, it doesn't trigger a second loading and if already loaded, it returns a completed task.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Root.razor.js"</param>
    /// <returns>A Task that completes when the module is loaded.</returns>
    public async ValueTask PreLoadModule(string moduleUrl)
        => await GetOrLoadModule(moduleUrl);

    protected Task<IJSObjectReference> GetOrLoadModule(string moduleUrl);


    #region module methods

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>false when the module is not loaded, otherwise true.</returns>
    public void InvokeVoid(string moduleUrl, string identifier, params object?[]? args)
        => Invoke<IJSVoidResult>(moduleUrl, identifier, out _, args);

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="success">false when the module is not loaded, otherwise true</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>false when the module is not loaded, otherwise true.</returns>
    public void InvokeVoid(string moduleUrl, string identifier, out bool success, params object?[]? args)
        => Invoke<IJSVoidResult>(moduleUrl, identifier, out success, args);

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>default when the module is not loaded, otherwise result of the js-function.</returns>
    public TResult Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TResult>(string moduleUrl, string identifier, params object?[]? args)
        => Invoke<TResult>(moduleUrl, identifier, out _, args);

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
    public TResult Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TResult>(string moduleUrl, string identifier, out bool success, params object?[]? args) {
        Task<IJSObjectReference> moduleTask = GetOrLoadModule(moduleUrl);
        if (!moduleTask.IsCompletedSuccessfully) {
            success = false;
            return default!;
        }

        success = true;
        return ((IJSInProcessObjectReference)moduleTask.Result).Invoke<TResult>(identifier, args);
    }


    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, params object?[]? args)
        => await InvokeTrySync<IJSVoidResult>(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args)
        => await InvokeTrySync<IJSVoidResult>(moduleUrl, identifier, cancellationToken, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, params object?[]? args)
        => InvokeTrySync<TValue>(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetOrLoadModule(moduleUrl);
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
    public async ValueTask InvokeVoidAsync(string moduleUrl, string identifier, params object?[]? args)
        => await InvokeAsync<IJSVoidResult>(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidAsync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args)
        => await InvokeAsync<IJSVoidResult>(moduleUrl, identifier, cancellationToken, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, params object?[]? args)
        => InvokeAsync<TValue>(moduleUrl, identifier, default, args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args) {
        IJSObjectReference module = await GetOrLoadModule(moduleUrl);
        return await module.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }

    #endregion


    #region non-module methods

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidTrySync(string identifier, params object?[]? args)
        => await InvokeTrySync<IJSVoidResult>(identifier, default(CancellationToken), args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public async ValueTask InvokeVoidTrySync(string identifier, CancellationToken cancellationToken, params object?[]? args)
        => await InvokeTrySync<IJSVoidResult>(identifier, cancellationToken, args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args)
        => InvokeTrySync<TValue>(identifier, default(CancellationToken), args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args) {
        if (JsRuntime is IJSInProcessRuntime jsInProcessRuntime)
            return ValueTask.FromResult(jsInProcessRuntime.Invoke<TValue>(identifier, args));
        else
            return JsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// <para>
    /// <see cref="JSRuntime"/> will apply timeouts to this operation based on the value configured in <see cref="JSRuntime.DefaultAsyncTimeout"/>. To dispatch a call with a different timeout, or no timeout,
    /// consider using <see cref="InvokeAsync{TValue}(string, CancellationToken, object[])" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    ValueTask<TValue> IJSRuntime.InvokeAsync<TValue>(string identifier, object?[]? args)
        => InvokeAsync<TValue>(identifier, default(CancellationToken), args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    ValueTask<TValue> IJSRuntime.InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        => JsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);

    #endregion
}
