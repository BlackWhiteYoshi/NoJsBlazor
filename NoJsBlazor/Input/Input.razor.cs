using InlineCompositionAttributes;

namespace NoJsBlazor;

/// <summary>
/// <para>Input field with a embedded label.</para>
/// <para>Don't use it inside EditForm, use <see cref="EditFormInput">EditFormInput</see> instead.</para>
/// </summary>
[Inline<InputBase>]
public sealed partial class Input : ComponentBase {
    private string? _value;
    /// <summary>
    /// <para>Value of this Input field.</para>
    /// <para>Should be used as two-way-binding.</para>
    /// </summary>
    [Parameter]
    public string? Value {
        get => _value;
        set {
            if (value != _value) {
                _value = value;
                hasValue = !string.IsNullOrEmpty(Value);
            }
        }
    }

    /// <summary>
    /// <para>Fires every time <see cref="Value"/> changes.</para>
    /// <para>This or <see cref="OnChange"/> can be used for two-way-binding.</para>
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }


    /// <summary>
    /// Captures unmatched values. The values are applied to the outer div container and not to the input field
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }


    private bool hasValue;
}
