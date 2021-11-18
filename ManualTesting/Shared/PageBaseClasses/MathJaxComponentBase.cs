namespace ManualTesting;

/// <summary>
/// A <see cref="PageComponentBase">PageComponent</see> that invokes JavaScript-MathJax for rendering all math expressions
/// </summary>
public abstract class MathJaxComponentBase : PageComponentBase {
    [Inject]
    [AllowNull]
    public IJSInProcessRuntime JsRuntime { get; init; }

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender)
            _ = JsRuntime.InvokeVoidAsync("MathJaxRender");
    }
}
