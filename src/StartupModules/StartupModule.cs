using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StartupModules;

/// <summary>
/// Defines a startup module to configure application services and middleware during startup.
/// An application can define multiple startup modules for each of its modules/components/features.
/// </summary>
public interface IStartupModule
{
    /// <summary>
    /// A callback to configure the application's services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to configure the application's services.</param>
    /// <param name="context">The <see cref="ConfigureServicesContext"/> instance which provides access to useful services.</param>
    void ConfigureServices(IServiceCollection services, ConfigureServicesContext context);

    /// <summary>
    /// A callback to configure the middleware pipeline of the application.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance to configure the middleware pipeline of the application.</param>
    /// <param name="context">The <see cref="ConfigureMiddlewareContext"/> instance which provides access to useful services.</param>
    void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context);
}
