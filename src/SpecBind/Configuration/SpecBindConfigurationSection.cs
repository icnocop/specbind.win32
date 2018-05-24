using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBind.Configuration
{
    public class SpecBindConfigurationSection : ConfigurationSection
    {
        private const string ExcludedAssembliesElement = @"excludedAssemblies";
        private const string ApplicationFactoryElementName = @"applicationFactory";

        /// <summary>
        /// Gets or sets the application factory configuration element.
        /// </summary>
        /// <value>The application factory configuration element.</value>
        [ConfigurationProperty(ApplicationFactoryElementName, DefaultValue = null, IsRequired = false)]
        public ApplicationFactoryConfigurationElement ApplicationFactory
        {
            get
            {
                return (ApplicationFactoryConfigurationElement)this[ApplicationFactoryElementName];
            }

            set
            {
                this[ApplicationFactoryElementName] = value;
            }
        }

        /// <summary>
        /// Gets the list of excluded assemblies, if any are supplied.
        /// </summary>
        /// <value>The list of excluded assemblies.</value>
        [ConfigurationProperty(ExcludedAssembliesElement, IsRequired = false)]
        [ConfigurationCollection(typeof(ExcludeAssemblyElement))]
        public ExcludeAssemblyCollection ExcludedAssemblies
        {
            get { return (ExcludeAssemblyCollection)this[ExcludedAssembliesElement]; }
        }
    }
}
