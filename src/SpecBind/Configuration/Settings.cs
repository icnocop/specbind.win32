using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SpecBind.Configuration
{
    public class Settings : ISettings
    {
        private const string DefaultProviderType = "SpecBind.CodedUI.CodedUIApplicationFactory, SpecBind.CodedUI";

        public Settings()
        {
            this.ExcludedAssemblies = new List<ExcludeAssembly>();
        }

        public static Settings FromConfiguration()
        {
            SpecBindConfigurationSection specBindConfigurationSection = ConfigurationManager.GetSection("specBind") as SpecBindConfigurationSection;
            if (specBindConfigurationSection == null)
            {
                return new Settings
                {
                    ProviderType = DefaultProviderType
                };
            }

            List<ExcludeAssembly> excludeAssemblies = specBindConfigurationSection
                    .ExcludedAssemblies
                    .Cast<ExcludeAssemblyElement>()
                    .Select(x => new ExcludeAssembly
                    {
                        Name = x.Name
                    })
                    .ToList();

            ApplicationFactoryConfigurationElement factory = specBindConfigurationSection.ApplicationFactory;
            string providerType = factory?.Provider ?? DefaultProviderType;

            bool highlightModeEnabled = factory?.Highlight ?? false;

            return new Settings
            {
                ProviderType = providerType,
                ExcludedAssemblies = excludeAssemblies,
                HighlightModeEnabled = highlightModeEnabled
            };

        }

        public string ProviderType { get; set; }

        public List<ExcludeAssembly> ExcludedAssemblies { get; set; }

        public bool HighlightModeEnabled { get; set; }
    }
}
