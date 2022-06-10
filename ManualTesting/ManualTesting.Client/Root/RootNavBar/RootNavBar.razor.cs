using ManualTesting.Client.Services;
using NoJsBlazor;

namespace ManualTesting.Client;

public partial class RootNavBar : ComponentBase {
    [Inject, AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    [Inject, AllowNull]
    private IRoot Root { get; init; }


    [AllowNull]
    private NavBar navBar;


    private void PhoneToggle(bool expanded) {
        if (expanded)
            Root.Click += Reset;
        else
            Root.Click -= Reset;
    }

    private void Reset(MouseEventArgs e) => navBar.Reset();


    public void Rerender() => InvokeAsync(StateHasChanged);
}
