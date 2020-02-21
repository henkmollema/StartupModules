﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules.Internal
{
    /// <summary>
    /// A startup filter that invokes the configured <see cref="IStartupModule"/>'s and <see cref="IApplicationInitializer"/>'s.
    /// </summary>
    public class ModulesStartupFilter : IStartupFilter
    {
        private readonly StartupModuleRunner _runner;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModulesStartupFilter"/> class.
        /// </summary>
        public ModulesStartupFilter(StartupModuleRunner runner, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _runner = runner;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            _runner.Configure(app, _configuration, _hostingEnvironment);
            _runner.RunApplicationInitializers(app.ApplicationServices).GetAwaiter().GetResult();
            next(app);
        };
    }
}
