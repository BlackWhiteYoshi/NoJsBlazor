namespace NoJsBlazor;

/// <summary>
/// Base Class for all ProgressBarComponents
/// </summary>
public abstract class ProgressBar : ComponentBase {
    /// <summary>
    /// <para>The relative amoint of the progress Bar</para>
    /// <para>0 means empty, 1 means full.</para>
    /// <para>Default is 0.0</para>
    /// </summary>
    [Parameter]
    public float Progress { get; set; } = 0.0f;

    /// <summary>
    /// <para>Displaying text of this component.</para>
    /// <para>Default is <see cref="string.Empty"/></para>
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Accessing <see cref="Progress"/> and <see cref="Text"/> together. Also rerenders the component.
    /// </summary>
    public (float bar, string text) Content {
        get => (Progress, Text);
        set {
            (Progress, Text) = value;
            InvokeAsync(StateHasChanged);
        }
    }
}
