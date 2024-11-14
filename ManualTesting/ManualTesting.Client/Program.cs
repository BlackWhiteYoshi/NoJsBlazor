using ManualTesting.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ManualTesting.Client;

public static class Program {
    public static Task Main(string[] args) {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        bool isDevelopmentEnvironment = builder.HostEnvironment.IsDevelopment();

        // configure Services
        {
            IServiceCollection services = builder.Services;

            services.AddCoreServices();
        }

        // add root components
        builder.RootComponents.Add<IApp>("#anchor");

        return builder.Build().RunAsync();
    }
}
