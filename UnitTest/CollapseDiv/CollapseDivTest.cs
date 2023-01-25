using AngleSharp.Dom;

namespace UnitTest;

public sealed class CollapseDivTest : TestContext {
    #region parameter

    [Fact]
    public void Head_Is_Rendered_In_Head() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Head, TEST_HTML);
        });

        IElement headDiv = collapseDivContainer.Find(".head");

        Assert.Equal(TEST_HTML, headDiv.InnerHtml);
    }

    [Fact]
    public void Content_Is_Rendered_In_Content() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Content, TEST_HTML);
        });

        IElement contentDiv = collapseDivContainer.Find(".content");

        Assert.Equal(TEST_HTML, contentDiv.InnerHtml);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void StartCollapsed_Starts_Collapsed(bool collapsed) {
        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, collapsed);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        Assert.Equal(collapsed, collapseDiv.Collapsed);
    }

    #endregion


    #region interactive

    [Fact]
    public void Head_Clicked_Trigger_Collapsing() {
        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, false);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        Assert.False(collapseDiv.Collapsed);
        collapseDivContainer.Find(".head").Click();
        Assert.True(collapseDiv.Collapsed);
    }

    #endregion


    #region public methods

    [Fact]
    public void SilentCollapsedSetter_Sets_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, false);
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.OnCollapseChanged, (bool collapsed) => fired++);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        collapseDiv.SilentCollapsedSetter = true;
        Assert.True(collapseDiv.Collapsed);
        Assert.Equal(0, fired);
    }

    #endregion


    #region events

    [Fact]
    public void OnCollapseChanged_Fires_When_Collapse_State_Changes() {
        int fired = 0;

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.OnCollapseChanged, (bool collapsed) => fired++);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        collapseDiv.Collapsed = !collapseDiv.Collapsed;

        Assert.Equal(1, fired);
    }

    #endregion
}
