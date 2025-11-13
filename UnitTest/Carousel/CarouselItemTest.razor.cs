using AngleSharp.Dom;
using Bunit.Rendering;

namespace UnitTest;

public sealed partial class CarouselItemTest : BunitContext {
    [Test]
    public async ValueTask ChildContent_Is_Rendered_In_carouselItemDiv() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        IRenderedComponent<ContainerFragment> fragment = RenderCarouselWithOneItem(TEST_HTML);

        IElement itemDiv = fragment.Find(".carousel-element");
        await Assert.That(itemDiv.InnerHtml).IsEqualTo(TEST_HTML.Value);
    }
}
