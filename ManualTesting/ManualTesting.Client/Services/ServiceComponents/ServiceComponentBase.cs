using System.Runtime.CompilerServices;

namespace ManualTesting.Client.Services;

/// <summary>
/// Same as <see cref="ComponentBase"/>, but<br />
/// - doesn't do the reassign check in <see cref="IComponent.Attach(RenderHandle)"/><br />
/// - has a Dispose() method that resets the <see cref="RenderHandle"/><br />
/// - provide Property <see cref="HasRenderHandle"/>
/// </summary>
public abstract class ServiceComponentBase : ComponentBase, IComponent, IDisposable {
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_renderHandle")]
    private extern static ref RenderHandle GetRenderHandle(ComponentBase @this);

    private RenderHandle RenderHandle {
        get => GetRenderHandle(this);
        set => GetRenderHandle(this) = value;
    }


    protected bool HasRenderHandle => RenderHandle.IsInitialized;

    void IComponent.Attach(RenderHandle renderHandle) => RenderHandle = renderHandle;

    public virtual void Dispose() => RenderHandle = default;
}
