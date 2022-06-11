using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// A normal component that register itself on initializing at the Layout, so the Layout can call Rerender()
/// </summary>
public abstract class PageComponentBase : ComponentBase, IDisposable {
    [Inject, AllowNull]
    protected ILanguageProvider Lang { get; init; }

    [Inject, AllowNull]
    protected IRoot Root { get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.OnLanguageChanged += Rerender;
        Root.PageComponent = this;
    }

    public void Dispose() => Lang.OnLanguageChanged -= Rerender;

    private void Rerender(Language language) => InvokeAsync(StateHasChanged);
}
