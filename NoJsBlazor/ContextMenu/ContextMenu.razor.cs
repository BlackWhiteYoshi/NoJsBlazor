using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NoJsBlazor {
    /// <summary>
    /// <para>A menu that can be toggled and shown at mouse position.</para>
    /// <para>Triggering the menu is not included.</para>
    /// </summary>
    public partial class ContextMenu {
        /// <summary>
        /// Content of this ContextMenu.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? Attributes { get; set; }

        private bool _expanded = false;
        /// <summary>
        /// Value for expanding or collapsing this submenu.
        /// </summary>
        /// <param name="value">expanded</param>
        public bool Expanded {
            get => _expanded;
            private set {
                _expanded = value;
                OnToggle?.Invoke(value);
                InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// <para>Fires every time after <see cref="Expanded">Expanded</see> got set.</para>
        /// <para>Value is equal <see cref="Expanded">Expanded</see>.</para>
        /// </summary>
        public event Action<bool>? OnToggle;

        private double top;
        private double left;


        /// <summary>
        /// <para>Display the context menu at the given location.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open(double x, double y) {
            left = x;
            top = y;
            Expanded = true;
        }

        /// <summary>
        /// <para>Display the context menu based on mouse position.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open(MouseEventArgs e) => Open(e.ClientX, e.ClientY);

        /// <summary>
        /// <para>Display the context menu based on touch position.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open(TouchEventArgs e) => Open(e.Touches[0].ClientX, e.Touches[0].ClientY);

        /// <summary>
        /// Display the context menu based on press position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open(EventArgs eventArgs) {
            switch (eventArgs) {
                case MouseEventArgs mouseEventArgs:
                    Open(mouseEventArgs);
                    break;
                case TouchEventArgs touchEventArgs:
                    Open(touchEventArgs);
                    break;
            }
        }

        /// <summary>
        /// Collapses all expanded submenus and closes the context menu.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Close() {
            Reset();
            Expanded = false;
        }

        #region ResetContextMenu

        private readonly List<ContextSubMenu> nestedItems = new();

        internal void Register(ContextSubMenu contextMenu) => nestedItems.Add(contextMenu);

        /// <summary>
        /// Collapses all expanded submenus.
        /// </summary>
        public void Reset() {
            foreach (ContextSubMenu menu in nestedItems)
                menu.Expanded = false;
        }

        #endregion

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
