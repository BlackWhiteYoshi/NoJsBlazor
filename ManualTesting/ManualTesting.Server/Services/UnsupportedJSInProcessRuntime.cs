using Microsoft.JSInterop;

namespace ManualTesting.Server.Services;

public class UnsupportedJSInProcessRuntime : IJSInProcessRuntime {
    public TResult Invoke<TResult>(string identifier, params object?[]? args) => throw new NotSupportedException();
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new NotSupportedException();
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotSupportedException();
}
