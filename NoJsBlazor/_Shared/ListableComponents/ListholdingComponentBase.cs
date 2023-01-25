namespace NoJsBlazor;

/// <summary>
/// <para>A component that can hold other components in a list.</para>
/// <para>The other components are from type <see cref="ListableComponentBase{T}"/>, so they register itself on initializing in the list.</para>
/// </summary>
/// <typeparam name="T">Component base class of the <see cref="ListableComponentBase{T}"/></typeparam>
public abstract class ListholdingComponentBase<T> : ComponentBase where T : ListableComponentBase<T> {
    /// <summary>
    /// List of all registered <see cref="ListableComponentBase{T}">ListableComponentBase</see> childs.
    /// </summary>
    protected readonly List<T> childList = new();


    /// <summary>
    /// API for <see cref="ListableComponentBase{T}"/> objects to add themselves to the list.
    /// </summary>
    /// <param name="child"></param>
    internal virtual void Add(T child) {
        childList.Add(child);
        StateHasChanged();
    }

    /// <summary>
    /// API for <see cref="ListableComponentBase{T}"/> objects to remove themselves to the list.
    /// </summary>
    /// <param name="child"></param>
    internal virtual void Remove(T child) {
        childList.Remove(child);
        StateHasChanged();
    }


    /// <summary>
    /// Gives the current number of registered childs.
    /// </summary>
    public int ChildCount => childList.Count;
}
