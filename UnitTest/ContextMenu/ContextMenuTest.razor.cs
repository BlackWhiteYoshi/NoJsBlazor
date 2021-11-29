using AngleSharp.Dom;

namespace UnitTest;

public partial class ContextMenuTest : TestContext {
    #region parameter

    [Fact]
    public void Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.ChildContent, FourContextSubMenuItems);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        Assert.Equal(4, contextMenu.ChildCount);
    }

    [Fact]
    public void ChildContent_Is_Rendered_In_ContextMenuDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.ChildContent, TEST_HTML);
        });

        IElement div = contextMenuContainer.Find(".context-menu");
        Assert.Equal(TEST_HTML, div.InnerHtml);
    }

    #endregion


    #region public methodes

    [Fact]
    public void Open_Expands_ContextMenu() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>();
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        Assert.True(contextMenu.Expanded);
    }

    [Fact]
    public void Close_Hides_ContextMenu() {
        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>();
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        contextMenu.Close();
        Assert.False(contextMenu.Expanded);
    }

    [Fact]
    public void Reset_Closes_Submenus() {
        (_, ContextMenu contextMenu, ContextSubMenu contextSubMenu, _) = RenderContextMenuTree();

        contextSubMenu.Toggle();
        contextMenu.Reset();
        Assert.False(contextSubMenu.Expanded);
    }

    [Fact]
    public void SilentOpen_Expands_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.SilentOpen(0.0, 0.0);
        Assert.True(contextMenu.Expanded);
        Assert.Equal(0, fired);
    }

    [Fact]
    public void SilentClose_Hides_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.SilentOpen(0.0, 0.0);
        contextMenu.SilentClose();
        Assert.False(contextMenu.Expanded);
        Assert.Equal(0, fired);
    }

    #endregion


    #region events

    [Fact]
    public void OnToggle_Fires_When_ContextMenu_Toggles() {
        int fired = 0;

        IRenderedComponent<ContextMenu> contextMenuContainer = RenderComponent<ContextMenu>((ComponentParameterCollectionBuilder<ContextMenu> builder) => {
            builder.Add((ContextMenu contextMenu) => contextMenu.OnToggle, (bool expanded) => fired++);
        });
        ContextMenu contextMenu = contextMenuContainer.Instance;

        contextMenu.Open(0.0, 0.0);
        Assert.Equal(1, fired);
    }

    #endregion
}
