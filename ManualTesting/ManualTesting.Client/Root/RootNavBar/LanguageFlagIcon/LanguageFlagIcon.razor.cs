using ManualTesting.Client.Languages;
using NoJsBlazor;

namespace ManualTesting.Client;

public partial class LanguageFlagIcon : ComponentBase, IDisposable {
    [Inject]
    [AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject]
    [AllowNull]
    private ILanguageProvider Lang { get; init; }

    [CascadingParameter]
    [AllowNull]
    private Root Root { get; init; }


    [AllowNull]
    private Dialog dialog;
    
    
    private readonly TouchClick toogleDialogTC;
    private readonly TouchClick<Language> setLanguageTC;


    public LanguageFlagIcon() {
        toogleDialogTC = new(ToggleLanguageDialog);
        setLanguageTC = new(SetLanguage);
    }


    protected override void OnInitialized() {
        Root.DialogBox.AddDialog(RenderLanguageDialog);
    }


    private void ToggleLanguageDialog(EventArgs e) {
        if (!dialog.Active) {
            Root.MouseDown += ToggleLanguageDialog;
            Root.TouchStart += ToggleLanguageDialog;
            Root.MouseMove += dialog.headBarTC.OnMouseMove;
            Root.TouchMove += dialog.headBarTC.OnTouchMove;
            Root.MouseUp += dialog.headBarTC.OnMouseUp;
            Root.TouchEnd += dialog.headBarTC.OnTouchEnd;
            dialog.Open();
        }
        else {
            Root.MouseDown -= ToggleLanguageDialog;
            Root.TouchStart -= ToggleLanguageDialog;
            Root.MouseMove -= dialog.headBarTC.OnMouseMove;
            Root.TouchMove -= dialog.headBarTC.OnTouchMove;
            Root.MouseUp -= dialog.headBarTC.OnMouseUp;
            Root.TouchEnd -= dialog.headBarTC.OnTouchEnd;
            JsRuntime.InvokeVoid("SetCookie", CBox.COOKIE_KEY_LANGUAGE, Lang.Language.ToString(), 365);
            dialog.Close();
            StateHasChanged();
        }
    }
    
    private void SetLanguage(EventArgs e) => Lang.Language = setLanguageTC.Parameter;


    public void Dispose() {
        Root.DialogBox.RemoveDialog(RenderLanguageDialog);
        GC.SuppressFinalize(this);
    }
}
