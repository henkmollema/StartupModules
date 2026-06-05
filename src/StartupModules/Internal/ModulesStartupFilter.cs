using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules.Internal;

/// <summary>
/// A startup filter that invokes the configured <see cref="IStartupModule"/>'s and <see cref="IApplicationInitializer"/>'s.
/// </summary>
public class ModulesStartupFilter(StartupModuleRunner runner, IConfiguration configuration, IWebHostEnvironment hostingEnvironment) : IStartupFilter
{
    /// <inheritdoc/>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
    {
        runner.Configure(app, configuration, hostingEnvironment);
        runner.RunApplicationInitializers(app.ApplicationServices).GetAwaiter().GetResult();
        next(app);
    };
}
