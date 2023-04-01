namespace NoJsBlazor;

/// <summary>
/// <para>Input field with a embedded label.</para>
/// <para>Don't use it inside EditForm, use <see cref="EditFormInput">EditFormInput</see> instead.</para>
/// </summary>
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
    /// <para>Fires every time item looses focus and value has changed.</para>
    /// <para>This or <see cref="ValueChanged"/> can be used for two-way-binding.</para>
    /// </summary>
    [Parameter]
    public EventCallback<string?> OnChange { get; set; }


    /// <summary>
    /// <para>Sets properties Label, Id, Name and Autocomplete to the given string (Autocomplete to true).</para>
    /// <para>Returns a comma seperated string with this 4 properties (Label,Id,Name,Autocomplete).</para>
    /// </summary>
    [Parameter]
    public string Title {
        get => $"{Label},{Id},{Name},{Autocomplete}";
        set {
            Label = value;
            Id = value;
            Name = value;
            Autocomplete = true;
        }
    }

    /// <summary>
    /// Displayed text of this Input field.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Sets the type attribute.
    /// </summary>
    [Parameter]
    public string? Type { get; set; }

    /// <summary>
    /// Sets the "id"-attribute in input field and the "for"-attribute in the label.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Sets the "name"-attribute in the input field.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// <para>Sets the "autocomplete"-attribute in the input field.</para>
    /// <para>default is true.</para>
    /// </summary>
    [Parameter]
    public bool Autocomplete { get; set; } = true;

    /// <summary>
    /// These values are applied to the input field.
    /// </summary>
    [Parameter]
    public IDictionary<string, object>? InputAttributes { get; set; }

    /// <summary>
    /// Captures unmatched values. The values are applied to the outer div container and not to the input field
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }


    private bool hasValue;


    private void OnFieldInput(ChangeEventArgs e) {
        Value = (string?)e.Value;
        ValueChanged.InvokeAsync(Value);
    }

    private void OnFieldChange(ChangeEventArgs e) => OnChange.InvokeAsync((string?)e.Value);
}
