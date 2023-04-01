using AngleSharp.Dom;

namespace UnitTest;

public sealed class StandardProgressBarTest : TestContext {
    #region parameter

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
