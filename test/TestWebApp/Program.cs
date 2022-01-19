using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using StartupModules;

namespace TestWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartupModules(x => x.Settings["AddHangfire"] = true)
            .UseStartup<Startup>();
}
