using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StartupModules.Internal;
using Xunit;

namespace StartupModules.Tests;

public class StartupModulesExtensionsTests
{
    [Fact]
    public void Noops_WhenNoStartupModulesAndApplicationInitializers()
    {
        var builder = new WebHostBuilder()
            .Configure(_ => { })
            .UseStartupModules(_ => { })
            .Build();

        Assert.Empty(builder.Services.GetService<IEnumerable<IStartupFilter>>().Where(x => x is ModulesStartupFilter));
    }

    [Fact]
    public void Noops_WhenNoStartupModulesAndApplicationInitializersInEntryAssembly()
    {
        var builder = new WebHostBuilder()
            .Configure(_ => { })
            .UseStartupModules()
            .Build();

        Assert.Empty(builder.Services.GetService<IEnumerable<IStartupFilter>>().Where(x => x is ModulesStartupFilter));
    }
}
