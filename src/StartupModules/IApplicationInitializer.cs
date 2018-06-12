using System.Threading.Tasks;

namespace StartupModules
{
    /// <summary>
    /// Represents a class that initializes application services during startup.
    /// </summary>
    public interface IApplicationInitializer
    {
        /// <summary>
        /// Invokes the <see cref="IApplicationInitializer"/> instance.
        /// </summary>
        Task Invoke();
    }
}
