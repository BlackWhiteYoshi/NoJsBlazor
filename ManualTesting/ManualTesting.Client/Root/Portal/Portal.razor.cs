using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// Provides a way to render fragments into another location outside of the DOM hierarchy of the parent component.
/// </summary>
public sealed partial class Portal : ServiceComponentBase, IPortal {
    private readonly HashSet<RenderFragment> renderFragmentList = new();

    /// <summary>
    /// <para>Registers the given RenderFragment.</para>
    /// <para>If already registered, nothing happens.</para>
    /// </summary>
    public void Add(RenderFragment renderFragment) {
        renderFragmentList.Add(renderFragment);
        if (HasRenderHandle)
            InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Unregisters the given RenderFragment.</para>
    /// <para>If not found, nothing happens.</para>
    /// </summary>
    public void Remove(RenderFragment renderFragment) {
        renderFragmentList.Remove(renderFragment);
        if (HasRenderHandle)
            InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Triggers a <see cref="ComponentBase.StateHasChanged">StateHasChanged</see>.
    /// </summary>
    public void Rerender() {
        if (HasRenderHandle)
            InvokeAsync(StateHasChanged);
    }
}
