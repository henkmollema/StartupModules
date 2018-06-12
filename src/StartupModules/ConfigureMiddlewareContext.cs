using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules
{
    /// <summary>
    /// Provides access to useful services during middleware configuration.
    /// </summary>
    public class ConfigureMiddlewareContext
    {
        /// <summary>
        /// Gets the application <see cref="IConfiguration"/> instance.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets the application <see cref="IHostingEnvironment"/> instance.
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance scoped for the lifetime of application startup.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
    }
}
