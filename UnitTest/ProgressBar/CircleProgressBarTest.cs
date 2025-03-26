using AngleSharp.Dom;

namespace UnitTest;

public sealed class CircleProgressBarTest : Bunit.TestContext {
    #region parameter

    [Test]
    [Arguments(0.5f)]
    [Arguments(1.0f)]
    public async ValueTask Progress_Fills_Circle(float progress) {
        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Progress, progress);
        });

        if (progress < 1.0)
            await Assert.That(circleProgressBarContainer.FindAll("path")).HasSingleItem();
        else {
            IRefreshableElementCollection<IElement> OuterAndInnerCircle = circleProgressBarContainer.FindAll("circle");
            await Assert.That(OuterAndInnerCircle.Count).IsEqualTo(2);
        }
    }

    [Test]
    public async ValueTask Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Text, TEST_TEXT);
        });

        IElement p = circleProgressBarContainer.Find("p");
        await Assert.That(p.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent<CircleProgressBar>();
        CircleProgressBar circleProgressBar = circleProgressBarContainer.Instance;

        circleProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        await Assert.That(circleProgressBar.Progress).IsEqualTo(TEST_VALUE);
        await Assert.That(circleProgressBar.Text).IsEqualTo(TEST_TEXT);
    }

    #endregion
}
