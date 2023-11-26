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
    /// <para>Default is false.</para>
    /// </summary>
    [Parameter]
    public bool StartExpanded { get; set; } = false;

    /// <summary>
    /// <para>Fires every time when collapse state is changed.</para>
    /// <para>Parameter indicates if the component is currently collapsed.</para>
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }

    private bool _expanded;
    /// <summary>
    /// The state of collapsed or expanded.
    /// </summary>
    public bool Expanded {
        get => _expanded;
        set {
            if (value != _expanded) {
                _expanded = value;
                OnExpandedChanged.InvokeAsync(value);
                InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// Sets the state of <see cref="Expanded"/> without notifying <see cref="OnExpandedChanged"/>.
    /// </summary>
    public bool SilentExpandedSetter {
        set {
            _expanded = value;
            InvokeAsync(StateHasChanged);
        }
    }


    protected override void OnInitialized() => _expanded = StartExpanded;
}
