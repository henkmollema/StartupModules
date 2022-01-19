using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace StartupModules.Tests;

public class StartupRunnerTests
{
    [Fact]
    public void ConfiguresServices()
    {
        // Arrange
        var options = new StartupModulesOptions();
        options.AddStartupModule<MyStartupModule>();
        var runner = new StartupModuleRunner(options);
        var services = new ServiceCollection();

        // Act
        runner.ConfigureServices(services, null, null);

        // Assert
        var sd = Assert.Single(services);
        Assert.Equal(typeof(MyStartupModule.MyService), sd.ImplementationType);
    }

    [Fact]
    public void Configures()
    {
        // Arrange
        var options = new StartupModulesOptions();
        var startupModule = new MyStartupModule();
        options.AddStartupModule(startupModule);
        var runner = new StartupModuleRunner(options);

        // Act
        runner.Configure(new ApplicationBuilder(new ServiceCollection().BuildServiceProvider()), null, null);

        // Assert
        Assert.True(startupModule.Configured);
    }

    [Fact]
    public async Task RunsApplicationInitializers()
    {
        // Arrange
        var options = new StartupModulesOptions();
        options.ApplicationInitializers.Add(typeof(MyAppInitializer));
        var runner = new StartupModuleRunner(options);

        // Act
        await runner.RunApplicationInitializers(new ServiceCollection().BuildServiceProvider());

        // Assert
        // wat do ¯\_(ツ)_/¯
    }
}

public class MyStartupModule : IStartupModule
{
    public class MyService { }

    public bool Configured { get; private set; }

    public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) => services.AddSingleton<MyService>();

    public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => Configured = true;
}

public class MyAppInitializer : IApplicationInitializer
{
    public Task Invoke() => Task.CompletedTask;
}
