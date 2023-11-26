namespace NoJsBlazor;

/// <summary>
/// A container that contains a nested list of items/links.<br />
/// It automatically collapses to phone view at a configurable threshold.<br />
/// Optional it can also show a brand.
/// </summary>
public sealed partial class NavBar : ListholdingComponentBase<NavBarMenu> {
    /// <summary>
    /// <para>Content which represents your site.</para>
    /// <para>If it is null, the corresponding parts are not rendered.</para>
    /// </summary>
    [Parameter]
    public RenderFragment? Brand { get; set; }

    /// <summary>
    /// <para>This should be a list of <see cref="NavBarMenu"/> of <see cref="NavBarItem"/>/<see cref="NavBarLink"/> objects.</para>
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment Items { get; set; }

    /// <summary>
    /// <para>Changes at the given width between phone and desktop view.</para>
    /// <para>Default is <see cref="NavBarBreakpoint.none"/>.</para>
    /// </summary>
    [Parameter]
    public NavBarBreakpoint Breakpoint { get; set; } = NavBarBreakpoint.none;

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
    public IDictionary<string, object>? Attributes { get; set; }


    private bool _expanded = false;
    /// <summary>
    /// Value for Expanding or Collapsing the navbar.
    /// </summary>
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
    /// Sets the state of <see cref="Expanded"/> without notifying <see cref="OnToggle"/>.
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
    
    /// <summary>
    /// Collapses this menu and all expanded submenus.
    /// </summary>
    public void Reset() {
        foreach (NavBarMenu navBar in childList)
            navBar.Expanded = false;

        Expanded = false;
    }


    private string NavRootClass => Breakpoint switch {
        NavBarBreakpoint.none => "nav-root",
        NavBarBreakpoint.px500 => "nav-root nav500px",
        NavBarBreakpoint.px600 => "nav-root nav600px",
        NavBarBreakpoint.px700 => "nav-root nav700px",
        NavBarBreakpoint.px800 => "nav-root nav800px",
        NavBarBreakpoint.px900 => "nav-root nav900px",
        NavBarBreakpoint.px1000 => "nav-root nav1000px",
        NavBarBreakpoint.px1100 => "nav-root nav1100px",
        NavBarBreakpoint.px1200 => "nav-root nav1200px",
        NavBarBreakpoint.px1300 => "nav-root nav1300px",
        NavBarBreakpoint.px1400 => "nav-root nav1400px",
        NavBarBreakpoint.px1500 => "nav-root nav1500px",
        NavBarBreakpoint.em30 => "nav-root nav30em",
        NavBarBreakpoint.em40 => "nav-root nav40em",
        NavBarBreakpoint.em50 => "nav-root nav50em",
        NavBarBreakpoint.em60 => "nav-root nav60em",
        NavBarBreakpoint.em70 => "nav-root nav70em",
        NavBarBreakpoint.em80 => "nav-root nav80em",
        NavBarBreakpoint.em90 => "nav-root nav90em",
        NavBarBreakpoint.em100 => "nav-root nav100em",
        _ => "nav-root"
    };
    
}
