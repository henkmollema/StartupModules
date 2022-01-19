using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StartupModules.Internal;

namespace StartupModules;

/// <summary>
/// Provides extensions to configure the startup modules infrastructure and configure or discover <see cref="IStartupModule"/> instances.
/// </summary>
public static class StartupModulesExtensions
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
    public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder, Action<StartupModulesOptions> configure) =>
        builder.ConfigureServices((hostContext, services) =>
            services.AddStartupModules(hostContext.Configuration, hostContext.HostingEnvironment, configure));

    /// <summary>
    /// Configures startup modules with the specified configuration for <see cref="StartupModulesOptions"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder UseStartupModules(this WebApplicationBuilder builder) =>
        builder.UseStartupModules(options => options.DiscoverStartupModules());

    /// <summary>
    /// Configures startup modules with the specified configuration for <see cref="StartupModulesOptions"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="assemblies">The assemblies to discover startup modules from.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder UseStartupModules(this WebApplicationBuilder builder, params Assembly[] assemblies) =>
        builder.UseStartupModules(options => options.DiscoverStartupModules(assemblies));

    /// <summary>
    /// Configures startup modules with the specified configuration for <see cref="StartupModulesOptions"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <param name="configure">A callback to configure <see cref="StartupModulesOptions"/>.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder UseStartupModules(this WebApplicationBuilder builder, Action<StartupModulesOptions> configure)
    {
        builder.Services.AddStartupModules(builder.Configuration, builder.Environment, configure);
        return builder;
    }

    /// <summary>
    /// Configures startup modules with the specified configuration for <see cref="StartupModulesOptions"/>.
    /// </summary>
    /// <param name="services">The service collection to add the StartupModules services to.</param>
    /// <param name="configuration">The application's configuration.</param>
    /// <param name="environment">The application's environment information.</param>
    /// <param name="configure">A callback to configure <see cref="StartupModulesOptions"/>.</param>
    public static void AddStartupModules(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, Action<StartupModulesOptions> configure)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        if (environment == null)
        {
            throw new ArgumentNullException(nameof(environment));
        }

        var options = new StartupModulesOptions();
        configure(options);

        if (options.StartupModules.Count == 0 && options.ApplicationInitializers.Count == 0)
        {
            // Nothing to do here
            return;
        }

        var runner = new StartupModuleRunner(options);
        services.AddSingleton<IStartupFilter>(sp => ActivatorUtilities.CreateInstance<ModulesStartupFilter>(sp, runner));

        var configureServicesContext = new ConfigureServicesContext(configuration, environment, options);
        runner.ConfigureServices(services, configuration, environment);
    }
}
