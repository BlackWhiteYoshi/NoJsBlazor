﻿using AngleSharp.Dom;

namespace UnitTest;

public class SliderTest : TestContext {
    #region parameter

    [Fact]
    public void Value_Is_Rendered_Inside_Label() {
        const int VALUE = 5;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Value, VALUE);
        });

        IElement label = sliderContainer.Find(".slider-buttons label");
        Assert.Equal(VALUE.ToString(), label.InnerHtml);
    }

    [Fact]
    public void Title_Is_Rendered_Inside_First_Label() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Title, TEST_TEXT);
        });

        IElement label = sliderContainer.FindAll("label")[0];
        Assert.Equal(TEST_TEXT, label.InnerHtml);
    }

    [Fact]
    public void Title_Is_Not_Rendered_When_Empty() {
        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Title, string.Empty);
        });

        Assert.Single(sliderContainer.FindAll("label"));
    }

    [Fact]
    public void Min_Sets_MinAttribute() {
        const int MIN = 3;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Min, MIN);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr min = slider.Attributes["min"]!;
        Assert.Equal(MIN.ToString(), min.Value);
    }

    [Fact]
    public void Max_Sets_MaxAttribute() {
        const int MAX = 3;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Max, MAX);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr max = slider.Attributes["max"]!;
        Assert.Equal(MAX.ToString(), max.Value);
    }

    [Fact]
    public void Step_Sets_StepAttribute() {
        const int STEP = 3;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Step, STEP);
        });

        IElement slider = sliderContainer.Find("input");
        IAttr step = slider.Attributes["step"]!;
        Assert.Equal(STEP.ToString(), step.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Editable_Renderes_InputField(bool editable) {
        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Editable, editable);
        });

        IRefreshableElementCollection<IElement> inputFields = sliderContainer.FindAll("input");
        // slider is also an inputField
        Assert.Equal(editable ? 2 : 1, inputFields.Count);
    }

    [Fact]
    public void LeftButtonContent_Is_Rendered_Inside_LeftButton() {
        const string TEST_TEXT = "Test Text";
        RenderFragment renderFragment = (RenderTreeBuilder builder) => builder.AddContent(0, TEST_TEXT);

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.LeftButtonContent, renderFragment);
        });

        IElement button = sliderContainer.Find("button");
        Assert.Equal(TEST_TEXT, button.InnerHtml);
    }

    [Fact]
    public void RightButtonContent_Is_Rendered_Inside_RightButton() {
        const string TEST_TEXT = "Test Text";
        RenderFragment renderFragment = (RenderTreeBuilder builder) => builder.AddContent(0, TEST_TEXT);

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.RightButtonContent, renderFragment);
        });

        IElement button = sliderContainer.FindAll("button")[1];
        Assert.Equal(TEST_TEXT, button.InnerHtml);
    }

    [Fact]
    public void Display_Changes_Label_Output() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Display, (int value) => TEST_TEXT);
        });

        IElement label = sliderContainer.Find(".slider-buttons label");
        Assert.Equal(TEST_TEXT, label.InnerHtml);
    }

    [Fact]
    public void ParseEdit_Reads_In_EditBox() {
        const int VALUE = 6;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Editable, true);
            builder.Add((Slider slider) => slider.ParseEdit, (string? input) => VALUE);
        });
        Slider slider = sliderContainer.Instance;

        IElement input = sliderContainer.Find("input");
        input.Change("doesn't matter because ParseEdit outputs always the same");
        Assert.Equal(VALUE, slider.Value);
    }

    #endregion


    #region events

    [Fact]
    public void ValueChanged_Is_Fired_When_Value_Is_Modified_By_User() {
        int fired = 0;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Value, 1);
            builder.Add((Slider slider) => slider.Min, 0);
            builder.Add((Slider slider) => slider.Max, 2);
            builder.Add((Slider slider) => slider.ValueChanged, (int value) => fired++);
        });

        IElement button = sliderContainer.Find("button");
        button.MouseDown();
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnChange_Is_Fired_When_Value_Is_Modified_By_User() {
        int fired = 0;

        IRenderedComponent<Slider> sliderContainer = RenderComponent((ComponentParameterCollectionBuilder<Slider> builder) => {
            builder.Add((Slider slider) => slider.Value, 1);
            builder.Add((Slider slider) => slider.Min, 0);
            builder.Add((Slider slider) => slider.Max, 2);
            builder.Add((Slider slider) => slider.OnChange, (int value) => fired++);
        });

        IElement button = sliderContainer.Find("button");
        button.MouseDown();
        Assert.Equal(1, fired);
    }

    #endregion
}