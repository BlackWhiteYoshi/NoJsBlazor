﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;

namespace ManualTesting {
    public class Program {
        public static void Main(string[] args) {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Root>("#app");
            builder.Services.AddSingleton((IServiceProvider serviceProvider) => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
            builder.Build().RunAsync();
        }
    }
}

/**
 * useful signs
 * „“ … ⋅ × ÷
 **/
