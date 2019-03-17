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
        /// Gets the application <see cref="IConfiguration"/> instance.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets the application <see cref="IWebHostEnvironment"/> instance.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; set; }
    }
}
