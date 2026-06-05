using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace StartupModules.Tests;

public class FunctionalTests
{
    [Fact]
    public async Task ConfiguresServicesAndMiddleware()
    {
        using var host = new HostBuilder()
            .ConfigureWebHost(x =>
            {
                x.UseTestServer();
                x.Configure(_ => { });
                x.UseStartupModules(c => c.AddStartupModule<FooStartupModule>());
            })
            .Build();
        await host.StartAsync();
        var server = host.GetTestServer();
        var client = server.CreateClient();
        var msg = await client.GetStringAsync("/message");

        Assert.Equal("Hello, World!", msg);
    }
}

public class FooStartupModule : IStartupModule
{
    public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
    {
        services.AddSingleton<MessageProvider>();
    }

    public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context)
    {
        app.Map("/message", sub => sub.Run(async ctx =>
        {
            var msgProvider = ctx.RequestServices.GetRequiredService<MessageProvider>();
            await ctx.Response.WriteAsync(msgProvider.GetMessage());
        }));
    }
}

public class MessageProvider
{
    public string GetMessage() => "Hello, World!";
}
