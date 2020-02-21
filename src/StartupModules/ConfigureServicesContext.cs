using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StartupModules
{
    /// <summary>
    /// Provides access to useful services during application services configuration.
    /// </summary>
    public class ConfigureServicesContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureServicesContext"/> class.
        /// </summary>
        public ConfigureServicesContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the application <see cref="IConfiguration"/> instance.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the application <see cref="IWebHostEnvironment"/> instance.
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }
    }
}
