using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ManualTesting {
    /// <summary>
    /// A normal component that register itself on initializing at the Layout, so the Layout can call Rerender()
    /// </summary>
    public class PageComponentBase : ComponentBase {
        [CascadingParameter]
        [AllowNull]
        public Root Root { get; init; }

        protected override void OnInitialized() {
            Root.PageComponent = this;
        }

        /// <summary>
        /// <para>This will notify the <see cref="PageComponentBase"/> to Rerender.</para>
        /// <para>Technically only this components StateHasChanged() is called</para>
        /// </summary>
        public void Rerender() {
            StateHasChanged();
        }
    }
}
