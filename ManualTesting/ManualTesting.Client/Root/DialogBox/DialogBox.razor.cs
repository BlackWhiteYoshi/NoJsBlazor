namespace ManualTesting.Client;

/// <summary>
/// A Container located at the top layer of &lt;body&gt;, so dialog-RenderFragments can be rendered here.
/// </summary>
public sealed partial class DialogBox : LanguageServiceComponentBase<IDialogBox>, IDialogBox {
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
}
