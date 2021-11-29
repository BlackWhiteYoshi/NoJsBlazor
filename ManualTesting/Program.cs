using ManualTesting.Languages;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ManualTesting;

public class Program {
    public static void Main(string[] args) {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<Root>("#app");
        ConfigureServices(builder.Services);
        builder.Build().RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services) {
        services.AddSingleton((IServiceProvider serviceProvider) => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
        services.AddSingleton<ILanguageProvider, LanguageProvider>();
    }
}

/**
 * useful signs
 * „“ … ⋅ × ÷
 **/
