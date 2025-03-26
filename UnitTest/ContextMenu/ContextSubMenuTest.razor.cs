using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class ContextSubMenuTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Head_Is_Rendered_In_ContextSubMenuDiv() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderContextMenuTree(TEST_HTML);

        IElement div = fragment.Find(".context-submenu-toggle");
        await Assert.That(div.InnerHtml).StartsWith(TEST_HTML.Value);
    }

    #endregion


    #region interavtive

    [Test]
    public async ValueTask OnClick_Triggers_Expanding() {
        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree(default);
        await Assert.That(contextSubMenu.Expanded).IsFalse();

        IElement checkbox = fragment.Find(".context-submenu-checkbox");
        checkbox.Change(true);
        await Assert.That(contextSubMenu.Expanded).IsTrue();
        checkbox.Change(false);
        await Assert.That(contextSubMenu.Expanded).IsFalse();
    }


    #endregion


    #region public methods

    [Test]
    public async ValueTask Toggle_Changes_Expanded_State() {
        (_, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree(default);
        await Assert.That(contextSubMenu.Expanded).IsFalse();

        contextSubMenu.Toggle();
        await Assert.That(contextSubMenu.Expanded).IsTrue();
    }

    [Test]
    public async ValueTask SilentExpandedSetter_Sets_Without_Notifying() {
        int fired = 0;

        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((bool expanded) => fired++);

        bool stateAfterToggling = !contextSubMenu.Expanded;
        contextSubMenu.SilentExpandedSetter = !contextSubMenu.Expanded;
        await Assert.That(contextSubMenu.Expanded).IsEqualTo(stateAfterToggling);
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnToggle_Fires_When_Menu_Expanded() {
        int fired = 0;

        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((bool expanded) => fired++);
        await Assert.That(fired).IsEqualTo(0);

        contextSubMenu.Expanded = true;
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
