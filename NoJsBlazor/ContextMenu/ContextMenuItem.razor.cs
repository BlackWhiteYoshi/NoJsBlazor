namespace NoJsBlazor;

/// <summary>
/// <para>Wrapper for the content that will be a context item.</para>
/// <para>This should be placed inside of a <see cref="ContextMenu"/> or <see cref="ContextSubMenu"/> instance.</para>
/// </summary>
public sealed partial class ContextMenuItem : ComponentBase {
    /// <summary>
    /// Html that will be displayed.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Invokes every time when this list item get clicked or touched.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnPressed { get; set; }


    private void OnClick(MouseEventArgs e) => OnPressed.InvokeAsync(e);
}
