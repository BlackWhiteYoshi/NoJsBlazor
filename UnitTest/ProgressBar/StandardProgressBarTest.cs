using AngleSharp.Dom;

namespace UnitTest;

public sealed class StandardProgressBarTest : TestContext {
    #region parameter

    [Fact]
    public void Width_Sets_Width_Attribute() {
        const int WIDTH = 100;

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Width, WIDTH);
        });

        IElement div = standardProgressBarContainer.Find(".standard-progress-bar div div");
        IAttr style = div.Attributes["style"]!;
        Assert.Contains($"width: {WIDTH}px", style.Value);
    }

    [Fact]
    public void Height_Sets_Height_Attribute() {
        const int HEIGHT = 100;

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Height, HEIGHT);
        });

        IElement div = standardProgressBarContainer.Find(".standard-progress-bar div div");
        IAttr style = div.Attributes["style"]!;
        Assert.Contains($"height: {HEIGHT}px", style.Value);
    }

    [Fact]
    public void Color_Sets_Color_Attribute() {
        const string COLOR = "#FFF";

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Color, COLOR);
        });

        IElement div = standardProgressBarContainer.Find(".standard-progress-bar div div");
        IAttr style = div.Attributes["style"]!;
        Assert.Contains(COLOR, style.Value);
    }

    [Fact]
    public void Progress_Sets_BorderWidth() {
        const int WIDTH = 200;
        const float PROGRESS = 0.3f;

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Width, WIDTH);
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Progress, PROGRESS);
        });

        float width = WIDTH * PROGRESS;
        IElement div = standardProgressBarContainer.Find(".standard-progress-bar div div");
        IAttr style = div.Attributes["style"]!;
        Assert.Contains($"{width}px", style.Value);
    }

    [Fact]
    public void Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Text, TEST_TEXT);
        });

        IElement p = standardProgressBarContainer.Find("p");
        Assert.Equal(TEST_TEXT, p.InnerHtml);
    }

    #endregion


    #region public methods

    [Fact]
    public void Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent<StandardProgressBar>();
        StandardProgressBar standardProgressBar = standardProgressBarContainer.Instance;

        standardProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        Assert.Equal(TEST_VALUE, standardProgressBar.Progress);
        Assert.Equal(TEST_TEXT, standardProgressBar.Text);
    }

    #endregion
}
