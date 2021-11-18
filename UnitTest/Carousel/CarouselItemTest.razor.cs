using AngleSharp.Dom;

namespace UnitTest;

public partial class CarouselItemTest : TestContext {
    [Fact]
    public void ChildContent_Is_Rendered_In_carouselItemDiv() {
        MarkupString TEST_HTML = new("<p>Test Text</p>");

        IRenderedFragment fragment = RenderCarouselWithOneItem(TEST_HTML);

        IElement itemDiv = fragment.Find(".carousel-item");
        Assert.Equal(TEST_HTML.Value, itemDiv.InnerHtml);
    }
}
