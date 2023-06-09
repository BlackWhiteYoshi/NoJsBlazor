﻿using ManualTesting.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ManualTesting.Client;

public static class Program {
    public static Task Main(string[] args) {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        ConfigureServices(builder.Services);
        return builder.Build().RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services) {
        services.AddSingleton<PreRendering>(() => false);
        services.AddCoreServices();
    }
}
