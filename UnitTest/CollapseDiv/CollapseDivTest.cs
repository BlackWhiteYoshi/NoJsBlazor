using AngleSharp.Dom;

namespace UnitTest;

public class CollapseDivTest : TestContext {
    #region parameter

    [Fact]
    public void Head_Is_Rendered_In_Button_Div() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Head, TEST_HTML);
        });

        IElement buttonDiv = collapseDivContainer.Find(".button-div");

        Assert.Equal(TEST_HTML, buttonDiv.InnerHtml);
    }

    [Fact]
    public void Content_Is_Rendered_Inside_outerDiv() {
        const string TEST_HTML = "<p class=\"test\">Test Text</p>";

        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.Content, TEST_HTML);
        });

        IElement outerDiv = collapseDivContainer.Find(".collapse-div");
        IElement buttonDiv = collapseDivContainer.Find(".button-div");

        Assert.True(outerDiv.Contains(buttonDiv));
    }

    [Fact]
    public void StartCollapsed_Starts_Collapsed() {
        {
            IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
                builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, true);
            });
            CollapseDiv collapseDiv = collapseDivContainer.Instance;

            Assert.True(collapseDiv.Collapsed);
        }

        {
            IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
                builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, false);
            });
            CollapseDiv collapseDiv = collapseDivContainer.Instance;

            Assert.False(collapseDiv.Collapsed);
        }
    }

    #endregion


    #region interactive

    [Fact]
    public void ButtonDiv_Clicked_Trigger_Collapsing() {
        IRenderedComponent<CollapseDiv> collapseDivContainer = RenderComponent((ComponentParameterCollectionBuilder<CollapseDiv> builder) => {
            builder.Add((CollapseDiv collapseDiv) => collapseDiv.StartCollapsed, false);
        });
        CollapseDiv collapseDiv = collapseDivContainer.Instance;

        Assert.False(collapseDiv.Collapsed);
        collapseDivContainer.Find(".button-div").MouseDown();
        Assert.True(collapseDiv.Collapsed);
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
