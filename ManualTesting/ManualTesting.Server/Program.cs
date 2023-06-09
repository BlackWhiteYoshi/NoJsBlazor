﻿using ManualTesting.Client.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManualTesting.Server;

public static class Program {
    public interface IAssemblyMarker { }


    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);
        WebApplication app = builder.Build();

        ConfigurePipeline(app);
        app.Run();
    }


    private static void ConfigureServices(IServiceCollection services) {
        services.AddRazorPages((RazorPagesOptions options) => options.RootDirectory = "/");
        
        services.AddSingleton<PreRendering>(() => true);
        services.AddCoreServices();
    }

    private static void ConfigurePipeline(WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.MapFallbackToPage("/_Host");
    }
}
