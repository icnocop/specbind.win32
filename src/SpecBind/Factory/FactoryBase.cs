using SpecBind.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpecBind.Factory
{
    public abstract class FactoryBase<T> where T : IFactory
    {
        public Type BaseType { get; set; }
        public ILogger Logger { get; set; }

        internal static T Get(ILogger logger, string typeName)
        {
            var type = Type.GetType(typeName, OnAssemblyResolve, OnTypeResolve);
            if (type == null || !typeof(T).IsAssignableFrom(type))
            {
                string message = string.Join(" ", new[]
                {
                     string.Format("Could not load type: {0}.", typeName),
                    "Make sure this is fully qualified and the assembly exists.",
                    string.Format("Also ensure the base type is {0}.", nameof(T))
                });
                throw new InvalidOperationException(message);
            }

            var factory = (T)Activator.CreateInstance(type);
            factory.Logger = logger;

            return factory;
        }

        /// <summary>
        /// Called when an assembly load failure occurs, this will try to load it from the same directory as the main assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The resolved assembly.</returns>
        private static Assembly OnAssemblyResolve(AssemblyName assemblyName)
        {
            try
            {
                // try load assembly from app domain first rather than filesystem as test runners
                // can place ddls in separate directories and may not always work as below.
                var assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    return assembly;
                }
            }

            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Ignore and resume as previous.
            }

            var currentLocation = Path.GetFullPath(typeof(T).Assembly.Location);
            if (!string.IsNullOrWhiteSpace(currentLocation) && File.Exists(currentLocation))
            {
                var parentDirectory = Path.GetDirectoryName(currentLocation);
                if (!string.IsNullOrWhiteSpace(parentDirectory) && Directory.Exists(parentDirectory))
                {
                    var file = string.Format("{0}.dll", assemblyName.Name);
                    var assemblyPath = Directory.EnumerateFiles(parentDirectory, file, SearchOption.AllDirectories).FirstOrDefault();
                    if (assemblyPath != null)
                    {
                        return Assembly.LoadFile(assemblyPath);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Called when The type should be resolved.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typeName">The type name.</param>
        /// <param name="ignoreCase">if set to <c>true</c> ignore the case.</param>
        /// <returns>The resolved type.</returns>
        private static Type OnTypeResolve(Assembly assembly, string typeName, bool ignoreCase)
        {
            return assembly.GetType(typeName, false, ignoreCase);
        }
    }
}
