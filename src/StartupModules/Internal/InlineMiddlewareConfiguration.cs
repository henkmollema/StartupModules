using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StartupModules.Internal;

/// <summary>
/// Provides a <see cref="IStartupModule"/> with inline middleware configuration.
/// </summary>
public class InlineMiddlewareConfiguration(Action<IApplicationBuilder, ConfigureMiddlewareContext> action) : IStartupModule
{
    /// <inheritdoc/>
    public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => action(app, context);

    /// <inheritdoc/>
    public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) { }
}
