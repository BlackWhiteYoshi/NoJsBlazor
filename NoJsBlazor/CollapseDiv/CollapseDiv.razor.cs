namespace NoJsBlazor;

/// <summary>
/// A container that content can be collapsed.
/// </summary>
public partial class CollapseDiv : ComponentBase {
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
    /// <para>Height in px of the head of the Component which also functions as the collapse/expand button.</para>
    /// <para>It is also the height of the Component when it is collapsed.</para>
    /// <para>Default is 62.</para>
    /// </summary>
    [Parameter]
    public int HeadHeight { get; set; } = 62;

    /// <summary>
    /// <para>Height of the Component when it is expanded.</para>
    /// <para>Default is 173.</para>
    /// </summary>
    [Parameter]
    public int ContentHeight { get; set; } = 173;

    /// <summary>
    /// Initializing collapsed or expanded.
    /// </summary>
    [Parameter]
    public bool StartCollapsed { get; init; } = true;

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
    public Dictionary<string, object>? Attributes { get; set; }

    private bool _collapsed;
    public bool Collapsed {
        get => _collapsed;
        set {
            _collapsed = value;
            OnCollapseChanged.InvokeAsync(value);
            InvokeAsync(StateHasChanged);
        }
    }


    private readonly TouchClick touchClick;

    public CollapseDiv() => touchClick = new TouchClick(ButtonDown);

    protected override void OnInitialized() => _collapsed = StartCollapsed;

    private void ButtonDown(EventArgs e) => Collapsed = !Collapsed;

    private object? AddStyles() {
        if (Attributes == null)
            return null;

        if (Attributes.TryGetValue("style", out object? styles))
            return styles;
        else
            return null;
    }
}
