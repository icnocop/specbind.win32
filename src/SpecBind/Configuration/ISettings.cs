using System.Collections.Generic;

namespace SpecBind.Configuration
{
    public interface ISettings
    {
        string ProviderType { get; set; }

        List<ExcludeAssembly> ExcludedAssemblies { get; set; }

        bool HighlightModeEnabled { get; set; }
    }
}