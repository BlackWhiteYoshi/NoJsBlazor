using System.Reflection;

namespace ManualTesting.Client.Services;

/// <summary>
/// Same as <see cref="ComponentBase"/>, but<br />
/// - doesn't do the reassign check in <see cref="IComponent.Attach(RenderHandle)"/><br />
/// - has a Dispose() method that resets the <see cref="RenderHandle"/><br />
/// - provide Property <see cref="HasRenderHandle"/>
/// </summary>
public abstract class ServiceComponentBase : ComponentBase, IComponent, IDisposable {
    private static readonly FieldInfo renderHandleField = typeof(ComponentBase).GetField("_renderHandle", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception($"""field "_renderHandle" in {nameof(ComponentBase)} got renamed""");
    private RenderHandle RenderHandle {
        get => (RenderHandle)renderHandleField.GetValue(this)!;
        set => renderHandleField.SetValue(this, value);
    }

    protected bool HasRenderHandle => RenderHandle.IsInitialized;


    void IComponent.Attach(RenderHandle renderHandle) => RenderHandle = renderHandle;

    public virtual void Dispose() => RenderHandle = default;
}
