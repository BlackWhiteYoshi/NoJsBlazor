namespace NoJsBlazor;

/// <summary>
/// A Component that can be hold by <see cref="ListholdingComponentBase{T}"/> in a List.
/// </summary>
/// <typeparam name="T">Component base class of the <see cref="ListableComponentBase{T}"/></typeparam>
public abstract class ListableComponentBase<T> : ComponentBase, IDisposable where T : ListableComponentBase<T> {
    [CascadingParameter(Name = "Parent")]
    public required ListholdingComponentBase<T> Parent { get; set; }


    /// <summary>
    /// <para>Registering the component at the parent <see cref="ListableComponentBase{T}">ListableComponentBase</see>.</para>
    /// <para>Therefore the reference of the parent has to be given with a CascadingParameter.</para>
    /// </summary>
    protected override void OnInitialized() => Parent.Add((T)this);

    public void Dispose() {
        Parent.Remove((T)this);
        GC.SuppressFinalize(this);
    }
}
