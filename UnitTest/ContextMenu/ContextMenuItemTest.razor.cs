using AngleSharp.Dom;

namespace UnitTest;

public partial class ContextMenuItemTest : TestContext {
    #region parameter

    [Fact]
    public void ChildContent_Is_Rendered_In_ContextItemLi() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedFragment? fragment, _, _, _) = RenderContextMenuTree(TEST_HTML);

        IElement li = fragment.FindAll(".context-item")[1];
        Assert.Equal(TEST_HTML.Value, li.InnerHtml);
    }

    #endregion


    #region interavtive

    public enum ClickType { mouse, touch }

    [Theory]
    [InlineData(ClickType.mouse)]
    [InlineData(ClickType.touch)]
    public void OnClick_Triggers_EventCallback(ClickType type) {
        int fired = 0;

        (IRenderedFragment? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((EventArgs e) => fired++);

        IElement li = fragment.FindAll(".context-item")[1];
        if (type == ClickType.mouse)
            li.MouseDown();
        else
            li.TouchStart();
        Assert.Equal(1, fired);
    }

    #endregion
}
