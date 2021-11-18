namespace NoJsBlazor;

/// <summary>
/// Another <see cref="ContextMenu"/> inside of a <see cref="ContextMenu"/> or <see cref="ContextSubMenu"/>.
/// </summary>
public partial class ContextSubMenu : ListableComponentBase<ContextSubMenu> {
    /// <summary>
    /// Content that is collapsed visible.
    /// </summary>
    [Parameter]
    public RenderFragment? Head { get; set; }

    /// <summary>
    /// Content that is expanded visible.
    /// </summary>
    [Parameter]
    public RenderFragment? Items { get; set; }

    /// <summary>
    /// <para>Fires every time after <see cref="Expanded">Expanded</see> got set.</para>
    /// <para>Value is equal <see cref="Expanded">Expanded</see>.</para>
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

    private bool _expanded = false;
    /// <summary>
    /// Value for expanding or collapsing this submenu.
    /// </summary>
    /// <param name="value">expanded</param>
    public bool Expanded {
        get => _expanded;
        set {
            _expanded = value;
            OnToggle.InvokeAsync(value);
            InvokeAsync(StateHasChanged);
        }
    }

    private readonly TouchClick dropRightTC;

    public ContextSubMenu() => dropRightTC = new TouchClick(PhoneDropRightHandler);


    private void PhoneDropRightHandler(EventArgs e) => Toggle();

    /// <summary>
    /// <para>Expands/Collapses this menu.</para>
    /// <para>Shorthand for: <see cref="Expanded">Expanded</see> = !<see cref="Expanded">Expanded</see>;</para>
    /// </summary>
    public void Toggle() => Expanded = !Expanded;
}
