using AngleSharp.Dom;

namespace UnitTest;

public partial class EditFormInputTest : TestContext {
    #region parameter

    [Fact]
    public void Title_Sets_Label_Id_Name_And_AutoComplete() {
        const string TEST_TEXT = "Test Text";

        (_, EditFormInput input) = RenderTitle(TEST_TEXT);

        Assert.Equal(TEST_TEXT, input.Label);
        Assert.Equal(TEST_TEXT, input.Id);
        Assert.Equal(TEST_TEXT, input.Name);
        Assert.True(input.Autocomplete);
    }

    [Fact]
    public void Label_Sets_Content_Of_Label() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderLabel(TEST_TEXT);

        IElement label = fragment.Find("label");
        Assert.Equal(TEST_TEXT, label.InnerHtml);
    }

    [Fact]
    public void Type_Sets_Attribute_Type() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderType(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr typeAttribute = inputTag.Attributes["type"]!;
        Assert.Equal(TEST_TEXT, typeAttribute.Value);
    }

    [Fact]
    public void Id_Sets_Attribute_Id() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderId(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr idAttribute = inputTag.Attributes["id"]!;
        Assert.Equal(TEST_TEXT, idAttribute.Value);
    }

    [Fact]
    public void Name_Sets_Attribute_Name() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderName(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr nameAttribute = inputTag.Attributes["name"]!;
        Assert.Equal(TEST_TEXT, nameAttribute.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Autocomplete_Sets_Attribute_Autocomplete(bool enabled) {
        (IRenderedFragment fragment, _) = RenderAutocomplete(enabled);

        IElement inputTag = fragment.Find("input");
        IAttr autocompleteAttribute = inputTag.Attributes["autocomplete"]!;
        Assert.Equal(enabled ? "on" : "off", autocompleteAttribute.Value);
    }

    #endregion


    #region events

    [Fact]
    public void Binding_Value_Sets_Model_Value() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment inputContainer, _, TestModel testModel) = RenderStandard();

        IElement inputTag = inputContainer.Find("input");
        inputTag.Input(TEST_TEXT);
        Assert.Equal(TEST_TEXT, testModel.Text);
    }

    #endregion
}
