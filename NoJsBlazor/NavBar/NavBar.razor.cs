namespace NoJsBlazor;

/// <summary>
/// A container that can display your brand, the content can be collapsed and contains <see cref="NavBarMenu"/> or/and <see cref="NavBarItem"/>/<see cref="NavBarLink"/>.
/// </summary>
public partial class NavBar : ListholdingComponentBase<NavBarMenu> {
    #region Parameters

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

    #endregion

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
