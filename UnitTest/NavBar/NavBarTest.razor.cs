using AngleSharp.Dom;

namespace UnitTest;

public partial class NavBarTest : TestContext {
    #region parameter

    [Fact]
    public void Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.Items, FourNavBarMenus);
        });
        NavBar navBar = navBarContainer.Instance;

        Assert.Equal(4, navBar.ChildCount);
    }

    [Fact]
    public void Brand_Is_Not_Rendered_If_It_Is_Null() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent<NavBar>();

        Assert.Empty(navBarContainer.FindAll(".navbar-brand"));
    }

    [Fact]
    public void Brand_Is_Rendered_In_Both_NavbarBrandDivs() {
        const string TEST_TEXT = "test text";

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.Brand, TEST_TEXT);
        });

        IRefreshableElementCollection<IElement> divCollection = navBarContainer.FindAll(".navbar-brand");
        Assert.Equal(2, divCollection.Count);
        Assert.Equal(TEST_TEXT, divCollection[0].InnerHtml);
        Assert.Equal(TEST_TEXT, divCollection[1].InnerHtml);
    }

    #endregion


    #region interactive

    [Fact]
    public void Click_On_PhoneButton_Triggers_Expanding() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent<NavBar>();
        NavBar navBar = navBarContainer.Instance;

        IElement toggle = navBarContainer.Find(".navbar-toggler-icon");

        toggle.MouseDown();
        Assert.True(navBar.Expanded);

        toggle.MouseDown();
        Assert.False(navBar.Expanded);
    }

    #endregion


    #region public methods

    [Fact]
    public void Reset_Closes_Submenus() {
        (_, NavBar navBar, NavBarMenu navBarMenu, _) = RenderNavBarTree();

        Assert.False(navBarMenu.Expanded);
        navBarMenu.Toggle();
        Assert.True(navBarMenu.Expanded);
        navBar.Reset();
        Assert.False(navBarMenu.Expanded);
    }

    [Fact]
    public void SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.OnToggle, (bool expanded) => fired++);
        });
        NavBar navBar = navBarContainer.Instance;

        navBar.SilentExpandedSetter = true;
        Assert.True(navBar.Expanded);
        Assert.Equal(0, fired);
    }

    #endregion


    #region events

    [Fact]
    public void OnToggle_Fires_When_Navbar_Expands() {
        int fired = 0;

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.OnToggle, (bool expanded) => fired++);
        });
        NavBar navBar = navBarContainer.Instance;

        navBar.Expanded = true;
        Assert.Equal(1, fired);
    }

    #endregion
}
