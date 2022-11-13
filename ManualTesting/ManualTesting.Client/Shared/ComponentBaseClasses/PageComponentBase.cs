namespace ManualTesting.Client;

/// <summary>
/// <para>A normal component that rerenders when language is changed.</para>
/// <para>A normal component that register itself on initializing at the Root, so Root know it current page.</para>
/// </summary>
public abstract class PageComponentBase : LanguageComponentBase, IDisposable {
    [Inject]
    public required IRoot Root { protected get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Root.PageComponent = this;
    }

    public new void Dispose() {
        base.Dispose();
        if (Root.PageComponent == this)
            Root.PageComponent = null;
    }
}
