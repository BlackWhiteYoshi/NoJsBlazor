using ManualTesting.Client.Services;
using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class LanguageFlagIcon : LanguageComponentBase, IDisposable {
    [Inject]
    private IJSModuleRuntime JsModuleRuntime { get; init; } = null!;

    [Inject]
    private IRoot Root { get; init; } = null!;

    [Inject]
    private ITopLevelPortal TopLevelPortal { get; init; } = null!;


    [AllowNull]
    private Dialog dialog;


    protected override void OnInitialized() {
        base.OnInitialized();
        TopLevelPortal.Add(RenderLanguageDialog);
    }

    public new void Dispose() {
        base.Dispose();
        TopLevelPortal.Remove(RenderLanguageDialog);
    }


    private void ToggleLanguageDialog(MouseEventArgs e) {
        if (!dialog.Active) {
            Root.Click += ToggleLanguageDialog;
            dialog.Open();
        }
        else {
            Root.Click -= ToggleLanguageDialog;
            _ = JsModuleRuntime.InvokeVoidTrySync(CBox.SHARED_JS, "setCookie", CBox.COOKIE_KEY_LANGUAGE, Lang.Language.ToString(), 365).Preserve();
            dialog.Close();
            StateHasChanged();
        }
    }

    private void OnTitleDown(PointerEventArgs e) {
        _ = JsModuleRuntime.InvokeVoidTrySync(CBox.SHARED_JS, "setPointerCapture", dialog.TitleDiv, e.PointerId).Preserve();
    }

    private void OnTitleUp(PointerEventArgs e) {
        _ = JsModuleRuntime.InvokeVoidTrySync(CBox.SHARED_JS, "releasePointerCapture", dialog.TitleDiv, e.PointerId).Preserve();
    }
}
