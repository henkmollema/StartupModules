using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StartupModules.Internal
{
    /// <summary>
    /// A startup filter that invokes <see cref="IStartupModule"/>s and <see cref="IApplicationInitializer"/>s.
    /// </summary>
    public class ModulesStartupFilter : IStartupFilter
    {
        private readonly StartupModulesOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ModulesStartupFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModulesStartupFilter"/> class.
        /// </summary>
        public ModulesStartupFilter(StartupModulesOptions options, IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILogger<ModulesStartupFilter> logger)
        {
            _options = options;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            next(app);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Create a middleware configuration context with a scoped service provider
                var scopedServiceProvider = scope.ServiceProvider;
                var ctx = new ConfigureMiddlewareContext
                {
                    Configuration = _configuration,
                    HostingEnvironment = _hostingEnvironment,
                    ServiceProvider = scopedServiceProvider
                };

                foreach (var module in _options.StartupModules)
                {
                    // Invoke the callback to configure middleware
                    _logger.LogInformation("Invoking the configure middleware callback for startup module {StartupModuleName}", module.GetType().Name);
                    module.Configure(app, ctx);
                }

                var applicationInitializers = _options.ApplicationInitializers
                    .Select(t =>
                    {
                        try
                        {
                            return ActivatorUtilities.CreateInstance(scopedServiceProvider, t);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException($"Failed to create instace of {nameof(IApplicationInitializer)} '{t.Name}'.", ex);
                        }
                    })
                    .Cast<IApplicationInitializer>()
                    .ToArray();

                foreach (var initializer in applicationInitializers)
                {
                    try
                    {
                        _logger.LogInformation("Invoking application initializer {ApplicationInitializerName}", initializer.GetType().Name);
                        initializer.Invoke().GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"An exception occured during the execution of {nameof(IApplicationInitializer)} '{initializer.GetType().Name}'.", ex);
                    }
                }
            }
        };
    }
}
