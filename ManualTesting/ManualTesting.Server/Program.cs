using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.JSInterop;
using ManualTesting.Client.Languages;

namespace ManualTesting.Server;

public class Program {
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);
        WebApplication app = builder.Build();

        ConfigurePipeline(app);
        app.Run();
    }


    private static void ConfigureServices(IServiceCollection services) {
        services.AddRazorPages((RazorPagesOptions options) => options.RootDirectory = "/");

        services.AddResponseCompression((ResponseCompressionOptions options) => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }));

        services.AddSingleton<IJSInProcessRuntime, UnsupportedJSInProcessRuntime>();
        services.AddScoped<ILanguageProvider, LanguageProvider>();
    }

    private static void ConfigurePipeline(WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
            app.UseExceptionHandler("/");

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapFallbackToPage("/_Host");
    }



    private class UnsupportedJSInProcessRuntime : IJSInProcessRuntime {
        public TResult Invoke<TResult>(string identifier, params object?[]? args) => throw new NotSupportedException();
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new NotSupportedException();
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotSupportedException();
    }
}
