using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;

namespace SpecBind.Generator.SpecFlowPlugin
{
    public class SpecBindGeneratorPlugin : IGeneratorPlugin
    {
        /// <summary>
        /// Initializes the plugin to change the behavior of the generator
        /// </summary>
        /// <param name="generatorPluginEvents">The generator plugin events.</param>
        /// <param name="generatorPluginParameters">Parameters to the generator.</param>
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            generatorPluginEvents.CustomizeDependencies += CustomizeDependencies;
        }

        private static void CustomizeDependencies(object sender, CustomizeDependenciesEventArgs eventArgs)
        {
            var container = eventArgs.ObjectContainer;

            var unitTestGenProvider = eventArgs.SpecFlowProjectConfiguration.SpecFlowConfiguration.UnitTestProvider;
            if (string.Equals(unitTestGenProvider, "mstest", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(unitTestGenProvider, "mstest.2010", StringComparison.OrdinalIgnoreCase))
            {
                container.RegisterTypeAs<SpecBindTestGeneratorProvider, IUnitTestGeneratorProvider>();
            }
        }
    }
}
