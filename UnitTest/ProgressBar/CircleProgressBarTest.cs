using AngleSharp.Dom;

namespace UnitTest;

public class CircleProgressBarTest : TestContext {
    #region parameter

    [Fact]
    public void Diameter_Sets_Width_And_Height_Attribute() {
        const int DIAMETER = 100;

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Diameter, DIAMETER);
        });

        IElement svg = circleProgressBarContainer.Find("svg");
        IAttr style = svg.Attributes["style"]!;
        Assert.Equal($"width: {DIAMETER}px; height: {DIAMETER}px;", style.Value);
    }

    [Fact]
    public void Margin_Sets_InnerRadius() {
        const int MARGIN = 5;

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Margin, MARGIN);
        });
        CircleProgressBar circleProgressBar = circleProgressBarContainer.Instance;

        circleProgressBar.Progress = 1.0f;

        IElement circle = circleProgressBarContainer.FindAll("circle")[1];
        IAttr r = circle.Attributes["r"]!;
        Assert.Equal((100 - MARGIN).ToString(), r.Value);
    }

    [Fact]
    public void Color_Sets_Color_Attribute() {
        const string COLOR = "#FFF";

        IRenderedComponent<CircleProgressBar> circleProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<CircleProgressBar> builder) => {
            builder.Add((CircleProgressBar circleProgressBar) => circleProgressBar.Color, COLOR);
        });

        IElement div = circleProgressBarContainer.Find("path");
        IAttr fill = div.Attributes["fill"]!;
        Assert.Equal(COLOR, fill.Value);
    }

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
