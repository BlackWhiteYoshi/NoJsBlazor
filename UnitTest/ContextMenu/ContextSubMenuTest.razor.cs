using AngleSharp.Dom;

namespace UnitTest;

public partial class ContextSubMenuTest : TestContext {
    #region parameter

    [Fact]
    public void Head_Is_Rendered_In_ContextSubMenuDiv() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderContextMenuTree(TEST_HTML);

        IElement div = fragment.Find(".context-div");
        Assert.Contains(TEST_HTML.Value, div.InnerHtml);
    }

    #endregion


    #region interavtive

    [Fact]
    public void OnTouch_Triggers_Expanding() {
        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree(default);
        Assert.False(contextSubMenu.Expanded);

        IElement div = fragment.Find(".context-div");
        div.TouchStart();
        Assert.True(contextSubMenu.Expanded);
    }


    #endregion


    #region public methods

    [Fact]
    public void Toggle_Changes_Expanded_State() {
        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree(default);
        Assert.False(contextSubMenu.Expanded);

        contextSubMenu.Toggle();
        Assert.True(contextSubMenu.Expanded);
    }

    [Fact]
    public void SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((bool expanded) => fired++);

        bool stateAfterToggling = !contextSubMenu.Expanded;
        contextSubMenu.SilentExpandedSetter = !contextSubMenu.Expanded;
        Assert.Equal(stateAfterToggling, contextSubMenu.Expanded);
        Assert.Equal(0, fired);
    }

    #endregion


    #region events

    [Fact]
    public void OnToggle_Fires_When_Menu_Expanded() {
        int fired = 0;

        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((bool expanded) => fired++);
        Assert.Equal(0, fired);

        contextSubMenu.Expanded = true;
        Assert.Equal(1, fired);
    }

    #endregion
}
