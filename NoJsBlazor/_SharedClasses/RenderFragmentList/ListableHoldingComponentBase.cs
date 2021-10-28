using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace NoJsBlazor {
    /// <summary>
    /// <para>It's actually just a combination of <see cref="ListableComponentBase{T}"/> and <see cref="ListholdingComponentBase{T}"/>.</para>
    /// <para>But since multiple inheritence doesn't work, an ugly workaround is used.</para>
    /// </summary>
    /// <typeparam name="T">Base class</typeparam>
    public abstract class ListableHoldingComponentBase<T> : ComponentBase where T : ListableHoldingComponentBase<T> {
        #region Listable

        [CascadingParameter(Name = "Parent")]
        public ListableHoldingComponentBase<T>? Parent { get; set; }

        /// <summary>
        /// <para>Registering the component at the parent <see cref="ListableComponentBase{T}"/>.</para>
        /// <para>Therefore the reference of the parent has to be given with a CascadingParameter.</para>
        /// </summary>
        protected override void OnInitialized() {
            if (Parent != null)
                Parent.Register((T)this);
        }

        #endregion

        #region Listholding

        /// <summary>
        /// List of all registered <see cref="ListableComponentBase{T}"/> childs.
        /// </summary>
        protected readonly List<T> childList = new();

        /// <summary>
        /// API for <see cref="ListableComponentBase{T}"/> objects to add themselves to the list.
        /// </summary>
        /// <param name="child"></param>
        internal void Register(T child) {
            childList.Add(child);
            StateHasChanged();
        }

        /// <summary>
        /// Gives the current number of registered childs.
        /// </summary>
        public int ChildCount => childList.Count;

        #endregion
    }
}
