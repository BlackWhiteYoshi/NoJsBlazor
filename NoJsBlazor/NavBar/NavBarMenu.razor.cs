using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NoJsBlazor {
    /// <summary>
    /// <para>A single menu that can hold other <see cref="NavBarMenu"/> or/and <see cref="NavBarItem"/>/<see cref="NavBarLink"/>.</para>
    /// <para>This should be placed inside of a <see cref="NavBar"/> or <see cref="NavBarMenu"/>.</para>
    /// </summary>
    public partial class NavBarMenu {
        /// <summary>
        /// Will be automatically set by the root.
        /// </summary>
        [CascadingParameter(Name = "Root")]
        [AllowNull]
        public NavBar NavBarRoot { get; init; }

        /// <summary>
        /// Content that is collapsed visible.
        /// </summary>
        [Parameter]
        public RenderFragment? Head { get; set; }

        /// <summary>
        /// Content that is expanded visible.
        /// </summary>
        [Parameter]
        public RenderFragment? Items { get; set; }

        private bool _expanded = false;
        /// <summary>
        /// Value for expanding or collapsing this submenu.
        /// </summary>
        /// <param name="value">expanded</param>
        public bool Expanded {
            get => _expanded;
            set {
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

        private readonly TouchClick dropDownTC;

        public NavBarMenu() => dropDownTC = new TouchClick(PhoneDropDownHandler);

        protected override void OnInitialized() => NavBarRoot.Register(this);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PhoneDropDownHandler(EventArgs e) => Toggle();

        /// <summary>
        /// <para>Expands/Collapses this menu.</para>
        /// <para>Shorthand for: <see cref="Expanded">Expanded</see> = !<see cref="Expanded">Expanded</see>;</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Toggle() => Expanded = !Expanded;
    }
}
