using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StartupModules.Internal;
using Xunit;

namespace StartupModules.Tests;

public class StartupModulesExtensionsTests
{
    [Fact]
    public void Noops_WhenNoStartupModulesAndApplicationInitializers()
    {
        using var builder = new HostBuilder()
            .ConfigureWebHost(x => x.UseStartupModules(_ => { }))
            .Build();

        Assert.DoesNotContain(builder.Services.GetService<IEnumerable<IStartupFilter>>(), x => x is ModulesStartupFilter);
    }

    [Fact]
    public void Noops_WhenNoStartupModulesAndApplicationInitializersInEntryAssembly()
    {
        using var builder = new HostBuilder()
            .ConfigureWebHost(x => x.UseStartupModules(_ => { }))
            .Build();

        Assert.DoesNotContain(builder.Services.GetService<IEnumerable<IStartupFilter>>(), x => x is ModulesStartupFilter);
    }
}
