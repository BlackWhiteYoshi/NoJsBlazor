using ManualTesting.Client.Services;
using NoJsBlazor;

namespace ManualTesting.Client;

public partial class RootNavBar : ComponentBase, IDisposable {
    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    [Inject, AllowNull]
    private IRoot Root { get; init; }


    [AllowNull]
    private NavBar navBar;


    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.OnLanguageChanged += Rerender;
    }

    public void Dispose() => Lang.OnLanguageChanged -= Rerender;

    private void Rerender(Language language) => InvokeAsync(StateHasChanged);


    private void PhoneToggle(bool expanded) {
        if (expanded)
            Root.Click += Reset;
        else
            Root.Click -= Reset;
    }

    private void Reset(MouseEventArgs e) => navBar.Reset();
}
