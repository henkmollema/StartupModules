using System.Threading.Tasks;
using StartupModules;

namespace TestWebApp
{
    public class FooAppInitializer : IApplicationInitializer
    {
        public Task Invoke() => Task.CompletedTask;
    }
}
