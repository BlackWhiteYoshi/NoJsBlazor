using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class CarouselItemTest : Bunit.TestContext {
    [Test]
    public async ValueTask ChildContent_Is_Rendered_In_carouselItemDiv() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        IRenderedFragment fragment = RenderCarouselWithOneItem(TEST_HTML);

        IElement itemDiv = fragment.Find(".carousel-element");
        await Assert.That(itemDiv.InnerHtml).IsEqualTo(TEST_HTML.Value);
    }
}
