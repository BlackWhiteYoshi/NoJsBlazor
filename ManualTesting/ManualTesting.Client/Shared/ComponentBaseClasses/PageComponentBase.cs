namespace ManualTesting.Client;

/// <summary>
/// A normal component that register itself on initializing at the Root, so Root know it current page.
/// </summary>
public abstract class PageComponentBase : LanguageComponentBase {
    [Inject, AllowNull]
    protected IRoot Root { get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Root.PageComponent = this;
    }
}
