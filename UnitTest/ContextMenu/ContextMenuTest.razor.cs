using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class ContextMenuTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.ChildContent, FourContextSubMenuItems);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        IElement menu = contextMenuContainer.Find(".context-menu-list");
        await Assert.That(menu.Children.Length).IsEqualTo(4);
    }

    [Test]
    public async ValueTask ChildContent_Is_Rendered_In_ContextMenuDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.ChildContent, TEST_HTML);
        });

        IElement div = contextMenuContainer.Find(".context-menu-list");
        await Assert.That(div.InnerHtml).IsEqualTo(TEST_HTML);
    }

    #endregion


    #region public methodes

    [Test]
    public async ValueTask Open_Expands_ContextMenu() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>();
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        await Assert.That(contextMenu.Expanded).IsTrue();
    }

    [Test]
    public async ValueTask Close_Hides_ContextMenu() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>();
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        contextMenu.Close();
        await Assert.That(contextMenu.Expanded).IsFalse();
    }

    [Test]
    public async ValueTask Reset_Closes_Submenus() {
        (_, ContextMenu contextMenu, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree();

        contextSubMenu.Toggle();
        contextMenu.Reset();
        await Assert.That(contextSubMenu.Expanded).IsFalse();
    }

    [Test]
    public async ValueTask SilentOpen_Expands_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.SilentOpen(0.0, 0.0);
        await Assert.That(contextMenu.Expanded).IsTrue();
        await Assert.That(fired).IsEqualTo(0);
    }

    [Test]
    public async ValueTask SilentClose_Hides_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.SilentOpen(0.0, 0.0);
        contextMenu.SilentClose();
        await Assert.That(contextMenu.Expanded).IsFalse();
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnToggle_Fires_When_ContextMenu_Toggles() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
