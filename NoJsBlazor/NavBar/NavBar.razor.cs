using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace NoJsBlazor {
    public partial class NavBar {
        #region Parameters

        /// <summary>
        /// Should skip the <see cref="Brand">Brand</see> output.
        /// </summary>
        [Parameter]
        public bool DisableBrand { get; set; } = false;

        /// <summary>
        /// Content which represents your site.
        /// </summary>
        [Parameter]
        public RenderFragment Brand { get; set; }

        /// <summary>
        /// <para>List of the <see cref="NavBar"/>.</para>
        /// <para>This should be a list of <see cref="NavBarMenu"/> of <see cref="NavBarItem"/> objects.</para>
        /// </summary>
        [Parameter]
        public RenderFragment Items { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        #endregion

        /// <summary>
        /// Value for Expanding or Collapsing the navbar
        /// </summary>
        public bool Expanded {
            get => _expanded;
            set {
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

        private readonly TouchClick ToggleTC;

        public NavBar() {
            ToggleTC = new TouchClick(ToggleNavBar);
        }

        private void ToggleNavBar(EventArgs e) {
            if (Expanded)
                Reset();
            else
                Expanded = true;
        }

        internal void Rerender() {
            InvokeAsync(StateHasChanged);
        }

        #region ResetNavBarMenu

        private readonly List<NavBarMenu> NestedItems = new List<NavBarMenu>();

        internal void Register(NavBarMenu nestedNavBar) {
            NestedItems.Add(nestedNavBar);
        }


        /// <summary>
        /// Collapses all expanded menus and submenus.
        /// </summary>
        public void Reset() {
            foreach (NavBarMenu navBar in NestedItems)
                navBar.Expanded = false;

            Expanded = false;
        }

        #endregion
    }
}
