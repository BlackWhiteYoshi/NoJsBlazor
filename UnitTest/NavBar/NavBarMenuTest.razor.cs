using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class NavBarMenuTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Head_Is_Rendered_In_NavItemDropdown() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderNavBarTree(TEST_HTML);

        IElement div = fragment.Find(".nav-element > .nav-div");
        await Assert.That(div.InnerHtml).Contains(TEST_HTML.Value);
    }

    #endregion


    #region interavtive

    [Test]
    public async ValueTask OnTouch_Triggers_Expanding() {
        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        await Assert.That(navBarMenu.Expanded).IsFalse();

        IElement div = fragment.Find(".nav-dropdown-checkbox");
        div.Change(true);
        await Assert.That(navBarMenu.Expanded).IsTrue();
        div.Change(false);
        await Assert.That(navBarMenu.Expanded).IsFalse();
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Toggle_Changes_Expanded_State() {
        (_, _, NavBarMenu navBarMenu, _) = RenderNavBarTree(default);
        await Assert.That(navBarMenu.Expanded).IsFalse();

        navBarMenu.Toggle();
        await Assert.That(navBarMenu.Expanded).IsTrue();
    }

    [Test]
    public async ValueTask SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarWithCallback((bool expanded) => fired++);

        navBarMenu.SilentExpandedSetter = true;
        await Assert.That(navBarMenu.Expanded).IsTrue();
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnToggle_Fires_When_Menu_Expanded() {
        int fired = 0;

        (IRenderedFragment? fragment, _, NavBarMenu navBarMenu, _) = RenderNavBarWithCallback((bool expanded) => fired++);
        await Assert.That(fired).IsEqualTo(0);

        navBarMenu.Expanded = true;
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
