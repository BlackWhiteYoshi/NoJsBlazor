using AngleSharp.Dom;

namespace UnitTest;

public sealed class SpeedometerProgressBarTest : Bunit.TestContext {
    #region parameter

    [Test]
    [Arguments(0.5f)]
    [Arguments(1.0f)]
    public async ValueTask Progress_Rotates_Meter(float progress) {
        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Progress, progress);
        });

        IElement line = speedometerProgressBarContainer.Find(".meter");
        IAttr style = line.Attributes["style"]!;
        await Assert.That(style.Value).IsEqualTo($"rotate: {(float)(3.0 / 2.0 * Math.PI) * progress + (float)(1.0 / 4.0 * Math.PI)}rad");
    }

    [Test]
    public async ValueTask Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<SpeedometerProgressBar> builder) => {
            builder.Add((SpeedometerProgressBar speedometerProgressBar) => speedometerProgressBar.Text, TEST_TEXT);
        });

        IElement p = speedometerProgressBarContainer.Find("p");
        await Assert.That(p.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<SpeedometerProgressBar> speedometerProgressBarContainer = RenderComponent<SpeedometerProgressBar>();
        SpeedometerProgressBar speedometerProgressBar = speedometerProgressBarContainer.Instance;

        speedometerProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        await Assert.That(speedometerProgressBar.Progress).IsEqualTo(TEST_VALUE);
        await Assert.That(speedometerProgressBar.Text).IsEqualTo(TEST_TEXT);
    }

    #endregion
}
