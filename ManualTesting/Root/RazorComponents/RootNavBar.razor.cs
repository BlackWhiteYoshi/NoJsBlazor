using ManualTesting.Languages;
using NoJsBlazor;

namespace ManualTesting;

public partial class RootNavBar : ComponentBase {
    [Inject]
    [AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject]
    [AllowNull]
    private ILanguageProvider Lang { get; init; }

    [Parameter]
    [EditorRequired]
    [AllowNull]
    public Root Root { get; init; }


    [AllowNull]
    private NavBar navBar;
    [AllowNull]
    private Dialog dialog;


    private readonly TouchClick dialogOpenTC;
    private readonly TouchClick dialogCloseTC;
    private readonly TouchClick<Language> setLanguageTC;


    public RootNavBar() {
        dialogOpenTC = new(DialogOpen);
        dialogCloseTC = new(DialogClose);
        setLanguageTC = new(SetLanguage);
    }

    #region OnClick/OnTouch

    private void PhoneToggle(bool expanded) {
        if (expanded) {
            Root.MouseDown += Reset;
            Root.TouchStart += Reset;
        }
        else {
            Root.MouseDown -= Reset;
            Root.TouchStart -= Reset;
        }
    }

    private void Reset(EventArgs e) => navBar.Reset();


    private void DialogOpen(EventArgs e) {
        Root.MouseDown += dialogCloseTC.OnMouseDown;
        Root.TouchStart += dialogCloseTC.OnTouchStart;
        Root.MouseMove += dialog.headBarTC.OnMouseMove;
        Root.TouchMove += dialog.headBarTC.OnTouchMove;
        Root.MouseUp += dialog.headBarTC.OnMouseUp;
        Root.TouchEnd += dialog.headBarTC.OnTouchEnd;
        dialog.Open();
    }

    private void DialogClose(EventArgs e) {
        Root.MouseDown -= dialogCloseTC.OnMouseDown;
        Root.TouchStart -= dialogCloseTC.OnTouchStart;
        Root.MouseMove -= dialog.headBarTC.OnMouseMove;
        Root.TouchMove -= dialog.headBarTC.OnTouchMove;
        Root.MouseUp -= dialog.headBarTC.OnMouseUp;
        Root.TouchEnd -= dialog.headBarTC.OnTouchEnd;
        JsRuntime.InvokeVoid("localStorage.setItem", CBox.StorageKeyLanguage, Lang.Language);
        dialog.Close();
    }

    private void SetLanguage(EventArgs e) => Lang.Language = setLanguageTC.Parameter;

    #endregion


    public void Rerender() => StateHasChanged();
}
