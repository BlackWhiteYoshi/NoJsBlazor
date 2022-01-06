namespace NoJsBlazor;

/// <summary>
/// <para>An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.</para>
/// <para>The value of this object is from type <see cref="int"/>.</para>
/// </summary>
public partial class Slider : ComponentBase {
    /// <summary>
    /// Value of the Slider
    /// </summary>
    [Parameter]
    public int Value { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:</para>
    /// <para>LeftButton, RightButton, Slider or EditField.</para>
    /// </summary>
    [Parameter]
    public EventCallback<int> ValueChanged { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:</para>
    /// <para>LeftButton, RightButton, Slider (only on release) or EditField.</para>
    /// </summary>
    [Parameter]
    public EventCallback<int> OnChange { get; set; }

    /// <summary>
    /// An optional label.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// <para>Slider lower bounds.</para>
    /// <para>Default is 0.</para>
    /// </summary>
    [Parameter]
    public int Min { get; set; } = 0;

    /// <summary>
    /// <para>Slider upper bounds.</para>
    /// <para>Default is 10.</para>
    /// </summary>
    [Parameter]
    public int Max { get; set; } = 10;

    /// <summary>
    /// <para>Slider precision</para>
    /// <para>Default is 1.</para>
    /// </summary>
    [Parameter]
    public int Step { get; set; } = 1;

    /// <summary>
    /// <para>Indicates if the user is able to edit the number directly.</para>
    /// <para>Technically the number is displayed in a input field instead of a label.</para>
    /// <para>Dafault is false.</para>
    /// </summary>
    [Parameter]
    public bool Editable { get; set; } = false;

    /// <summary>
    /// <para>Content inside the left Button.</para>
    /// <para>Default is a svg showing "🡸".</para>
    /// </summary>
    [Parameter]
    public RenderFragment LeftButtonContent { get; set; }

    /// <summary>
    /// <para>Content inside the right Button.</para>
    /// <para>Default is a svg showing "🡺".</para>
    /// </summary>
    [Parameter]
    public RenderFragment RightButtonContent { get; set; }

    /// <summary>
    /// <para>The way the value should be printed.</para>
    /// <para>Default is value.ToString().</para>
    /// </summary>
    [Parameter]
    public Func<int, string> Display { get; set; }

    /// <summary>
    /// <para>Parsing function for the edit field.</para>
    /// <para>It gets the content of the edit field as string and returns the appropriated integer. It Returns null if the value is not valid.</para>
    /// <para>Default succeed if the value is int-parseable and in Min/Max bounds.</para>
    /// </summary>
    [Parameter]
    public Func<string?, int?> ParseEdit { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private readonly TouchClick leftButtonTC;
    private readonly TouchClick rightButtonTC;


    public Slider() {
        leftButtonTC = new TouchClick(LeftButton);
        rightButtonTC = new TouchClick(RightButton);

        LeftButtonContent = DefaultLeftButton;
        RightButtonContent = DefaultRightButton;
        Display = DefaultDisplay;
        ParseEdit = DefaultParseEdit;
    }


    #region private Methods

    private void LeftButton(EventArgs e) {
        if (Value > Min) {
            Value -= Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }

    private void RightButton(EventArgs e) {
        if (Value < Max) {
            Value += Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }


    private void OnInputSlider(ChangeEventArgs e) {
        Value = int.Parse((string)e.Value!);
        ValueChanged.InvokeAsync(Value);
    }

    private void OnChangeSlider(ChangeEventArgs e) => OnChange.InvokeAsync(Value);


    private void OnChangeEditField(ChangeEventArgs input) {
        int? buffer = ParseEdit((string?)input.Value);
        if (buffer != null) {
            Value = (int)buffer;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }



    private static string DefaultDisplay(int value) => value.ToString();

    private int? DefaultParseEdit(string? input) {
        if (int.TryParse(input, out int result))
            if (result < Min)
                return Min;
            else if (Max < result)
                return Max;
            else
                return result;

        return null;
    }

    #endregion
}
