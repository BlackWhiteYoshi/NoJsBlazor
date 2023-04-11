using InlineCompositionAttributes;

namespace NoJsBlazor;

[InlineBase]
public abstract class InputBase {
    [NoInline]
    public string? Value { get; set; }

    [NoInline]
    public EventCallback<string?> ValueChanged { get; set; }


    /// <summary>
    /// <para>Fires every time item looses focus and value has changed.</para>
    /// <para>This or ValueChanged can be used for two-way-binding.</para>
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


    private void OnFieldInput(ChangeEventArgs e) {
        Value = (string?)e.Value;
        ValueChanged.InvokeAsync(Value);
    }

    private void OnFieldChange(ChangeEventArgs e) => OnChange.InvokeAsync((string?)e.Value);
}
