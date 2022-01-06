namespace NoJsBlazor;

/// <summary>
/// A container that can display your brand, the content can be collapsed and contains <see cref="NavBarMenu"/> or/and <see cref="NavBarItem"/>/<see cref="NavBarLink"/>.
/// </summary>
public partial class NavBar : ListholdingComponentBase<NavBarMenu> {
    /// <summary>
    /// <para>Content which represents your site.</para>
    /// <para>If it is null, the corresponding parts are not rendered.</para>
    /// </summary>
    [Parameter]
    public RenderFragment? Brand { get; set; }

    /// <summary>
    /// <para>This should be a list of <see cref="NavBarMenu"/> of <see cref="NavBarItem"/>/<see cref="NavBarLink"/> objects.</para>
    /// </summary>
    [Parameter]
    public RenderFragment? Items { get; set; }

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
    public Dictionary<string, object>? Attributes { get; set; }


    private bool _expanded = false;
    /// <summary>
    /// Value for Expanding or Collapsing the navbar
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


    [AllowNull]
    private string navRootClass;

    protected override void OnInitialized() {
        navRootClass = Breakpoint switch {
            NavBarBreakpoint.none => "nav-root",
            NavBarBreakpoint.px100 => "nav-root-100",
            NavBarBreakpoint.px200 => "nav-root-200",
            NavBarBreakpoint.px300 => "nav-root-300",
            NavBarBreakpoint.px400 => "nav-root-400",
            NavBarBreakpoint.px500 => "nav-root-500",
            NavBarBreakpoint.px600 => "nav-root-600",
            NavBarBreakpoint.px700 => "nav-root-700",
            NavBarBreakpoint.px800 => "nav-root-800",
            NavBarBreakpoint.px900 => "nav-root-900",
            NavBarBreakpoint.px1000 => "nav-root-1000",
            NavBarBreakpoint.px1100 => "nav-root-1100",
            NavBarBreakpoint.px1200 => "nav-root-1200",
            NavBarBreakpoint.px1300 => "nav-root-1300",
            NavBarBreakpoint.px1400 => "nav-root-1400",
            NavBarBreakpoint.px1500 => "nav-root-1500",
            _ => "nav-root"
        };
    }
    
    
    private readonly TouchClick ToggleTC;

    public NavBar() => ToggleTC = new TouchClick(ToggleNavBar);

    private void ToggleNavBar(EventArgs e) {
        if (Expanded)
            Reset();
        else
            Expanded = true;
    }


    /// <summary>
    /// Collapses all expanded menus and submenus.
    /// </summary>
    public void Reset() {
        foreach (NavBarMenu navBar in childList)
            navBar.Expanded = false;

        Expanded = false;
    }
}
