using ManualTesting.Client.Services;

namespace ManualTesting.Client;

/// <summary>
/// A component that register itself on initializing at the <see cref="Mediator" /> and unregister on disposing.
/// </summary>
/// <typeparam name="T">The type on which this instance is registering.</typeparam>
public abstract class ServiceComponentBase<T> : ComponentBase, IDisposable {
    [Inject, AllowNull]
    protected IComponentMediator Mediator { get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        Mediator.Register<T>(this);
    }

    public void Dispose() {
        Mediator.Unregister<T>();
        GC.SuppressFinalize(this);
    }
}
