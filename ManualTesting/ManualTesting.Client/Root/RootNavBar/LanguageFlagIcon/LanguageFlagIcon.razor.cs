using ManualTesting.Client.Services;
using NoJsBlazor;

namespace ManualTesting.Client;

public partial class LanguageFlagIcon : ComponentBase, IDisposable {
    [Inject, AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    [Inject, AllowNull]
    private IRoot Root { get; init; }

    [Inject, AllowNull]
    private IDialogBox DialogBox { get; init; }


    [AllowNull]
    private Dialog dialog;
    
    
    protected override void OnInitialized() {
        DialogBox.Add(RenderLanguageDialog);
    }


    private void ToggleLanguageDialog(MouseEventArgs e) {
        if (!dialog.Active) {
            Root.Click += ToggleLanguageDialog;
            dialog.Open();
        }
        else {
            Root.Click -= ToggleLanguageDialog;
            JsRuntime.InvokeVoid("SetCookie", CBox.COOKIE_KEY_LANGUAGE, Lang.Language.ToString(), 365);
            dialog.Close();
            StateHasChanged();
        }
    }

    private void OnTitleDown(PointerEventArgs e) {
        JsRuntime.InvokeVoid("SetPointerCapture", dialog.TitleDiv, e.PointerId);
    }

    private void OnTitleUp(PointerEventArgs e) {
        JsRuntime.InvokeVoid("ReleasePointerCapture", dialog.TitleDiv, e.PointerId);
    }
    

    private void SetLanguage(Language language) {
        Lang.Language = language;
        DialogBox.Rerender();
    }


    public void Dispose() {
        DialogBox.Remove(RenderLanguageDialog);
    }
}
