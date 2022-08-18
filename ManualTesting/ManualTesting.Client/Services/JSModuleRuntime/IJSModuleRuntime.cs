namespace ManualTesting.Client.Services;

public interface IJSModuleRuntime : IJSRuntime, IDisposable, IAsyncDisposable {
    /// <summary>
    /// <para>Preloads the given js-file as javascript-module.</para>
    /// <para>If already loaded/loading, it does nothing.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Root.razor.js"</param>
    public void PreLoadModule(string moduleUrl);


    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>false when the module is not loaded, otherwise true.</returns>
    public bool InvokeVoid(string moduleUrl, string identifier, params object?[]? args);

    /// <summary>
    /// <para>Invokes the specified JavaScript function in the specified module synchronously.</para>
    /// <para>If module is not loaded, it returns without any invoking. If synchronous is not supported, it fails with an exception.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns>default when the module is not loaded, otherwise result of the js-function.</returns>
    public TValue Invoke<TValue>(string moduleUrl, string identifier, params object?[]? args);

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
    public TValue Invoke<TValue>(string moduleUrl, string identifier, out bool success, params object?[]? args);


    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">pparameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module synchronously when supported, otherwise asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args);


    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidAsync(string moduleUrl, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask InvokeVoidAsync(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function in the specified module asynchronously.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleUrl">complete path of the module, e.g. "/Pages/Example.razor.js"</param>
    /// <param name="identifier">name of the javascript function</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">parameter passing to the js-function</param>
    /// <returns></returns>
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string moduleUrl, string identifier, CancellationToken cancellationToken, params object?[]? args);


    #region non-module methods

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public ValueTask<TValue> InvokeTrySync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string identifier, params object?[]? args);

    /// <summary>
    /// This method performs synchronous, if the underlying implementation supports synchrounous interoperability.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns></returns>
    public ValueTask InvokeVoidTrySync(string identifier, CancellationToken cancellationToken, params object?[]? args);

    #endregion
}
