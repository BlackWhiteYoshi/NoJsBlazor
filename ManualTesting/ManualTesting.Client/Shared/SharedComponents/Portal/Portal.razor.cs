﻿namespace ManualTesting.Client;

/// <summary>
/// Provides a way to render fragments into another location outside of the DOM hierarchy of the parent component.
/// </summary>
public abstract partial class Portal<T> : LanguageServiceComponentBase<T>, IPortal where T : IPortal {
    private readonly HashSet<RenderFragment> renderFragmentList = new();

    /// <summary>
    /// <para>Registers the given RenderFragment.</para>
    /// <para>If already registered, nothing happens.</para>
    /// </summary>
    public void Add(RenderFragment renderFragment) {
        renderFragmentList.Add(renderFragment);
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Unregisters the given RenderFragment.</para>
    /// <para>If not found, nothing happens.</para>
    /// </summary>
    public void Remove(RenderFragment renderFragment) {
        renderFragmentList.Remove(renderFragment);
        InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Triggers a <see cref="ComponentBase.StateHasChanged">StateHasChanged</see>.
    /// </summary>
    public void Rerender() => InvokeAsync(StateHasChanged);
}
