using SpecBind.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecBind.Mapper
{
    public abstract class MapperBase : IMapper
    {
        private readonly string classNameSuffix;
        private readonly Dictionary<string, Type> typeCache;

        /// <summary>
        /// Initializes the <see cref="MapperBase" /> class.
        /// </summary>
        public MapperBase(string classNameSuffix)
        {
            this.classNameSuffix = classNameSuffix;
            this.typeCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the type from the given name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The resolved type; otherwise <c>null</c>.</returns>
        public Type GetTypeFromName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            Type type;
            return this.typeCache.TryGetValue(typeName.ToLookupKey(), out type) ? type : null;
        }

        /// <summary>
        /// Maps the loaded assemblies into the type mapper.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        public void Initialize(Type baseType)
        {
            // There are several blank catches to avoid loading bad asssemblies.
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !a.GlobalAssemblyCache))
                {
                    try
                    {
                        // Map all public types.
                        this.MapAssemblyTypes(assembly.GetExportedTypes(), baseType);
                    }
                    catch (SystemException)
                    {
                    }
                }
            }
            catch (SystemException)
            {
            }
        }

        /// <summary>
        /// Maps the assembly types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="baseType">The base type.</param>
        internal void MapAssemblyTypes(IEnumerable<Type> types, Type baseType)
        {
            foreach (var applicationType in types.Where(t => t.IsClass && !t.IsAbstract
                && (baseType == null || (t.BaseType != null && ((baseType.IsGenericType && IsAssignableToGenericType(t.BaseType, baseType)) || (baseType.IsAssignableFrom(t.BaseType)))))))
            {
                var initialName = applicationType.Name;
                if (initialName.EndsWith(this.classNameSuffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    initialName = initialName.Substring(0, initialName.Length - this.classNameSuffix.Length);
                }

                if (!this.typeCache.ContainsKey(initialName))
                {
                    this.typeCache.Add(initialName, applicationType);
                }
            }
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
