using ManualTesting.Client.Languages;
using ManualTesting.Client.PreRendering;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ManualTesting.Client;

public class Program {
    public static void Main(string[] args) {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        ConfigureServices(builder.Services);
        builder.Build().RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services) {
        services.AddSingleton((IServiceProvider serviceProvider) => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
        services.AddSingleton<IPreRenderFlag, PreRenderFlag>();
        services.AddSingleton<ILanguageProvider, LanguageProvider>();
    }



    private class PreRenderFlag : IPreRenderFlag {
        public bool Flag => false;
    }
}

/**
 * useful signs
 * „“ … ⋅ × ÷
 **/
