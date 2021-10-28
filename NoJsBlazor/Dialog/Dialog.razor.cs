using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NoJsBlazor {
    /// <summary>
    /// A little message window that it is first hidden and can be displayed on demand.
    /// </summary>
    public partial class Dialog {
        /// <summary>
        /// Html that will be displayed inside the head bar.
        /// </summary>
        [Parameter]
        public RenderFragment? Title { get; set; }

        /// <summary>
        /// Html that will be displayed inside the window.
        /// </summary>
        [Parameter]
        public RenderFragment? Content { get; set; }

        /// <summary>
        /// Indicates whether to skip the <see cref="Title">Title</see> section
        /// </summary>
        [Parameter]
        public bool ShowTitle { get; set; } = true;

        /// <summary>
        /// The Window can be grabed and draged around the screen
        /// </summary>
        [Parameter]
        public bool Moveable { get; set; } = true;

        /// <summary>
        /// <para>If the background should be blurred/unavailable.</para>
        /// <para>Technically the background will be overlayed with a div with high opacity.</para>
        /// </summary>
        [Parameter]
        public bool ModalScreen { get; set; } = true;

        /// <summary>
        /// If on the <see cref="ModalScreen">ModalBackground</see> a click/touch occurs, whether the window should close or not close.
        /// </summary>
        [Parameter]
        public bool CloseOnModalBackground { get; set; } = true;

        /// <summary>
        /// <para>Invokes every time when the Dialog get closed.</para>
        /// <para>Technically invokes every time when <see cref="Active">Active</see> is set to false</para>
        /// </summary>
        [Parameter]
        public Action? OnClose { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? Attributes { get; set; }

        /// <summary>
        /// Current offset of x-coordinate, 0 means middle of the screen.
        /// </summary>
        public double XMovement { get; set; } = 0.0;
        /// <summary>
        /// Current offset of y-coordinate, 0 means middle of the screen.
        /// </summary>
        public double YMovement { get; set; } = 0.0;

        /// <summary>
        /// <para>true -> window is shown;</para>
        /// <para>false -> window is closed</para>
        /// </summary>
        public bool Active {
            get => _active;
            set {
                _active = value;
                if (!value)
                    OnClose?.Invoke();
                InvokeAsync(StateHasChanged);
            }
        }
        private bool _active;

        /// <summary>
        /// Use this to fill the @onmove and @onup event of the object behind this window if you don't enable <see cref="ModalScreen">ModalScreen</see>
        /// </summary>
        public readonly TouchClick headBarTC;
        private readonly TouchClick modalBackgroundTC;
        private CoordinateTracker tracker;

        public Dialog() {
            modalBackgroundTC = new TouchClick(ModalBackgroundClick);
            headBarTC = new TouchClick(HeadBarDown, HeadBarMove);
        }

        private void ModalBackgroundClick(EventArgs e) {
            if (CloseOnModalBackground)
                Active = false;
        }


        private void HeadBarDown(EventArgs e) => tracker.SetCoordinate(e);

        private void HeadBarMove(EventArgs e) {
            if (!Moveable)
                return;

            (double dx, double dy) = tracker.MoveCoordinate(e);

            XMovement += dx;
            YMovement += dy;
            StateHasChanged();
        }


        /// <summary>
        /// <para>Display the window.</para>
        /// <para>The same as setting XMovement/YMovement = 0 and Active = true</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open() {
            XMovement = 0.0;
            YMovement = 0.0;
            Active = true;
        }

        /// <summary>
        /// <para>Display the window at the last moved position.</para>
        /// <para>The same as Active = true</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OpenWithLastPosition() {
            Active = true;
        }

        /// <summary>
        /// <para>Closes the window.</para>
        /// <para>The same as Active = false</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Close() => Active = false;


        private object? AddStyles() {
            if (Attributes == null)
                return null;

            if (Attributes.TryGetValue("style", out object? styles))
                return styles;
            else
                return null;
        }
    }
}
