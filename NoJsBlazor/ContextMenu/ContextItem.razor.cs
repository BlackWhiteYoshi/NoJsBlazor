using Microsoft.AspNetCore.Components;
using System;

namespace NoJsBlazor {
    public partial class ContextItem {
        /// <summary>
        /// Html that will be displayed.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// <para>Invokes every time when this list item get clicked or touched.</para>
        /// <para><see cref="EventArgs"/> is either <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>, depends if it was invoked by clicking or touching.</para>
        /// </summary>
        [Parameter]
        public Action<EventArgs> OnPressed { get; set; }


        private TouchClick touchClick;

        public ContextItem() {
            touchClick = new TouchClick(OnDown);
        }

        private void OnDown(EventArgs e) {
            OnPressed?.Invoke(e);
        }
    }
}
