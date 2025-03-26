using AngleSharp.Dom;

namespace UnitTest;

public sealed class StandardProgressBarTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Text_Sets_Description() {
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent((ComponentParameterCollectionBuilder<StandardProgressBar> builder) => {
            builder.Add((StandardProgressBar standardProgressBar) => standardProgressBar.Text, TEST_TEXT);
        });

        IElement p = standardProgressBarContainer.Find("p");
        await Assert.That(p.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Content_Sets_Progress_And_Text() {
        const float TEST_VALUE = 0.5f;
        const string TEST_TEXT = "Test Text";

        IRenderedComponent<StandardProgressBar> standardProgressBarContainer = RenderComponent<StandardProgressBar>();
        StandardProgressBar standardProgressBar = standardProgressBarContainer.Instance;

        standardProgressBar.Content = (TEST_VALUE, TEST_TEXT);
        await Assert.That(standardProgressBar.Progress).IsEqualTo(TEST_VALUE);
        await Assert.That(standardProgressBar.Text).IsEqualTo(TEST_TEXT);
    }

    #endregion
}
