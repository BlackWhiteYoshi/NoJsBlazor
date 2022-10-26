namespace NoJsBlazor;

/// <summary>
/// A container that content can be collapsed.
/// </summary>
public sealed partial class CollapseDiv : ComponentBase {
    /// <summary>
    /// <para>Content that is also visible collapsed.</para>
    /// <para>If clicked on it, it will expand/collapse.</para>
    /// </summary>
    [Parameter]
    public RenderFragment? Head { get; set; }

    /// <summary>
    /// Content that is hidden when collapsed.
    /// </summary>
    [Parameter]
    public RenderFragment? Content { get; set; }

    /// <summary>
    /// <para>Initializing collapsed or expanded.</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool StartCollapsed { get; set; } = true;

    /// <summary>
    /// <para>Fires every time when collapse state is changed.</para>
    /// <para>Parameter indicates if the component is currently collapsed.</para>
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnCollapseChanged { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }

    private bool _collapsed;
    /// <summary>
    /// The state of collapsed or expanded.
    /// </summary>
    public bool Collapsed {
        get => _collapsed;
        set {
            if (value != _collapsed) {
                _collapsed = value;
                OnCollapseChanged.InvokeAsync(value);
                InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// Sets the state of <see cref="Collapsed"/> without notifying <see cref="OnCollapseChanged"/>.
    /// </summary>
    public bool SilentCollapsedSetter {
        set {
            _collapsed = value;
            InvokeAsync(StateHasChanged);
        }
    }


    protected override void OnInitialized() => _collapsed = StartCollapsed;

    private void HeadClick(MouseEventArgs e) => Collapsed = !Collapsed;
}
