using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using StartupModules;

namespace TestWebApp
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder.UseStartupModules();
                    webHostBuilder.UseStartup<Startup>();
                });
    }
}
