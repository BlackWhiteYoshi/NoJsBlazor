namespace NoJsBlazor;

/// <summary>
/// <para>Wrapper for the content that will be a carousel item.</para>
/// <para>This should be placed inside Renderfragment <b>Items</b> of a <see cref="Carousel"/> instance.</para>
/// </summary>
public class CarouselItem : ListableComponentBase<CarouselItem> {
    /// <summary>
    /// Content of this component.
    /// </summary>
    [Parameter]
    [AllowNull]
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


    private bool active = false;

    protected override void OnInitialized() {
        if (((Carousel)Parent).ActiveStart == Parent.ChildCount)
            active = true;
        base.OnInitialized();
    }

    /// <summary>
    /// How this component renders, what is normally written in razor-syntax
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        /***
            * <div class="carousel-item" @if (active) { style="z-index: 20;" }>
            *     @ChildContent
            * </div>
            ***/
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "carousel-item");
        if (active)
            builder.AddAttribute(2, "style", "z-index: 20;");
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}
