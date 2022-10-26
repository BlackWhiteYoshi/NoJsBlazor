namespace NoJsBlazor;

/// <summary>
/// <para>A normal input field with a embedded label.</para>
/// <para>It can only be used inside EditForm.</para>
/// </summary>
public sealed partial class EditFormInput : InputBase<string> {
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
    /// e.g.<br></br>
    /// normal text (abcd)<br></br>
    /// password (****)<br></br>
    /// numberField (0123)<br></br>
    /// …
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
    public IDictionary<string, object>? InputAttributes { get; set; }


    protected override string? FormatValueAsString(string? value) => value;

    protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage) {
        result = value ?? string.Empty;
        validationErrorMessage = string.Empty;
        return true;
    }


    private bool HasValue => !string.IsNullOrEmpty(Value);

    private void OnInput(ChangeEventArgs e) {
        Value = (string?)e.Value;
        ValueChanged.InvokeAsync(Value);
    }
}
