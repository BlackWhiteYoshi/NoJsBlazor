namespace NoJsBlazor;

/// <summary>
/// Rotation directions
/// </summary>
public enum LoaderDirection {
    /// <summary>
    /// clockwise
    /// </summary>
    Normal,
    /// <summary>
    /// counterclockwise
    /// </summary>
    Reverse,
    /// <summary>
    /// begins with clockwise, then counterclockwise, ...
    /// </summary>
    Alternate,
    /// <summary>
    /// begins with counterclockwise, then clockwise, ...
    /// </summary>
    Alternate_reverse
}
