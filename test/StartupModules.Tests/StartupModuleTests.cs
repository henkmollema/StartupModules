using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace StartupModules.Tests
{
    public class StartupModuleTests
    {
        [Fact]
        public async Task ConfiguresContext()
        {
            var hostBuilder = CreateBuilder().UseStartupModules(o => o.AddStartupModule<FooStartupModule>());
            using var host = hostBuilder.Build();
            await host.StartAsync();
            await host.StopAsync();
        }

        [Fact]
        public async Task DiscoversFromSpecifiedAssembly()
        {
            var hostBuilder = CreateBuilder().UseStartupModules(o => o.DiscoverStartupModules(typeof(FooStartupModule).Assembly));
            using var host = hostBuilder.Build();
            await host.StartAsync();
            await host.StopAsync();
        }

        [Fact]
        public async Task DiscoversFromEntryAssembly()
        {
            // Equivalent of calling UseStartupModules()
            var hostBuilder = CreateBuilder().UseStartupModules(o => o.DiscoverStartupModules());
            using var host = hostBuilder.Build();
            await host.StartAsync();
            await host.StopAsync();
        }

        [Fact]
        public async Task DiscoversFromEntryAssemblyWithDefaultMethod()
        {
            // Equivalent of calling UseStartupModules()
            var hostBuilder = CreateBuilder().UseStartupModules();
            using var host = hostBuilder.Build();
            await host.StartAsync();
            await host.StopAsync();
        }

        private IWebHostBuilder CreateBuilder() => new WebHostBuilder().UseKestrel().Configure(_ => { });

        public class FooStartupModule : IStartupModule
        {
            public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
            {
                Assert.NotNull(services);
                Assert.NotNull(context);
                Assert.NotNull(context.Configuration);
                Assert.NotNull(context.HostingEnvironment);
            }

            public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context)
            {
                Assert.NotNull(app);
                Assert.NotNull(context);
                Assert.NotNull(context.Configuration);
                Assert.NotNull(context.HostingEnvironment);
                Assert.NotNull(context.ServiceProvider);
            }
        }

        public class FooApplicationInitializer : IApplicationInitializer
        {
            private readonly IHostApplicationLifetime _applicationLifetime;

            public FooApplicationInitializer(IHostApplicationLifetime applicationLifetime)
            {
                _applicationLifetime = applicationLifetime;
            }

            public Task Invoke()
            {
                Assert.NotNull(_applicationLifetime);
                return Task.CompletedTask;
            }
        }
    }
}
