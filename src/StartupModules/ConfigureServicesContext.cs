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
#if NETSTANDARD2_0
        public ConfigureServicesContext(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
#elif NETCOREAPP3_0
        public ConfigureServicesContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
#endif
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the application <see cref="IConfiguration"/> instance.
        /// </summary>
        public IConfiguration Configuration { get; }

#if NETSTANDARD2_0
        /// <summary>
        /// Gets the application <see cref="IHostingEnvironment"/> instance.
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; set; }
#elif NETCOREAPP3_0

        /// <summary>
        /// Gets the application <see cref="IWebHostEnvironment"/> instance.
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }
#endif
    }
}
