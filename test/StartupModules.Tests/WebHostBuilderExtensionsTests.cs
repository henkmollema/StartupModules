using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace StartupModules.Tests
{
    public class WebHostBuilderExtensionsTests
    {
        [Fact]
        public void ThrowsException_WhenNoBuilderSpecified()
        {
            Assert.Throws<ArgumentNullException>("builder", () => WebHostBuilderExtensions.UseStartupModules(null));
            Assert.Throws<ArgumentNullException>("builder", () => WebHostBuilderExtensions.UseStartupModules(null, Array.Empty<Assembly>()));
        }

        [Fact]
        public void ThrowsException_WhenNoAssembliesSpecified()
        {
            var builder = new WebHostBuilder();
            var ex = Assert.Throws<ArgumentException>("assemblies", () => builder.UseStartupModules((Assembly[])null));

            Assert.Equal(new ArgumentException("No assemblies to discover startup modules from specified.", "assemblies").Message, ex.Message);
        }

        [Fact]
        public void ThrowsException_WhenEmptyssembliesSpecified()
        {
            var builder = new WebHostBuilder();
            var ex = Assert.Throws<ArgumentException>("assemblies", () => builder.UseStartupModules(Array.Empty<Assembly>()));

            Assert.Equal(new ArgumentException("No assemblies to discover startup modules from specified.", "assemblies").Message, ex.Message);
        }

        [Fact]
        public void ThrowsException_WhenNullAssembliesSpecified()
        {
            var builder = new WebHostBuilder();
            var ex = Assert.Throws<ArgumentException>("assemblies", () => builder.UseStartupModules((Assembly)null));

            Assert.Equal(new ArgumentException("No assemblies to discover startup modules from specified.", "assemblies").Message, ex.Message);
        }

        [Fact]
        public void TestTodo()
        {
            // Arrange
            var hostBuilder = CreateBuilder().UseStartupModules(typeof(WebHostBuilderExtensionsTests).Assembly);
        }

        private IWebHostBuilder CreateBuilder() => new WebHostBuilder().UseKestrel().Configure(_ => { });
    }
}
