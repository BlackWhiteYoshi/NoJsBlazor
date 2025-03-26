using AngleSharp.Dom;

namespace UnitTest;

public sealed class CollapseDivTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Head_Is_Rendered_In_Head() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Head, TEST_HTML);
        });

        IElement headDiv = collapseDivContainer.Find(".head");

        await Assert.That(headDiv.InnerHtml).EndsWith(TEST_HTML);
    }

    [Test]
    public async ValueTask Content_Is_Rendered_In_Content() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Content, TEST_HTML);
        });

        IElement contentDiv = collapseDivContainer.Find(".content-inner");

        await Assert.That(contentDiv.InnerHtml).IsEqualTo(TEST_HTML);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask StartCollapsed_Starts_Collapsed(bool collapsed) {
        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartExpanded, collapsed);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        await Assert.That(collapseDiv.Expanded).IsEqualTo(collapsed);
    }

    #endregion


    #region interactive

    [Test]
    public async ValueTask Head_Clicked_Trigger_Collapsing() {
        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartExpanded, false);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        await Assert.That(collapseDiv.Expanded).IsFalse();
        collapseDivContainer.Find(".collapse-toggle").Change(true);
        await Assert.That(collapseDiv.Expanded).IsTrue();
        collapseDivContainer.Find(".collapse-toggle").Change(false);
        await Assert.That(collapseDiv.Expanded).IsFalse();
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask SilentCollapsedSetter_Sets_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartExpanded, false);
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.OnExpandedChanged, (bool collapsed) => fired++);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        collapseDiv.SilentExpandedSetter = true;
        await Assert.That(collapseDiv.Expanded).IsTrue();
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnCollapseChanged_Fires_When_Collapse_State_Changes() {
        int fired = 0;

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.OnExpandedChanged, (bool collapsed) => fired++);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        collapseDiv.Expanded = !collapseDiv.Expanded;

        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
