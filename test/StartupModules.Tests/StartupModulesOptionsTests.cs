using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StartupModules.Internal;
using Xunit;

namespace StartupModules.Tests
{
    public class StartupModulesOptionsTests
    {
        [Fact]
        public void AddStartupModule_FromGenericType()
        {
            var options = new StartupModulesOptions();
            options.AddStartupModule<FooStartupModule>();

            var module = Assert.Single(options.StartupModules);
            Assert.IsType<FooStartupModule>(module);
        }

        [Fact]
        public void AddStartupModule_FromType()
        {
            var options = new StartupModulesOptions();
            options.AddStartupModule(typeof(FooStartupModule));

            var module = Assert.Single(options.StartupModules);
            Assert.IsType<FooStartupModule>(module);
        }

        [Fact]
        public void AddStartupModule_FromInstance()
        {
            var options = new StartupModulesOptions();
            options.AddStartupModule(new FooStartupModule());

            var module = Assert.Single(options.StartupModules);
            Assert.IsType<FooStartupModule>(module);
        }

        [Fact]
        public void ThrowsException_WhenTypeNotIStartupModule()
        {
            var options = new StartupModulesOptions();
            var ex = Assert.Throws<ArgumentException>("type", () => options.AddStartupModule(typeof(decimal)));
            var expectedEx = new ArgumentException($"Specified startup module '{typeof(decimal).Name}' does not implement {nameof(IStartupModule)}.", "type");
            Assert.Equal(expectedEx.Message, ex.Message);
        }

        [Fact]
        public void ConfigureMiddleware_AddsStartupModule()
        {
            var options = new StartupModulesOptions();
            options.ConfigureMiddleware((app, ctx) => { });

            var module = Assert.Single(options.StartupModules);
            Assert.IsType<InlineMiddlewareConfiguration>(module);
            module.ConfigureServices(null, null);
            module.Configure(null, null);
        }

        [Fact]
        public void ThrowsException_WhenNoParameterlessConstructor()
        {
            var options = new StartupModulesOptions();
            var ex = Assert.Throws<InvalidOperationException>(() => options.AddStartupModule<StartupModuleWithCtor>());
            var expectedMsg = $"Failed to create instance for {nameof(IStartupModule)} type '{typeof(StartupModuleWithCtor).Name}'.";
            Assert.Equal(expectedMsg, ex.Message);
        }

        [Fact]
        public void ThrowsException_WithErrorConstructor()
        {
            var options = new StartupModulesOptions();
            var ex = Assert.Throws<InvalidOperationException>(() => options.AddStartupModule<StartupModuleWithErrorCtor>());
            var expectedMsg = $"Failed to create instance for {nameof(IStartupModule)} type '{typeof(StartupModuleWithErrorCtor).Name}'.";
            Assert.Equal(expectedMsg, ex.Message);
        }

        // Internal to avoid automatic discovery
        internal class FooStartupModule : IStartupModule
        {
            public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => throw new NotImplementedException();
            public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) => throw new NotImplementedException();
        }

        // Internal to avoid automatic discovery
        internal class StartupModuleWithCtor : IStartupModule
        {
            public StartupModuleWithCtor(object o) { }

            public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => throw new NotImplementedException();
            public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) => throw new NotImplementedException();
        }

        // Internal to avoid automatic discovery
        internal class StartupModuleWithErrorCtor : IStartupModule
        {
            public StartupModuleWithErrorCtor()
            {
                throw new ArgumentException();
            }

            public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context) => throw new NotImplementedException();
            public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context) => throw new NotImplementedException();
        }
    }
}
