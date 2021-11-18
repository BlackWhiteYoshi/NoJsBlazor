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
    /// Transition on transform: x-axis
    /// </summary>
    Slide,
    /// <summary>
    /// Transition on transform: x-axis and y-axis
    /// </summary>
    SlideRotate
}