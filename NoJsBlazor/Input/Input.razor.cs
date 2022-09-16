namespace NoJsBlazor;

/// <summary>
/// <para>A normal input field with a embedded label.</para>
/// <para>Don't use it inside EditForm, use <see cref="EditFormInput">EditFormInput</see> instead.</para>
/// </summary>
public sealed partial class Input : ComponentBase {
    private string? _value;
    /// <summary>
    /// Value of this Input field.
    /// </summary>
    [Parameter]
    public string? Value {
        get => _value;
        set {
            if (value != _value) {
                _value = value;
                hasValue = !string.IsNullOrEmpty(Value);
                ValueChanged.InvokeAsync(Value);
            }
        }
    }

    /// <summary>
    /// Sets the state of <see cref="Value"/> without notifying <see cref="ValueChanged"/>.
    /// </summary>
    public string? SilentValueSetter {
        set {
            _value = value;
            hasValue = !string.IsNullOrEmpty(Value);
        }
    }


    /// <summary>
    /// Fires every time <see cref="Value"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// <para>sets properties Label, Id, Name and Autocomplete to the given string (Autocomplete to true)</para>
    /// <para>returns a comma seperated string with this 4 properties (Label,Id,Name,Autocomplete)</para>
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
    /// sets the "id"-attribute in input field and the "for"-attribute in the label.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// sets the "name"-attribute in the input field.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// sets the "autocomplete"-attribute in the input field.
    /// </summary>
    [Parameter]
    public bool Autocomplete { get; set; }

    /// <summary>
    /// These values are applied to the input field.
    /// </summary>
    [Parameter]
    public Dictionary<string, object>? InputAttributes { get; set; }

    /// <summary>
    /// Captures unmatched values. The values are applied to the outer div container and not to the input field
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }


    private bool hasValue;

    private void OnInput(ChangeEventArgs e) => Value = (string?)e.Value;
}
