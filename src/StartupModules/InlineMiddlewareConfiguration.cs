using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StartupModules
{
    internal class InlineMiddlewareConfiguration : IStartupModule
    {
        private readonly Action<IApplicationBuilder, ConfigureMiddlewareContext> _action;

        public InlineMiddlewareConfiguration(Action<IApplicationBuilder, ConfigureMiddlewareContext> action)
        {
            _action = action;
        }

        public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => _action(app, context);

        public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) { }
    }
}
