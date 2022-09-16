namespace NoJsBlazor;

/// <summary>
/// <para>A menu that can be toggled and shown at mouse position.</para>
/// <para>Triggering the menu is not included.</para>
/// </summary>
public sealed partial class ContextMenu : ListholdingComponentBase<ContextSubMenu> {
    /// <summary>
    /// Content of this ContextMenu.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// <para>Fires every time when <see cref="Expanded">Expanded</see> get changed.</para>
    /// <para>Value is equal <see cref="Expanded">Expanded</see>.</para>
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }


    private bool _expanded = false;
    /// <summary>
    /// Value for expanding or collapsing this submenu.
    /// </summary>
    /// <param name="value">expanded</param>
    public bool Expanded {
        get => _expanded;
        private set {
            if (value != _expanded) {
                _expanded = value;
                OnToggle.InvokeAsync(value);
                InvokeAsync(StateHasChanged);
            }
        }
    }


    private double top;
    private double left;


    /// <summary>
    /// Display the context menu at the given location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Open(double x, double y) {
        left = x;
        top = y;
        Expanded = true;
    }

    /// <summary>
    /// Collapses all expanded submenus and closes the context menu.
    /// </summary>
    public void Close() {
        Reset();
        Expanded = false;
    }


    /// <summary>
    /// Display the context menu at the given location without notifying <see cref="OnToggle"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SilentOpen(double x, double y) {
        left = x;
        top = y;
        _expanded = true;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Collapses all expanded submenus and closes the context menu without notifying <see cref="OnToggle"/>.
    /// </summary>
    public void SilentClose() {
        Reset();
        _expanded = false;
        InvokeAsync(StateHasChanged);
    }
    
    
    /// <summary>
    /// Collapses all expanded submenus.
    /// </summary>
    public void Reset() {
        foreach (ContextSubMenu menu in childList)
            menu.Expanded = false;
    }


    private object? AddStyles() {
        if (Attributes == null)
            return null;

        if (Attributes.TryGetValue("style", out object? styles))
            return styles;
        else
            return null;
    }
}
