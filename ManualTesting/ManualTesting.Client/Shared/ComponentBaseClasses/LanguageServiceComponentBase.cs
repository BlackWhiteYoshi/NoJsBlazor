using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// <para>A normal component that rerenders when language is changed.</para>
/// <para>A component that register itself on initializing at the <see cref="Mediator" /> and unregister on disposing.</para>
/// </summary>
/// <typeparam name="T">The type on which this instance is registering.</typeparam>
public abstract class LanguageServiceComponentBase<T> : LanguageComponentBase, IDisposable {
    [Inject]
    public required IComponentMediator Mediator { protected get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Mediator.Register<T>(this);
    }

    public new void Dispose() {
        base.Dispose();
        Mediator.Unregister<T>();
    }
}
