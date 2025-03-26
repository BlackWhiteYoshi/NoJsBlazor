using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class NavBarTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.Items, FourNavBarMenus);
        });
        NavBar navBar = navBarContainer.Instance;

        await Assert.That(navBar.ChildCount).IsEqualTo(4);
    }

    [Test]
    public async ValueTask Brand_Is_Not_Rendered_If_It_Is_Null() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent<NavBar>();

        await Assert.That(navBarContainer.FindAll(".navbar-brand")).IsEmpty();
    }

    [Test]
    public async ValueTask Brand_Is_Rendered_In_Both_NavbarBrandDivs() {
        const string TEST_TEXT = "test text";

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.Brand, TEST_TEXT);
        });

        IElement brand = navBarContainer.Find(".nav-brand");
        await Assert.That(brand.InnerHtml).IsEqualTo(TEST_TEXT);
    }

    #endregion


    #region interactive

    [Test]
    public async ValueTask Click_On_PhoneButton_Triggers_Expanding() {
        IRenderedComponent<NavBar> navBarContainer = RenderComponent<NavBar>();
        NavBar navBar = navBarContainer.Instance;

        IElement toggle = navBarContainer.Find(".nav-checkbox");

        toggle.Change(true);
        await Assert.That(navBar.Expanded).IsTrue();

        toggle.Change(false);
        await Assert.That(navBar.Expanded).IsFalse();
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Reset_Closes_Submenus() {
        (_, NavBar navBar, NavBarMenu navBarMenu, _) = RenderNavBarTree();

        await Assert.That(navBarMenu.Expanded).IsFalse();
        navBarMenu.Toggle();
        await Assert.That(navBarMenu.Expanded).IsTrue();
        navBar.Reset();
        await Assert.That(navBarMenu.Expanded).IsFalse();
    }

    [Test]
    public async ValueTask SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.OnToggle, (bool expanded) => fired++);
        });
        NavBar navBar = navBarContainer.Instance;

        navBar.SilentExpandedSetter = true;
        await Assert.That(navBar.Expanded).IsTrue();
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnToggle_Fires_When_Navbar_Expands() {
        int fired = 0;

        IRenderedComponent<NavBar> navBarContainer = RenderComponent((ComponentParameterCollectionBuilder<NavBar> builder) => {
            builder.Add((NavBar navBar) => navBar.OnToggle, (bool expanded) => fired++);
        });
        NavBar navBar = navBarContainer.Instance;

        navBar.Expanded = true;
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
