using ManualTesting.Client.Services;
using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class LanguageFlagIcon : ComponentBase, IDisposable {
    [Inject, AllowNull]
    private IJSModuleRuntime JsModuleRuntime { get; init; }

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    [Inject, AllowNull]
    private IRoot Root { get; init; }

    [Inject, AllowNull]
    private IDialogBox DialogBox { get; init; }


    [AllowNull]
    private Dialog dialog;
    
    
    protected override void OnInitialized() {
        base.OnInitialized();
        DialogBox.Add(RenderLanguageDialog);
        Lang.OnLanguageChanged += Rerender;
    }

    public void Dispose() {
        DialogBox.Remove(RenderLanguageDialog);
        Lang.OnLanguageChanged -= Rerender;
    }
    
    private void Rerender(Language language) => InvokeAsync(StateHasChanged);


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
