using AngleSharp.Dom;

namespace UnitTest;

public sealed class CircleProgressBarTest : TestContext {
    #region parameter

    [Theory]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    public void Progress_Fills_Circle(float progress) {
        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Progress, progress);
        });

        if (progress < 1.0)
            Assert.Single(circleProgressBarContainer.FindAll("path"));
        else {
            IRefreshableElementCollection<IElement> OuterAndInnerCircle = circleProgressBarContainer.FindAll("circle");
            Assert.Equal(2, OuterAndInnerCircle.Count);
        }
    }

    [Fact]
    public void Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Text, TEST_TEXT);
        });

        IElement p = circleProgressBarContainer.Find("p");
        Assert.Equal(TEST_TEXT, p.InnerHtml);
    }

    #endregion


    #region public methods

    [Fact]
    public void Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent<CircleProgressBar>();
        CircleProgressBar circleProgressBar = circleProgressBarContainer.Instance;

        circleProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        Assert.Equal(TEST_VALUE, circleProgressBar.Progress);
        Assert.Equal(TEST_TEXT, circleProgressBar.Text);
    }

    #endregion
}
