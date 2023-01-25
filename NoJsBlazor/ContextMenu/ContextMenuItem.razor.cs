namespace NoJsBlazor;

/// <summary>
/// <para>Wrapper for the content that will be a context item.</para>
/// <para>This should be placed inside of a <see cref="ContextMenu"/>/<see cref="ContextSubMenu"/> instance.</para>
/// </summary>
public sealed partial class ContextMenuItem : ComponentBase {
    /// <summary>
    /// Html that will be displayed.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    /// <summary>
    /// <para>Invokes every time when this list item get clicked or touched.</para>
    /// <para><see cref="EventArgs"/> is either <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>, depends if it was invoked by clicking or touching.</para>
    /// </summary>
    [Parameter]
    public EventCallback<EventArgs> OnPressed { get; set; }


    private void OnClick(EventArgs e) => OnPressed.InvokeAsync(e);
}
