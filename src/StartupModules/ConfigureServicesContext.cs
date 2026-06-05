using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules;

/// <summary>
/// Provides access to useful services during application services configuration.
/// </summary>
public class ConfigureServicesContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, StartupModulesOptions options)
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
    /// Gets the <see cref="StartupModulesOptions"/>.
    /// </summary>
    public StartupModulesOptions Options { get; } = options;
}
