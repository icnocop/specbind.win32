using System.Configuration;

namespace SpecBind.Configuration
{
    internal class ExcludeAssemblyElement : ConfigurationElement
    {
        private const string NameKey = @"name";

        /// <summary>
        /// Gets or sets the assembly's name.
        /// </summary>
        /// <value>The assembly's name.</value>
        [ConfigurationProperty(NameKey, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[NameKey];
            }

            set
            {
                this[NameKey] = value;
            }
        }
    }
}