using AngleSharp.Dom;

namespace UnitTest;

public sealed class InputTest : TestContext {
    #region parameter

    [Fact]
    public void Value_Sets_ValueAttrubute() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Value, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr valueAttribute = inputTag.Attributes["value"]!;
        Assert.Equal(TEST_TEXT, valueAttribute.Value);
    }

    [Fact]
    public void Title_Sets_Label_Id_Name_And_AutoComplete() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Title, TEST_TEXT);
        });
        Input input = inputContainer.Instance;

        Assert.Equal(TEST_TEXT, input.Label);
        Assert.Equal(TEST_TEXT, input.Id);
        Assert.Equal(TEST_TEXT, input.Name);
        Assert.True(input.Autocomplete);
    }

    [Fact]
    public void Label_Sets_Content_Of_Label() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Label, TEST_TEXT);
        });

        IElement label = inputContainer.Find("label");
        Assert.Equal(TEST_TEXT, label.InnerHtml);
    }

    [Fact]
    public void Type_Sets_Attribute_Type() {
        const string TEST_TEXT = "password";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Type, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr typeAttribute = inputTag.Attributes["type"]!;
        Assert.Equal(TEST_TEXT, typeAttribute.Value);
    }

    [Fact]
    public void Id_Sets_Attribute_Id() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Id, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr idAttribute = inputTag.Attributes["id"]!;
        Assert.Equal(TEST_TEXT, idAttribute.Value);
    }

    [Fact]
    public void Name_Sets_Attribute_Name() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Name, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr nameAttribute = inputTag.Attributes["name"]!;
        Assert.Equal(TEST_TEXT, nameAttribute.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Autocomplete_Sets_Attribute_Autocomplete(bool enabled) {
        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Autocomplete, enabled);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr autocompleteAttribute = inputTag.Attributes["autocomplete"]!;
        Assert.Equal(enabled ? "on" : "off", autocompleteAttribute.Value);
    }

    #endregion


    #region events

    [Fact]
    public void ValueChanged_Fires_When_Input_Changed() {
        int fired = 0;

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.ValueChanged, (string? value) => fired++);
        });

        IElement inputTag = inputContainer.Find("input");
        inputTag.Input("A");
        Assert.Equal(1, fired);
    }

    #endregion
}
