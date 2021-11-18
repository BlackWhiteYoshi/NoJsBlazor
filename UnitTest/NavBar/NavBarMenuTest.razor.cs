using AngleSharp.Dom;

namespace UnitTest;

public partial class NavBarMenuTest : TestContext {
    #region parameter

    [Fact]
    public void Head_Is_Rendered_In_NavItemDropdown() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderNavBarTree(TEST_HTML);

        IElement div = fragment.Find(".nav-item.dropdown");
        Assert.Contains(TEST_HTML.Value, div.InnerHtml);
    }

    #endregion


    #region interavtive

    [Fact]
    public void OnTouch_Triggers_Expanding() {
        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        Assert.False(navBarMenu.Expanded);

        IElement div = fragment.Find(".dropdown-arrow");
        div.TouchStart();
        Assert.True(navBarMenu.Expanded);
    }

    #endregion


    #region public Methods

    [Fact]
    public void Toggle_Changes_Expanded_State() {
        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        Assert.False(navBarMenu.Expanded);

        navBarMenu.Toggle();
        Assert.True(navBarMenu.Expanded);
    }

    #endregion


    #region events

    [Fact]
    public void OnToggle_Fires_When_Menu_Expanded() {
        int fired = 0;

        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarWithCallback((bool expanded) => fired++);
        Assert.Equal(0, fired);

        navBarMenu.Expanded = true;
        Assert.Equal(1, fired);
    }

    #endregion
}
