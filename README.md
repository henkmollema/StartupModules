# StartupModules
Startup modules for ASP.NET Core.

Create modular and focused Startup-like classes for each of your application's features/components/modules and keep your startup file sane.

| Windows | Linux | NuGet |
| ------- | ----- | ----- |
| [![Windows](https://ci.appveyor.com/api/projects/status/jq76jr3b5cmxs5v6/branch/master?svg=true)](https://ci.appveyor.com/project/henkmollema/startupmodules/branch/master) | [![Linux](https://travis-ci.org/henkmollema/StartupModules.svg?branch=master)](https://travis-ci.org/henkmollema/StartupModules) | [![NuGet](https://img.shields.io/nuget/vpre/StartupModules.svg?style=flat-square)](https://www.nuget.org/packages/StartupModules) 

## Installation

#### StartupModules is [available on NuGet](https://www.nuget.org/packages/StartupModules) for ASP.NET Core 2.1.

## Getting started

### Create a startup module

Creating a startup module is easy. Create a new class and inherit from the `IStartupModule` interface:

```cs
public class MyStartupModule : IStartupModule
{
    public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
    {
    }

    public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context)
    {
    }
}
```

You'll notice the familiair `ConfigureServices` and `Configure` method. A convient context-object is passed to both of them providing useful services while configuring your application, such as `IHostingEnvironment` and `IConfiguration`. A scoped `IServiceProvider` is present in the `ConfigureMiddlewareContext` as well.

### Configure startup modules

You can configure startup modules using the `UseStartupModules` when building the web host in `Program.cs`:

```cs
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartupModules()
        .UseStartup<Startup>();
```

This will automatically discover startup modules (and application initializer) in the entry assembly of your application. You can also specify an array of assemblies to discover startup modules from:

```cs
.UseStartupModules(typeof(Startup).Assembly, typeof(SomeTypeInAnotherAssembly).Assembly)
```

You can have more control of the configuration using the overload with the options:

```cs
.UseStartupModules(options =>
{
    // Discover from entry assembly
    o.DiscoverStartupModules();

    // Discover from specific assemblies
    o.DiscoverStartupModules(typeof(Startup).Assembly, typeof(SomeTypeInAnotherAssembly).Assembly);

    // Add individual startup modules
    o.AddStartupModule<MyStartupModule>();
})
```

More docs will follow later.

### Application Initializers
Application initializers allow you to write startup logic for your application, such as configuring your Entity Framework database context and executing migrations. Applications initializers are, just like startup modules, discovered automatically as well.

```cs
public class DatabaseInitializer : IApplicationInitializer
{
    private readonly AppDbContext _dbContext;

    public DatabaseInitializer(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Task Invoke()
    {
        await _dbContext.MigrateAsync();
    }
}
```

You can specify any number of dependencies available in your application via the constructor of an application initializer. The dependencies will be resolved from a **scoped** service provider instance and will be disposed after application startup. This prevents common pitfalls such as a resolving a singleton database context and leaking its connection for the lifetime off the application and avoids the hassle of creating a service provider scope yourself.