using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules;

/// <summary>
/// Provides access to useful services during middleware configuration.
/// </summary>
public class ConfigureMiddlewareContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IServiceProvider serviceProvider, StartupModulesOptions options)
{
    /// <summary>
    /// Gets the application <see cref="IConfiguration"/> instance.
    /// </summary>
    public IConfiguration Configuration { get; } = configuration;

    /// <summary>
    /// Gets the application <see cref="IWebHostEnvironment"/> instance.
    /// </summary>
    public IWebHostEnvironment HostingEnvironment { get; } = hostingEnvironment;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance scoped for the lifetime of application startup.
    /// </summary>
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the <see cref="StartupModulesOptions"/>.
    /// </summary>
    public StartupModulesOptions Options { get; } = options;
}
