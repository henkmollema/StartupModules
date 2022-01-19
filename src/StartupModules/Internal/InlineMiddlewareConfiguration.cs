using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StartupModules.Internal;

/// <summary>
/// Provides a <see cref="IStartupModule"/> with inline middleware configuration.
/// </summary>
public class InlineMiddlewareConfiguration : IStartupModule
{
    private readonly Action<IApplicationBuilder, ConfigureMiddlewareContext> _action;

    /// <summary>
    /// Initializes a new instance of <see cref="InlineMiddlewareConfiguration"/>.
    /// </summary>
    /// <param name="action"></param>
    public InlineMiddlewareConfiguration(Action<IApplicationBuilder, ConfigureMiddlewareContext> action)
    {
        _action = action;
    }

    /// <inheritdoc/>
    public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => _action(app, context);

    /// <inheritdoc/>
    public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) { }
}
