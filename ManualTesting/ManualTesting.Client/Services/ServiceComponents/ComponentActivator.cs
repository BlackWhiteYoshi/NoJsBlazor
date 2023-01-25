using System.Reflection;

namespace ManualTesting.Client.Services;

/// <summary>
/// A decorator of <see cref="DefaultComponentActivator"/> that first checks if the type is registered in the <see cref="IServiceProvider"/> and retrieves the component there,<br />
/// otherwise do <see cref="DefaultComponentActivator.CreateInstance(Type)"/>
/// </summary>
public sealed class ComponentActivator : IComponentActivator {
    private readonly IServiceProvider serviceProvider;
    private readonly IComponentActivator defaultComponentActivator;

    public ComponentActivator(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;


        const string CLASS_NAME = "Microsoft.AspNetCore.Components.DefaultComponentActivator";
        const string PROPERTY_NAME = "Instance";

        Type type = typeof(IComponentActivator).Assembly.GetType(CLASS_NAME) ?? throw new Exception($"""class "{CLASS_NAME}" cannot be found""");
        PropertyInfo? property = type.GetProperty(PROPERTY_NAME, BindingFlags.Public | BindingFlags.Static) ?? throw new Exception($"""property "{PROPERTY_NAME}" got renamed""");
        defaultComponentActivator = (IComponentActivator)property.GetValue(null)!;
    }


    public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType) {
        IComponent? component = (IComponent?)serviceProvider.GetService(componentType);
        if (component != null)
            return component;

        return defaultComponentActivator.CreateInstance(componentType);
    }
}
