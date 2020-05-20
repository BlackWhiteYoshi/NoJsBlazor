using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NoJsBlazor {
    public partial class ContextMenu {
        /// <summary>
        /// Content of this ContextMenu.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

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
        private bool _expanded = false;

        /// <summary>
        /// <para>Fires every time after <see cref="Expanded">Expanded</see> got set.</para>
        /// <para>Value is equal <see cref="Expanded">Expanded</see>.</para>
        /// </summary>
        public event Action<bool> OnToggle;

        private double top;
        private double left;


        /// <summary>
        /// <para>Display the context menu at the given location.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(double x, double y) {
            left = x;
            top = y;
            Expanded = true;
        }

        /// <summary>
        /// <para>Display the context menu based on mouse position.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(MouseEventArgs e) {
            Show(e.ClientX, e.ClientY);
        }

        /// <summary>
        /// <para>Display the context menu based on touch position.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(TouchEventArgs e) {
            Show(e.Touches[0].ClientX, e.Touches[0].ClientY);
        }

        /// <summary>
        /// Display the context menu based on press position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(EventArgs e) {
            if (e is MouseEventArgs m)
                Show(m);
            else
                Show((TouchEventArgs)e);
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

        private readonly List<ContextSubMenu> nestedItems = new List<ContextSubMenu>();

        internal void Register(ContextSubMenu contextMenu) {
            nestedItems.Add(contextMenu);
        }

        /// <summary>
        /// Collapses all expanded submenus.
        /// </summary>
        public void Reset() {
            foreach (ContextSubMenu menu in nestedItems)
                menu.Expanded = false;
        }

        #endregion

        private object AddStyles() {
            if (Attributes == null)
                return null;

            if (Attributes.TryGetValue("style", out object styles))
                return styles;
            else
                return null;
        }
    }
}
