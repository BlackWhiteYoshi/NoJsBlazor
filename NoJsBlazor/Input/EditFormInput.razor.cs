using InlineCompositionAttributes;

namespace NoJsBlazor;

/// <summary>
/// <para>Input field with a embedded label.</para>
/// <para>It can only be used inside EditForm.</para>
/// </summary>
[Inline<InputBase>]
public sealed partial class EditFormInput : InputBase<string> {
    protected override string? FormatValueAsString(string? value) => value;

    protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage) {
        result = value ?? string.Empty;
        validationErrorMessage = string.Empty;
        return true;
    }
}
