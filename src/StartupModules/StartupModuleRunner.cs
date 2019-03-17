using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StartupModules
{
    /// <summary>
    /// A runner for <see cref="IStartupModule"/>'s discoverd via <see cref="StartupModulesOptions"/>.
    /// </summary>
    public class StartupModuleRunner
    {
        private readonly StartupModulesOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupModuleRunner"/> class.
        /// </summary>
        /// <param name="options">The <see cref="StartupModulesOptions"/> to discover <see cref="IStartupModule"/>'s.</param>
        public StartupModuleRunner(StartupModulesOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Calls <see cref="IStartupModule.ConfigureServices(IServiceCollection, ConfigureServicesContext)"/> on the
        /// discoverd <see cref="IStartupModule"/>'s.
        /// </summary>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var ctx = new ConfigureServicesContext
            {
                Configuration = configuration,
                WebHostEnvironment = webHostEnvironment
            };

            foreach (var cfg in _options.StartupModules)
            {
                cfg.ConfigureServices(services, ctx);
            }
        }

        /// <summary>
        /// Calls <see cref="IStartupModule.Configure(IApplicationBuilder, ConfigureMiddlewareContext)"/> on the 
        /// discovered <see cref="IStartupModule"/>.
        /// </summary>
        public void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var ctx = new ConfigureMiddlewareContext
                {
                    Configuration = configuration,
                    WebHostEnvironment = webHostEnvironment,
                    ServiceProvider = scope.ServiceProvider
                };

                foreach (var cfg in _options.StartupModules)
                {
                    cfg.Configure(app, ctx);
                }
            }
        }

        /// <summary>
        /// Invokes the discovered <see cref="IApplicationInitializer"/> instances.
        /// </summary>
        /// <param name="serviceProvider">The application's root service provider.</param>
        public async Task RunApplicationInitializers(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var applicationInitializers = _options.ApplicationInitializers
                .Select(t =>
                {
                    try
                    {
                        return ActivatorUtilities.CreateInstance(scope.ServiceProvider, t);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to create instace of {nameof(IApplicationInitializer)} '{t.Name}'.", ex);
                    }
                })
                .Cast<IApplicationInitializer>();

                foreach (var initializer in applicationInitializers)
                {
                    try
                    {
                        await initializer.Invoke();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"An exception occured during the execution of {nameof(IApplicationInitializer)} '{initializer.GetType().Name}'.", ex);
                    }
                }
            }
        }
    }
}
