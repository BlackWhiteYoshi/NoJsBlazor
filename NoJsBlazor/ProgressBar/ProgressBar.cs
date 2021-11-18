namespace NoJsBlazor;

/// <summary>
/// Base Class for all ProgressBarComponents
/// </summary>
public abstract class ProgressBar : ComponentBase {
    private float _progress = 0.0f;
    /// <summary>
    /// <para>The relative amoint of the progress Bar</para>
    /// <para>0 means empty, 1 means full.</para>
    /// <para>Default is 0.0</para>
    /// </summary>
    public float Progress {
        get => _progress;
        set {
            _progress = value;
            InvokeAsync(StateHasChanged);
        }
    }

    private string _text = string.Empty;
    /// <summary>
    /// <para>Displaying text of this component.</para>
    /// <para>Default is <see cref="string.Empty"/></para>
    /// </summary>
    public string Text {
        get => _text;
        set {
            _text = value;
            InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Accessing <see cref="Progress"/> and <see cref="Text"/> together.
    /// </summary>
    public (float bar, string text) Content {
        get => (Progress, Text);
        set {
            (Progress, Text) = value;
            InvokeAsync(StateHasChanged);
        }
    }
}
