using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StartupModules.Internal;

namespace StartupModules
{
    /// <summary>
    /// Provides extensions to configure the startup modules infrastructure and configure or discover <see cref="IStartupModule"/> instances.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Configures startup modules and automatically discovers <see cref="IStartupModule"/>'s from the applications entry assembly.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> instance.</param>
        /// <returns>The <see cref="IWebHostBuilder"/> instance.</returns>
        public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder) =>
            UseStartupModules(builder, options => options.DiscoverStartupModules());

        /// <summary>
        /// Configures startup modules and automatically discovers <see cref="IStartupModule"/>'s from the specified assemblies.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> instance.</param>
        /// <param name="assemblies">The assemblies to discover startup modules from.</param>
        /// <returns>The <see cref="IWebHostBuilder"/> instance.</returns>
        public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder, params Assembly[] assemblies) =>
            UseStartupModules(builder, options => options.DiscoverStartupModules(assemblies));

        /// <summary>
        /// Configures startup modules with the specified configuration for <see cref="StartupModulesOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> instance.</param>
        /// <param name="configure">A callback to configure <see cref="StartupModulesOptions"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/> instance.</returns>
        public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder, Action<StartupModulesOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new StartupModulesOptions();
            configure(options);

            if (options.StartupModules.Count == 0 && options.ApplicationInitializers.Count == 0)
            {
                // Nothing to do here
                return builder;
            }

            var runner = new StartupModuleRunner(options);
            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IStartupFilter>(sp => ActivatorUtilities.CreateInstance<ModulesStartupFilter>(sp, runner));

                var configureServicesContext = new ConfigureServicesContext
                {
                    Configuration = hostContext.Configuration,
                    WebHostEnvironment = hostContext.HostingEnvironment
                };

                runner.ConfigureServices(services, hostContext.Configuration, hostContext.HostingEnvironment);
            });

            return builder;
        }
    }
}
