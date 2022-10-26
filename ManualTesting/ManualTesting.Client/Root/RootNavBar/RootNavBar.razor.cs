using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class RootNavBar : LanguageComponentBase {
    [Inject]
    private IRoot Root { get; init; } = null!;


    [AllowNull]
    private NavBar navBar;


    private void PhoneToggle(bool expanded) {
        if (expanded)
            Root.Click += Reset;
        else
            Root.Click -= Reset;
    }

    private void Reset(MouseEventArgs e) => navBar.Reset();
}
