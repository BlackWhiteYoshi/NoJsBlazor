namespace NoJsBlazor;

/// <summary>
/// This class can save a 2D-coordinate, calculates the difference to the next coordinate, then update the coordinate and calculates the next difference and so on.
/// </summary>
public struct CoordinateTracker {
    /// <summary>
    /// x-coordinate
    /// </summary>
    public double x;
    /// <summary>
    /// y-coordinate
    /// </summary>
    public double y;


    /// <summary>
    /// <para>Sets the coordinates x/y with ClientX/ClientY and returns those.</para>
    /// <para>Normally called in onpointerdown event.</para>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public (double x, double y) SetCoordinate(PointerEventArgs e) {
        x = e.ClientX;
        y = e.ClientY;
        return (x, y);
    }

    /// <summary>
    /// <para>Sets the coordinates x/y with ClientX/ClientY and returns the difference to it's previous.</para>
    /// <para>Normally called in onpointermove event.</para>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public (double dx, double dy) MoveCoordinate(PointerEventArgs e) {
        double dx = e.ClientX - x;
        double dy = e.ClientY - y;

        x = e.ClientX;
        y = e.ClientY;

        return (dx, dy);
    }
}
