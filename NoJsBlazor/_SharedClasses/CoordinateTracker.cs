using Microsoft.AspNetCore.Components.Web;
using System;

namespace NoJsBlazor {
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
        /// <para>Normally called in onmousedown event.</para>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public (double x, double y) SetCoordinate(MouseEventArgs e) {
            x = e.ClientX;
            y = e.ClientY;
            return (x, y);
        }

        /// <summary>
        /// <para>Sets the coordinates x/y with ClientX/ClientY and returns those.</para>
        /// <para>Normally called in ontouchstart event.</para>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public (double x, double y) SetCoordinate(TouchEventArgs e) {
            x = e.TargetTouches[0].ClientX;
            y = e.TargetTouches[0].ClientY;
            return (x, y);
        }

        /// <summary>
        /// Sets the coordinates x/y with ClientX/ClientY and returns those.
        /// </summary>
        /// <param name="e">Has to be a <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>, otherwise an Exception is thrown.</param>
        /// <returns></returns>
        public (double x, double y) SetCoordinate(EventArgs e) {
            if (e is MouseEventArgs m)
                return SetCoordinate(m);
            else
                return SetCoordinate((TouchEventArgs)e);
        }

        /// <summary>
        /// <para>Sets the coordinates x/y with ClientX/ClientY and returns the difference to it's previous.</para>
        /// <para>Normally called in onmousemove event.</para>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public (double dx, double dy) MoveCoordinate(MouseEventArgs e) {
            double dx = e.ClientX - x;
            double dy = e.ClientY - y;

            x = e.ClientX;
            y = e.ClientY;

            return (dx, dy);
        }

        /// <summary>
        /// <para>Sets the coordinates x/y with ClientX/ClientY and returns the difference to it's previous.</para>
        /// <para>Normally called in ontouchmove event.</para>
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public (double dx, double dy) MoveCoordinate(TouchEventArgs e) {
            double dx = e.TargetTouches[0].ClientX - x;
            double dy = e.TargetTouches[0].ClientY - y;

            x = e.TargetTouches[0].ClientX;
            y = e.TargetTouches[0].ClientY;

            return (dx, dy);
        }

        /// <summary>
        /// <para>Sets the coordinates x/y with ClientX/ClientY and returns the difference to it's previous.</para>
        /// </summary>
        /// <param name="e">Has to be a <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>, otherwise an Exception is thrown.</param>
        /// <returns></returns>
        public (double dx, double dy) MoveCoordinate(EventArgs e) {
            if (e is MouseEventArgs m)
                return MoveCoordinate(m);
            else
                return MoveCoordinate((TouchEventArgs)e);
        }
    }
}
