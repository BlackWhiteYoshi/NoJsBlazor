namespace NoJsBlazor;

/// <summary>
/// Type of animation, how the items are swapped.
/// </summary>
public enum CarouselAnimation {
    /// <summary>
    /// Transition on opacity
    /// </summary>
    FadeOut,
    /// <summary>
    /// Transition on translate x-axis
    /// </summary>
    Slide,
    /// <summary>
    /// Transition on translate x-axis and rotate y-axis
    /// </summary>
    SlideRotate
}
