using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using StartupModules.Internal;

namespace StartupModules
{
    /// <summary>
    /// Specifies options for startup modules.
    /// </summary>
    public class StartupModulesOptions
    {
        /// <summary>
        /// Gets a collection of <see cref="IStartupModule"/> instances to configure the application with.
        /// </summary>
        public ICollection<IStartupModule> StartupModules { get; } = new List<IStartupModule>();

        /// <summary>
        /// Gets a collection of <see cref="IApplicationInitializer"/> types to initialize the application with.
        /// </summary>
        public ICollection<Type> ApplicationInitializers { get; } = new List<Type>();

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public IDictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Discovers <see cref="IStartupModule"/> implementations from the entry assembly of the application.
        /// </summary>
        public void DiscoverStartupModules() => DiscoverStartupModules(Assembly.GetEntryAssembly()!);

        /// <summary>
        /// Discovers <see cref="IStartupModule"/> implementations from the specified assemblies.
        /// </summary>
        public void DiscoverStartupModules(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0 || assemblies.All(a => a == null))
            {
                throw new ArgumentException("No assemblies to discover startup modules from specified.", nameof(assemblies));
            }

            foreach (var type in assemblies.SelectMany(a => a.ExportedTypes))
            {
                if (typeof(IStartupModule).IsAssignableFrom(type))
                {
                    var instance = Activate(type);
                    StartupModules.Add(instance);
                }
                else if (typeof(IApplicationInitializer).IsAssignableFrom(type))
                {
                    ApplicationInitializers.Add(type);
                }
            }
        }

        /// <summary>
        /// Adds the <see cref="IStartupModule"/> instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IStartupModule"/>.</typeparam>
        /// <param name="startupModule">The <see cref="IStartupModule"/> instance.</param>
        public void AddStartupModule<T>(T startupModule) where T : IStartupModule
            => StartupModules.Add(startupModule);

        /// <summary>
        /// Adds the <see cref="IStartupModule"/> implementation of type <typeparamref name="T"/>.
        /// The type <typeparamref name="T"/> will be activated using the default, parameterless constructor.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IStartupModule"/>.</typeparam>
        public void AddStartupModule<T>() where T : IStartupModule
            => AddStartupModule(typeof(T));

        /// <summary>
        /// Adds a <see cref="IStartupModule"/> by the specified type.
        /// The type will be activated using the default, parameterless constructor.
        /// </summary>
        /// <param name="type">The type of the <see cref="IStartupModule"/>.</param>
        public void AddStartupModule(Type type)
        {
            if (typeof(IStartupModule).IsAssignableFrom(type))
            {
                var instance = Activate(type);
                StartupModules.Add(instance);
            }
            else
            {
                throw new ArgumentException(
                    $"Specified startup module '{type.Name}' does not implement {nameof(IStartupModule)}.",
                    nameof(type));
            }
        }

        /// <summary>
        /// Adds an inline middleware configuration to the application.
        /// </summary>
        /// <param name="action">A callback to configure the middleware pipeline.</param>
        public void ConfigureMiddleware(Action<IApplicationBuilder, ConfigureMiddlewareContext> action) =>
            StartupModules.Add(new InlineMiddlewareConfiguration(action));

        private IStartupModule Activate(Type type)
        {
            try
            {
                return (IStartupModule)Activator.CreateInstance(type)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create instance for {nameof(IStartupModule)} type '{type.Name}'.", ex);
            }
        }
    }
}
