using AngleSharp.Dom;

namespace UnitTest;

public sealed class SliderTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Value_Is_Rendered_Inside_Label() {
        const int VALUE = 5;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Value, VALUE);
        });

        IElement label = sliderContainer.Find(".slider-buttons label");
        await Assert.That(label.InnerHtml).IsEqualTo(VALUE.ToString());
    }

    [Test]
    public async ValueTask Title_Is_Rendered_Inside_First_Label() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Title, TEST_TEXT);
        });

        IElement label = sliderContainer.FindAll("label")[0];
        await Assert.That(label.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Title_Is_Not_Rendered_When_Empty() {
        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Title, string.Empty);
        });

        await Assert.That(sliderContainer.FindAll("label")).HasSingleItem();
    }

    [Test]
    public async ValueTask Min_Sets_MinAttribute() {
        const int MIN = 3;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Min, MIN);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr min = slider.Attributes["min"]!;
        await Assert.That(min.Value).IsEqualTo(MIN.ToString());
    }

    [Test]
    public async ValueTask Max_Sets_MaxAttribute() {
        const int MAX = 3;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Max, MAX);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr max = slider.Attributes["max"]!;
        await Assert.That(max.Value).IsEqualTo(MAX.ToString());
    }

    [Test]
    public async ValueTask Step_Sets_StepAttribute() {
        const int STEP = 3;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Step, STEP);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr step = slider.Attributes["step"]!;
        await Assert.That(step.Value).IsEqualTo(STEP.ToString());
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask Enable_Stes_DisabledAttribute(bool enabled) {
        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Enabled, enabled);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr? disabled = slider.Attributes["disabled"];
        if (enabled)
            await Assert.That(disabled).IsNull();
        else
            await Assert.That(disabled).IsNotNull();
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask Editable_Renderes_InputField(bool editable) {
        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Editable, editable);
        });

        IRefreshableElementCollection<IElement> inputFields = sliderContainer.FindAll("input");
        // slider is also an inputField
        await Assert.That(inputFields.Count).IsEqualTo(editable ? 2 : 1);
    }

    [Test]
    public async ValueTask LeftButtonContent_Is_Rendered_Inside_LeftButton() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.LeftButtonContent, (RenderTreeBuilder builder) => builder.AddContent(0, TEST_TEXT));
        });

        IElement button = sliderContainer.Find("button");
        await Assert.That(button.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask RightButtonContent_Is_Rendered_Inside_RightButton() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.RightButtonContent, (RenderTreeBuilder builder) => builder.AddContent(0, TEST_TEXT));
        });

        IElement button = sliderContainer.FindAll("button")[1];
        await Assert.That(button.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask Display_Changes_Label_Output() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Display, (int value) => TEST_TEXT);
        });

        IElement label = sliderContainer.Find(".slider-buttons label");
        await Assert.That(label.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    [Test]
    public async ValueTask ParseEdit_Reads_In_EditBox() {
        const int VALUE = 6;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Editable, true);
            builder.Add((Slider<int> slider) => slider.ParseEdit, (string? input) => VALUE);
        });
        Slider<int> slider = sliderContainer.Instance;

        IElement input = sliderContainer.Find("input");
        input.Change("doesn't matter because ParseEdit outputs always the same");
        await Assert.That(slider.Value).IsEqualTo(VALUE);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask ValueChanged_Is_Fired_When_Value_Is_Modified_By_User() {
        int fired = 0;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Value, 1);
            builder.Add((Slider<int> slider) => slider.Min, 0);
            builder.Add((Slider<int> slider) => slider.Max, 2);
            builder.Add((Slider<int> slider) => slider.ValueChanged, (int value) => fired++);
        });

        IElement button = sliderContainer.Find("button");
        button.Click();
        await Assert.That(fired).IsEqualTo(1);
    }

    [Test]
    public async ValueTask OnChange_Is_Fired_When_Value_Is_Modified_By_User() {
        int fired = 0;

        IRenderedComponent<Slider<int>> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider<int>> builder) => {
            builder.Add((Slider<int> slider) => slider.Value, 1);
            builder.Add((Slider<int> slider) => slider.Min, 0);
            builder.Add((Slider<int> slider) => slider.Max, 2);
            builder.Add((Slider<int> slider) => slider.OnChange, (int value) => fired++);
        });

        IElement button = sliderContainer.Find("button");
        button.Click();
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
