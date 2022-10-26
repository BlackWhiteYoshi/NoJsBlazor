namespace NoJsBlazor;

/// <summary>
/// <para>An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.</para>
/// <para>The value of this object is from type <see cref="decimal"/>.</para>
/// </summary>
public sealed partial class DecimalSlider : ComponentBase {
    /// <summary>
    /// Value of the Slider
    /// </summary>
    [Parameter]
    public decimal Value { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:</para>
    /// <para>LeftButton, RightButton, Slider or EditField.</para>
    /// </summary>
    [Parameter]
    public EventCallback<decimal> ValueChanged { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:</para>
    /// <para>LeftButton, RightButton, Slider (only on release) or EditField.</para>
    /// </summary>
    [Parameter]
    public EventCallback<decimal> OnChange { get; set; }

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
    public decimal Min { get; set; } = 0m;

    /// <summary>
    /// <para>Slider upper bounds.</para>
    /// <para>Default is 10.</para>
    /// </summary>
    [Parameter]
    public decimal Max { get; set; } = 10m;

    /// <summary>
    /// <para>Slider precision</para>
    /// <para>Default is 0.1.</para>
    /// </summary>
    [Parameter]
    public decimal Step { get; set; } = 0.1m;

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
    public RenderFragment LeftButtonContent { get; set; } = DefaultLeftButton;

    /// <summary>
    /// <para>Content inside the right Button.</para>
    /// <para>Default is a svg showing "🡺".</para>
    /// </summary>
    [Parameter]
    public RenderFragment RightButtonContent { get; set; } = DefaultRightButton;

    /// <summary>
    /// <para>The way the value should be printed.</para>
    /// <para>Default is value.ToString().</para>
    /// </summary>
    [Parameter]
    public Func<decimal, string> Display { get; set; } = DefaultDisplay;

    /// <summary>
    /// <para>Parsing function for the edit field.</para>
    /// <para>It gets the content of the edit field as string and returns the appropriated decimal number. It Returns null if the value is not valid.</para>
    /// <para>Default succeed if the value is decimal-parseable and in Min/Max bounds.</para>
    /// </summary>
    [Parameter]
    public Func<string?, decimal?> ParseEdit { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }


    public DecimalSlider() {
        ParseEdit = DefaultParseEdit;
    }


    #region private Methods

    private void LeftButton(MouseEventArgs e) {
        if (Value > Min) {
            Value -= Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }

    private void RightButton(MouseEventArgs e) {
        if (Value < Max) {
            Value += Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }


    private void OnInputSlider(ChangeEventArgs e) {
        Value = decimal.Parse((string)e.Value!);
        ValueChanged.InvokeAsync(Value);
    }

    private void OnChangeSlider(ChangeEventArgs e) => OnChange.InvokeAsync(Value);


    private void OnChangeEditField(ChangeEventArgs input) {
        decimal? buffer = ParseEdit((string?)input.Value);
        if (buffer != null) {
            Value = (decimal)buffer;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }


    private static string DefaultDisplay(decimal value) => value.ToString();

    private decimal? DefaultParseEdit(string? input) {
        if (decimal.TryParse(input, out decimal result))
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
