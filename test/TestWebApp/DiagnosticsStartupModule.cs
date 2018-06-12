using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StartupModules;
using Webenable.Diagnostics;

namespace TestWebApp
{
    public class DiagnosticsStartupModule : IStartupModule
    {
        public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
        {
            services.AddDiagnosticTests();
            services.RegisterDiagnosticTests();
        }

        public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context)
        {
            app.UseDiagnosticTests("/health");
        }
    }
}
