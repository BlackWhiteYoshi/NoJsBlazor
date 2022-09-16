using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// A normal component that rerenders when language is changed.
/// </summary>
public abstract class LanguageComponentBase : ComponentBase, IDisposable {
    [Inject, AllowNull]
    protected ILanguageProvider Lang { get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.OnLanguageChanged += Rerender;
    }

    public void Dispose() {
        Lang.OnLanguageChanged -= Rerender;
        GC.SuppressFinalize(this);
    }

    private void Rerender(Language language) => InvokeAsync(StateHasChanged);
}
