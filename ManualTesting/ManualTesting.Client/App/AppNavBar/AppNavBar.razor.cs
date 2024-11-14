using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class AppNavBar : ComponentBase {
    [Inject]
    public required IApp App { private get; init; }


    [AllowNull]
    private NavBar navBar;


    private void PhoneToggle(bool expanded) {
        if (expanded)
            App.Click += Reset;
        else
            App.Click -= Reset;
    }

    private void Reset(MouseEventArgs e) => navBar.Reset();
}
