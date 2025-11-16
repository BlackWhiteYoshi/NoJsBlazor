using AngleSharp.Dom;
using Bunit.Rendering;

namespace UnitTest;

public sealed partial class ContextMenuItemTest : BunitContext {
    #region parameter

    [Test]
    public async ValueTask ChildContent_Is_Rendered_In_ContextItemLi() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        (IRenderedComponent<ContainerFragment>? fragment, _, _, _) = RenderContextMenuTree(TEST_HTML);

        IElement li = fragment.FindAll(".context-element")[1];
        await Assert.That(li.InnerHtml).IsEqualTo(TEST_HTML.Value);
    }

    #endregion


    #region interavtive

    [Test]
    public async ValueTask OnClick_Triggers_EventCallback() {
        int fired = 0;

        (IRenderedComponent<ContainerFragment>? fragment, _, ContextSubMenu contextSubMenu, _) = RenderContextMenuWithCallback((EventArgs e) => fired++);

        fragment.FindAll(".context-element")[1].Click();
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
