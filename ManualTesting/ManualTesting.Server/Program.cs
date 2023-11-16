using ManualTesting.Client.Services;

namespace ManualTesting.Server;

public static class Program {
    public interface IAssemblyMarker;


    public static Task Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        bool isDevelopmentEnvironment = builder.Environment.IsDevelopment();


        // configure Services
        {
            IServiceCollection services = builder.Services;

            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            services.AddSingleton<PreRendering>(() => true);
            services.AddCoreServices();
        }

        WebApplication app = builder.Build();

        // configure Pipeline
        {
            if (isDevelopmentEnvironment) {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<Root>().AddAdditionalAssemblies(typeof(Client.App).Assembly)
                .AddInteractiveWebAssemblyRenderMode();
        }


        return app.RunAsync();
    }
}
