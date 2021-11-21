using NoJsBlazor;

namespace ManualTesting;

public partial class RootNavBar : ComponentBase {
    [Inject]
    [AllowNull]
    public IJSInProcessRuntime JsRuntime { get; init; }

    [Parameter]
    [AllowNull]
    public Root Root { get; init; }


    [AllowNull]
    private NavBar navBar;

    [AllowNull]
    private Dialog dialog;


    private readonly TouchClick navBarResetTC;
    private readonly TouchClick dialogOpenTC;
    private readonly TouchClick dialogCloseTC;
    private readonly TouchClick<Language> setLanguageTC;


    public RootNavBar() {
        navBarResetTC = new(ResetNavBar);
        dialogOpenTC = new(DialogOpen);
        dialogCloseTC = new(DialogClose);
        setLanguageTC = new(SetLanguage);
    }

    #region OnClick/OnTouch

    private void PhoneToggle(bool expanded) {
        if (expanded) {
            Root.MouseDown += navBarResetTC.OnMouseDown;
            Root.TouchStart += navBarResetTC.OnTouchStart;
        }
        else {
            Root.MouseDown -= navBarResetTC.OnMouseDown;
            Root.TouchStart -= navBarResetTC.OnTouchStart;
        }
    }

    private void ResetNavBar(EventArgs e) {
        navBar.Reset();
    }

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
        JsRuntime.InvokeVoid("localStorage.setItem", CBox.StorageKeyLanguage, DBox.Language);
        dialog.Close();
    }

    private void SetLanguage(EventArgs e) {
        CBox.SetLanguage(setLanguageTC.Parameter, JsRuntime);
        Root.Rerender();
    }

    #endregion
}
