namespace NoJsBlazor;

/// <summary>
/// <para>Wrapper for the content that will be a carousel item.</para>
/// <para>This should be placed inside Renderfragment <b>Items</b> of a <see cref="Carousel"/> instance.</para>
/// </summary>
public sealed partial class CarouselItem : ListableComponentBase<CarouselItem> {
    /// <summary>
    /// Content of this component.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; set; }


    private bool active = false;

    protected override void OnInitialized() {
        if (((Carousel)Parent).ActiveStart == Parent.ChildCount)
            active = true;
        base.OnInitialized();
    }
}
