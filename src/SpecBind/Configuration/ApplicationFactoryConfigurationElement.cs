using System.Configuration;

namespace SpecBind.Configuration
{
    public class ApplicationFactoryConfigurationElement : ConfigurationElement
    {
        private const string ProviderElementName = "provider";
        private const string HighlightAttributeName = "highlight";

        /// <summary>
        /// Gets or sets the provider for the element.
        /// </summary>
        /// <value>The provider.</value>
        [ConfigurationProperty(ProviderElementName, DefaultValue = null, IsRequired = true)]
        public string Provider
        {
            get
            {
                return (string)this[ProviderElementName];
            }

            set
            {
                this[ProviderElementName] = value;
            }
        }

        [ConfigurationProperty(HighlightAttributeName, DefaultValue = false, IsRequired = false)]
        public bool Highlight
        {
            get
            {
                return (bool)this[HighlightAttributeName];
            }

            set
            {
                this[HighlightAttributeName] = value;
            }
        }
    }
}