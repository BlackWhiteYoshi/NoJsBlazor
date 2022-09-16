using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class NavBarMenuTest : TestContext {
    #region parameter

    [Fact]
    public void Head_Is_Rendered_In_NavItemDropdown() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderNavBarTree(TEST_HTML);

        IElement div = fragment.Find(".nav-element > .nav-div");
        Assert.Contains(TEST_HTML.Value, div.InnerHtml);
    }

    #endregion


    #region interavtive

    [Fact]
    public void OnTouch_Triggers_Expanding() {
        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        Assert.False(navBarMenu.Expanded);

        IElement div = fragment.Find(".nav-dropdown-arrow");
        div.Click();
        Assert.True(navBarMenu.Expanded);
    }

    #endregion


    #region public methods

    [Fact]
    public void Toggle_Changes_Expanded_State() {
        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        Assert.False(navBarMenu.Expanded);

        navBarMenu.Toggle();
        Assert.True(navBarMenu.Expanded);
    }

    [Fact]
    public void SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarWithCallback((bool expanded) => fired++);

        navBarMenu.SilentExpandedSetter = true;
        Assert.True(navBarMenu.Expanded);
        Assert.Equal(0, fired);
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
