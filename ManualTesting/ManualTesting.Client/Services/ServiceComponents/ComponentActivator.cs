using System.Reflection;

namespace ManualTesting.Client.Services;

/// <summary>
/// Am <see cref="IComponentActivator"/> that first checks if the given type can be retrieved from the serviceProvider, then tries to create it with the parameterless constructor.
/// </summary>
public sealed class ComponentActivator(IServiceProvider serviceProvider) : IComponentActivator {
    public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
        => (IComponent?)serviceProvider.GetService(componentType) ?? (IComponent)Activator.CreateInstance(componentType)!;
}
