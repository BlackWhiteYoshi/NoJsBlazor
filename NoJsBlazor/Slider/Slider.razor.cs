using System.Globalization;
using System.Numerics;

namespace NoJsBlazor;

/// <summary>
/// An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.
/// </summary>
public sealed partial class Slider<Type> : ComponentBase where Type : struct, INumber<Type> {
    /// <summary>
    /// <para>Value of the Slider.</para>
    /// <para>Should be used as two-way-binding.</para>
    /// </summary>
    [Parameter]
    public Type Value { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:<br />
    /// LeftButton, RightButton, Slider or EditField.</para>
    /// <para>This or <see cref="OnChange"/> can be used for two-way-binding.</para>
    /// </summary>
    [Parameter]
    public EventCallback<Type> ValueChanged { get; set; }

    /// <summary>
    /// <para>Invokes every time Value get changed with:<br />
    /// LeftButton, RightButton, Slider (only on release) or EditField.</para>
    /// <para>This or <see cref="ValueChanged"/> can be used for two-way-binding.</para>
    /// </summary>
    [Parameter]
    public EventCallback<Type> OnChange { get; set; }

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
    public Type Min { get; set; } = Type.Zero;

    /// <summary>
    /// <para>Slider upper bounds.</para>
    /// <para>Default is 10.</para>
    /// </summary>
    [Parameter]
    public Type Max { get; set; } = Type.One + Type.One + Type.One + Type.One + Type.One + Type.One + Type.One + Type.One + Type.One + Type.One; // 10

    /// <summary>
    /// <para>Slider precision</para>
    /// <para>Default is 1.</para>
    /// </summary>
    [Parameter]
    public Type Step { get; set; } = Type.One;

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
    public Func<Type, string> Display { get; set; } = DefaultDisplay;

    /// <summary>
    /// <para>Parsing function for the edit field.</para>
    /// <para>It should get the content of the edit field as string and return the appropriated number. It should return null if the value is not valid.</para>
    /// <para>Default try parses number and when succeed, clamps to Min,Max-bounds.</para>
    /// </summary>
    [Parameter]
    public Func<string?, Type?> ParseEdit { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }


    public Slider() {
        ParseEdit = DefaultParseEdit;
    }


    #region private Methods

    private void OnLeftButton(MouseEventArgs e) {
        if (Value > Min) {
            Value -= Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }

    private void OnRightButton(MouseEventArgs e) {
        if (Value < Max) {
            Value += Step;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }


    private void OnInputSlider(ChangeEventArgs e) {
        Value = Type.Parse((string)e.Value!, CultureInfo.CurrentCulture);
        ValueChanged.InvokeAsync(Value);
    }

    private void OnChangeSlider(ChangeEventArgs e) => OnChange.InvokeAsync(Value);


    private void OnChangeEditField(ChangeEventArgs input) {
        Type? buffer = ParseEdit((string?)input.Value);
        if (buffer != null) {
            Value = (Type)buffer;
            ValueChanged.InvokeAsync(Value);
            OnChange.InvokeAsync(Value);
        }
    }



    private static string DefaultDisplay(Type value) => value.ToString() ?? string.Empty;

    private Type? DefaultParseEdit(string? input) {
        if (Type.TryParse(input, CultureInfo.CurrentCulture, out Type result))
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
