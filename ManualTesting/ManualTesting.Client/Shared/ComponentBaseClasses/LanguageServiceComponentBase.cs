using System.Reflection;

namespace ManualTesting.Client;

/// <summary>
/// <para>A normal component that rerenders when language is changed.</para>
/// <para>A component that register itself on initializing at the <see cref="Mediator" /> and unregister on disposing.</para>
/// </summary>
/// <typeparam name="T">The type on which this instance is registering.</typeparam>
public abstract class LanguageServiceComponentBase : LanguageComponentBase, IComponent, IDisposable {
    private static readonly FieldInfo _renderHandleField = typeof(ComponentBase).GetField("_renderHandle", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception($"""field "_renderHandle" in {nameof(ComponentBase)} got renamed""");
    private RenderHandle RenderHandle {
        get => (RenderHandle)_renderHandleField.GetValue(this)!;
        set => _renderHandleField.SetValue(this, value);
    }

    protected bool HasRenderHandle => RenderHandle.IsInitialized;


    void IComponent.Attach(RenderHandle renderHandle) => RenderHandle = renderHandle;

    public new virtual void Dispose() {
        base.Dispose();
        RenderHandle = default;
        GC.SuppressFinalize(this);
    }
}
