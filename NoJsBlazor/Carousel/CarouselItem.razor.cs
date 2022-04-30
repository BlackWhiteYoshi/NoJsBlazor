namespace NoJsBlazor;

/// <summary>
/// <para>Wrapper for the content that will be a carousel item.</para>
/// <para>This should be placed inside Renderfragment <b>Items</b> of a <see cref="Carousel"/> instance.</para>
/// </summary>
public partial class CarouselItem : ListableComponentBase<CarouselItem> {
    /// <summary>
    /// Content of this component.
    /// </summary>
    [Parameter, AllowNull]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Creates a <see cref="CarouselItem"/> with no content.
    /// </summary>
    public CarouselItem() : base() { }

    /// <summary>
    /// Creates a <see cref="CarouselItem"/> with the given content, which can be inserted in an existing carousel.
    /// </summary>
    /// <param name="child">content of this object</param>
    public CarouselItem(RenderFragment child) : base() => ChildContent = child;


    private string cssClass = "carousel-element";

    protected override void OnInitialized() {
        if (((Carousel)Parent).ActiveStart == Parent.ChildCount)
            cssClass = "carousel-element active";
        base.OnInitialized();
    }
}
