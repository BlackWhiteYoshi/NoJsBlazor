using ManualTesting.Client.Languages;
using NoJsBlazor;

namespace ManualTesting.Client;

public partial class LanguageFlagIcon : ComponentBase, IDisposable {
    [Inject, AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    [CascadingParameter, AllowNull]
    private Root Root { get; init; }


    [AllowNull]
    private Dialog dialog;
    
    
    protected override void OnInitialized() {
        Root.DialogBox.Add(RenderLanguageDialog);
    }


    private void ToggleLanguageDialog(MouseEventArgs e) {
        if (!dialog.Active) {
            Root.Click += ToggleLanguageDialog;
            Root.MouseMove += dialog.headBarTC.OnMouseMove;
            Root.TouchMove += dialog.headBarTC.OnTouchMove;
            Root.MouseUp += dialog.headBarTC.OnMouseUp;
            Root.TouchEnd += dialog.headBarTC.OnTouchEnd;
            dialog.Open();
        }
        else {
            Root.Click -= ToggleLanguageDialog;
            Root.MouseMove -= dialog.headBarTC.OnMouseMove;
            Root.TouchMove -= dialog.headBarTC.OnTouchMove;
            Root.MouseUp -= dialog.headBarTC.OnMouseUp;
            Root.TouchEnd -= dialog.headBarTC.OnTouchEnd;
            JsRuntime.InvokeVoid("SetCookie", CBox.COOKIE_KEY_LANGUAGE, Lang.Language.ToString(), 365);
            dialog.Close();
            StateHasChanged();
        }
    }

    private void SetLanguage(Language language) {
        Lang.Language = language;
        Root.DialogBox.Rerender();
    }


    public void Dispose() {
        Root.DialogBox.Remove(RenderLanguageDialog);
        GC.SuppressFinalize(this);
    }
}
