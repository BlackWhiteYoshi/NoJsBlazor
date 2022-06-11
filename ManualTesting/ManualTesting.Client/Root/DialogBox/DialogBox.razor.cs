using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// A Container located at the top layer of &lt;body&gt;, so dialog-RenderFragments can be rendered here.
/// </summary>
public partial class DialogBox : ServiceComponentBase<IDialogBox>, IDialogBox {
    private readonly List<RenderFragment> dialogList = new();

    /// <summary>
    /// Adds the given RenderFragment to the list.
    /// </summary>
    public void Add(RenderFragment renderFragment) {
        dialogList.Add(renderFragment);
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Removes the given RenderFragment from the list.</para>
    /// <para>If not found, nothing happens.</para>
    /// </summary>
    public void Remove(RenderFragment renderFragment) {
        dialogList.Remove(renderFragment);
        InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Triggers a <see cref="ComponentBase.StateHasChanged">StateHasChanged</see>.
    /// </summary>
    public void Rerender() => InvokeAsync(StateHasChanged);


    #region Language Listener

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.OnLanguageChanged += Rerender;
    }

    public new void Dispose() => Lang.OnLanguageChanged -= Rerender;

    private void Rerender(Language language) => InvokeAsync(StateHasChanged);

    #endregion
}
