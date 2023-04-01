using AngleSharp.Dom;

namespace UnitTest;

public sealed class SpeedometerProgressBarTest : TestContext {
    #region parameter

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
