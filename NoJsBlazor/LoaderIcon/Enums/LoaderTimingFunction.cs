namespace NoJsBlazor;

/// <summary>
/// Speed/Acceleration selection
/// </summary>
public enum LoaderTimingFunction {
    /// <summary>
    /// constant speed
    /// </summary>
    Linear,
    /// <summary>
    /// slow start, then fast, ends slowly
    /// </summary>
    Ease,
    /// <summary>
    /// slow start, fast end
    /// </summary>
    Ease_in,
    /// <summary>
    /// fast start, slow end
    /// </summary>
    Ease_out,
    /// <summary>
    /// same as <see cref="Ease">Ease</see>, only even slower start and end
    /// </summary>
    Ease_in_out,
    /// <summary>
    /// fast rotation, but in the middle part very slow
    /// </summary>
    Slow_middle
}
