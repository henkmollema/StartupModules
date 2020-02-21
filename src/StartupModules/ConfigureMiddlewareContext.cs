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
        /// Initializes a new instance of the <see cref="ConfigureServicesContext"/> class.
        /// </summary>
        public ConfigureMiddlewareContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the application <see cref="IConfiguration"/> instance.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the application <see cref="IWebHostEnvironment"/> instance.
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance scoped for the lifetime of application startup.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
    }
}
