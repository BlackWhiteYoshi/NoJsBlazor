namespace ManualTesting.Client.Services;

public static class CoreServicesExtension {
    /// <summary>
    /// Add dependencies to the ServiceCollection which are necessary for all entry points the same.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services) {
        services.AddScoped((IServiceProvider serviceProvider) => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
        services.AddScoped<IComponentActivator, ComponentActivator>();

        // component services
        services.AddScoped<IRoot, Root>();
        services.AddScoped<IPortal, Portal>();
        
        return services;
    }
}
