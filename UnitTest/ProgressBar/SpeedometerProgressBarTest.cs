using AngleSharp.Dom;

namespace UnitTest;

public sealed class SpeedometerProgressBarTest : TestContext {
    #region parameter

    [Fact]
    public void Diameter_Sets_Width_And_Height_Attribute() {
        const int DIAMETER = 100;

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Diameter, DIAMETER);
        });

        IElement svg = speedometerProgressBarContainer.Find("svg");
        IAttr style = svg.Attributes["style"]!;
        Assert.Equal($"width: {DIAMETER}px; height: {DIAMETER}px;", style.Value);
    }

    [Fact]
    public void Length_Influences_Coordinates() {
        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Length, 0.0f);
        });

        IElement line = speedometerProgressBarContainer.Find(".meter");

        AssertEqualZero(line.Attributes["x1"]!);
        AssertEqualZero(line.Attributes["y1"]!);
        AssertEqualZero(line.Attributes["x2"]!);
        AssertEqualZero(line.Attributes["y2"]!);


        static void AssertEqualZero(IAttr attr) {
            if (attr.Value.Length == 1)
                Assert.Equal("0", attr.Value);
            else
                Assert.Equal("-0", attr.Value);
        }
    }

    [Fact]
    public void Color_Sets_Color_Attribute() {
        const string COLOR = "#FFF";

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Color, COLOR);
        });

        IElement line = speedometerProgressBarContainer.Find(".meter");
        IAttr stroke = line.Attributes["stroke"]!;
        Assert.Equal(COLOR, stroke.Value);
    }

    [Theory]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    public void Progress_Rotates_Meter(float progress) {
        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Progress, progress);
        });

        IElement line = speedometerProgressBarContainer.Find(".meter");
        IAttr style = line.Attributes["style"]!;
        Assert.Equal($"rotate: {(float)(3.0 / 2.0 * Math.PI) * progress + (float)(1.0 / 4.0 * Math.PI)}rad", style.Value);
    }

    [Fact]
    public void Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Text, TEST_TEXT);
        });

        IElement p = speedometerProgressBarContainer.Find("p");
        Assert.Equal(TEST_TEXT, p.InnerHtml);
    }

    #endregion


    #region public methods

    [Fact]
    public void Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent<SpeedometerProgressBar>();
        SpeedometerProgressBar speedometerProgressBar = speedometerProgressBarContainer.Instance;

        speedometerProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        Assert.Equal(TEST_VALUE, speedometerProgressBar.Progress);
        Assert.Equal(TEST_TEXT, speedometerProgressBar.Text);
    }

    #endregion
}
