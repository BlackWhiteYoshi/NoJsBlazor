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


    private bool _active = false;
    internal bool Active {
        get => _active;
        set {
            _active = value;
            InvokeAsync(StateHasChanged);
        }
    }


    #region Slide and SlideRotate

    private int position = 1;
    private int rotateAmount = 0;
    private string slideClass = string.Empty; // "slide" or string.Empty;
    private bool rotateAfterRender = false;

    internal void Rotate(int start, int amount) {
        slideClass = string.Empty;
        position = start;
        rotateAmount = amount;

        rotateAfterRender = true;
        StateHasChanged();
    }


    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (rotateAfterRender) {
            rotateAfterRender = false;
            await Task.Delay(30);

            slideClass = "slide";
            position -= rotateAmount;
            Active = (position == 0);

            StateHasChanged();
        }
    }

    #endregion
}
