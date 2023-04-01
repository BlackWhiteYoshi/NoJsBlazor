namespace NoJsBlazor;

/// <summary>
/// <para>A single menu that can hold other <see cref="NavBarMenu"/> and <see cref="NavBarItem"/>/<see cref="NavBarLink"/>.</para>
/// <para>This should be placed inside of a <see cref="NavBar"/> or <see cref="NavBarMenu"/>.</para>
/// </summary>
public sealed partial class NavBarMenu : ListableComponentBase<NavBarMenu> {
    /// <summary>
    /// Content that is collapsed visible.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment Head { get; set; }

    /// <summary>
    /// Content that is expanded visible.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment Items { get; set; }

    /// <summary>
    /// <para>Fires every time when <see cref="Expanded">Expanded</see> get changed.</para>
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
            if (value != _expanded) {
                _expanded = value;
                OnToggle.InvokeAsync(value);
                InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// Sets the state of <see cref="Expanded"/> without notifying <see cref="OnToggle"/> of this submenu.
    /// </summary>
    public bool SilentExpandedSetter {
        set {
            _expanded = value;
            InvokeAsync(StateHasChanged);
        }
    }


    /// <summary>
    /// <para>Expands/Collapses this menu.</para>
    /// <para>Shorthand for: <see cref="Expanded">Expanded</see> = !<see cref="Expanded">Expanded</see>;</para>
    /// </summary>
    public void Toggle() => Expanded = !Expanded;
}
