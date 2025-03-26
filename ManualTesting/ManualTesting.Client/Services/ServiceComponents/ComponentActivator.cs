namespace ManualTesting.Client.Services;

/// <summary>
/// Am <see cref="IComponentActivator"/> that first checks if the given type can be retrieved from the serviceProvider, then tries to create it with the parameterless constructor.
/// </summary>
public sealed class ComponentActivator : IComponentActivator {
    private readonly IServiceProvider serviceProvider;
    private readonly IComponentActivator defaultComponentActivator;

    public ComponentActivator(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;

        Type defaultComponentActivatorClass = typeof(Microsoft.AspNetCore.Components.RenderTree.Renderer).Assembly.GetType("Microsoft.AspNetCore.Components.DefaultComponentActivator") ?? throw new Exception("Microsoft.AspNetCore.Components.DefaultComponentActivator got renamed or does not exists anymore.");
        defaultComponentActivator = (IComponentActivator)Activator.CreateInstance(defaultComponentActivatorClass, [serviceProvider])!;
    }

    public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
        => (IComponent?)serviceProvider.GetService(componentType) ?? defaultComponentActivator.CreateInstance(componentType);
}
