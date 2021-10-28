using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace NoJsBlazor {
    /// <summary>
    /// <para>Wrapper for the content that will be a context item.</para>
    /// <para>This should be placed inside of a <see cref="ContextMenu"/>/<see cref="ContextSubMenu"/> instance.</para>
    /// </summary>
    public partial class ContextMenuItem {
        /// <summary>
        /// Html that will be displayed.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// <para>Invokes every time when this list item get clicked or touched.</para>
        /// <para><see cref="EventArgs"/> is either <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>, depends if it was invoked by clicking or touching.</para>
        /// </summary>
        [Parameter]
        public Action<EventArgs>? OnPressed { get; set; }


        private readonly TouchClick touchClick;

        public ContextMenuItem() => touchClick = new TouchClick(OnDown);

        private void OnDown(EventArgs e) => OnPressed?.Invoke(e);
    }
}
