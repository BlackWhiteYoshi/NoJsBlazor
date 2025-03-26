using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class EditFormInputTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Title_Sets_Label_Id_Name_And_AutoComplete() {
        const string TEST_TEXT = "Test Text";

        (_, EditFormInput input) = RenderTitle(TEST_TEXT);

        await Assert.That(input.Label).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Id).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Name).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Autocomplete).IsTrue();
    }

    [Test]
    public async ValueTask Label_Sets_Content_Of_Label() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderLabel(TEST_TEXT);

        IElement label = fragment.Find("label");
        await Assert.That(label.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Type_Sets_Attribute_Type() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderType(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr typeAttribute = inputTag.Attributes["type"]!;
        await Assert.That(typeAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Id_Sets_Attribute_Id() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderId(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr idAttribute = inputTag.Attributes["id"]!;
        await Assert.That(idAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Name_Sets_Attribute_Name() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment fragment, _) = RenderName(TEST_TEXT);

        IElement inputTag = fragment.Find("input");
        IAttr nameAttribute = inputTag.Attributes["name"]!;
        await Assert.That(nameAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask Autocomplete_Sets_Attribute_Autocomplete(bool enabled) {
        (IRenderedFragment fragment, _) = RenderAutocomplete(enabled);

        IElement inputTag = fragment.Find("input");
        IAttr autocompleteAttribute = inputTag.Attributes["autocomplete"]!;
        await Assert.That(autocompleteAttribute.Value).IsEqualTo(enabled ? "on" : "off");
    }

    #endregion


    #region events

    [Test]
    public async ValueTask Binding_Value_Sets_Model_Value() {
        const string TEST_TEXT = "Test Text";

        (IRenderedFragment inputContainer, _, TestModel testModel) = RenderStandard();

        IElement inputTag = inputContainer.Find("input");
        inputTag.Input(TEST_TEXT);
        await Assert.That(testModel.Text).IsEqualTo(TEST_TEXT);
    }

    #endregion
}
