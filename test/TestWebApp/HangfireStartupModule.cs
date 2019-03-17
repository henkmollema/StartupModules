﻿using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StartupModules;
using Webenable.Hangfire.Contrib;

namespace TestWebApp
{
    public class HangfireStartupModule : IStartupModule
    {
        public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
        {
            if (context.WebHostEnvironment.IsDevelopment())
            {
                // Do something based on this
            }

            services.AddHangfireContrib(c => c.UseMemoryStorage());
        }

        public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) { }
    }
}
