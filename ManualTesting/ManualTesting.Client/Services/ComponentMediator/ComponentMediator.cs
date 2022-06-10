namespace ManualTesting.Client.Services;

/// <summary>
/// <para>A service provider with dynamic lifetime entries.</para>
/// <para>Components can be registered and unregistered any time, normally when that component is instantiated/disposed.</para>
/// </summary>
public class ComponentMediator : IComponentMediator {
    private readonly Dictionary<Type, object> componentList = new();


    /// <summary>
    /// <para>Register a component on the given type.</para>
    /// <para>If multiple registration happens, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name="T">The type on which the component is registered.</typeparam>
    public void Register<T>(ComponentBase component) {
        try {
            componentList.Add(typeof(T), component);
        }
        catch (ArgumentException e) {
            throw new InvalidOperationException($"Component {nameof(T)} was already registered.", e);
        }
    }
    
    /// <summary>
    /// <para>Unregister the component on the given type.</para>
    /// <para>If that type is not registered, nothing happens.</para>
    /// </summary>
    /// <typeparam name="T">The type to be freed.</typeparam>
    public void Unregister<T>() => componentList.Remove(typeof(T));

    
    /// <summary>
    /// <para>Returns the instance registered on the given type.</para>
    /// <para>If that type is not registered, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name="T">The type of the demanded instance.</typeparam>
    /// <returns>The instance of the registered type as the registered type.</returns>
    public T Get<T>() {
        try {
            return (T)componentList[typeof(T)];
        }
        catch (KeyNotFoundException e) {
            throw new InvalidOperationException($"Component {nameof(T)} is currently not registered.", e);
        }
        catch (InvalidCastException e) {
            Type instanceType = componentList[typeof(T)].GetType();
            Type registedType = typeof(T);
            throw new ArgumentException($"{instanceType} is registered on {registedType}, but {instanceType} does not implement/inherit {registedType}", e);
        }
    }
}
