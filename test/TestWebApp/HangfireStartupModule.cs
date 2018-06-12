using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StartupModules;
using Webenable.Hangfire.Contrib;

namespace TestWebApp
{
    public class HangfireStartupModule : IStartupModule
    {

        public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {

            }

            services.AddHangfireContrib(c => c.UseMemoryStorage());
        }

        public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) { }
    }
}
