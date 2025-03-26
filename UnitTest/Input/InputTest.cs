using AngleSharp.Dom;

namespace UnitTest;

public sealed class InputTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Value_Sets_ValueAttrubute() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Value, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr valueAttribute = inputTag.Attributes["value"]!;
        await Assert.That(valueAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Title_Sets_Label_Id_Name_And_AutoComplete() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Title, TEST_TEXT);
        });
        Input input = inputContainer.Instance;

        await Assert.That(input.Label).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Id).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Name).IsEqualTo(TEST_TEXT);
        await Assert.That(input.Autocomplete).IsTrue();
    }

    [Test]
    public async ValueTask Label_Sets_Content_Of_Label() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Label, TEST_TEXT);
        });

        IElement label = inputContainer.Find("label");
        await Assert.That(label.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Type_Sets_Attribute_Type() {
        const string TEST_TEXT = "password";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Type, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr typeAttribute = inputTag.Attributes["type"]!;
        await Assert.That(typeAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Id_Sets_Attribute_Id() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Id, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr idAttribute = inputTag.Attributes["id"]!;
        await Assert.That(idAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Name_Sets_Attribute_Name() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Name, TEST_TEXT);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr nameAttribute = inputTag.Attributes["name"]!;
        await Assert.That(nameAttribute.Value).IsEqualTo(TEST_TEXT);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask Autocomplete_Sets_Attribute_Autocomplete(bool enabled) {
        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.Autocomplete, enabled);
        });

        IElement inputTag = inputContainer.Find("input");
        IAttr autocompleteAttribute = inputTag.Attributes["autocomplete"]!;
        await Assert.That(autocompleteAttribute.Value).IsEqualTo(enabled ? "on" : "off");
    }

    #endregion


    #region events

    [Test]
    public async ValueTask ValueChanged_Fires_When_Input_Changed() {
        int fired = 0;

        IRenderedComponent<Input> inputContainer = RenderComponent((ComponentParameterCollectionBuilder<Input> builder) => {
            builder.Add((Input input) => input.ValueChanged, (string? value) => fired++);
        });

        IElement inputTag = inputContainer.Find("input");
        inputTag.Input("A");
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
