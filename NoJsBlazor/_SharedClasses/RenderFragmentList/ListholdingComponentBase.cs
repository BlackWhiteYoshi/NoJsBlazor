using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace NoJsBlazor {
    /// <summary>
    /// <para>A component that can hold other components in a list.</para>
    /// <para>The other components are from type <see cref="ListableComponentBase{T}"/>, so they register itself on initializing in the list.</para>
    /// </summary>
    /// <typeparam name="T">Component base class of the <see cref="ListableComponentBase{T}"/></typeparam>
    public class ListholdingComponentBase<T> : ComponentBase where T : ListableComponentBase<T> {
        /// <summary>
        /// List of all registered <see cref="ListableComponentBase{T}">ListableComponentBase</see> childs.
        /// </summary>
        protected readonly List<T> childList = new List<T>();

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
    }
}
