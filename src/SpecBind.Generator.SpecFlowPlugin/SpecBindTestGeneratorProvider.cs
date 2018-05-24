using System.CodeDom;
using System.Linq;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Utils;

namespace SpecBind.Generator.SpecFlowPlugin
{
    public class SpecBindTestGeneratorProvider : MsTest2010GeneratorProvider
    {
        private const string TestClassAttribute = @"Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute";
        private const string CodedUiTestClassAttribute = @"Microsoft.VisualStudio.TestTools.UITesting.CodedUITestAttribute";
        private const string DeploymentItemAttribute = "Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute";

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecBindTestGeneratorProvider" /> class.
        /// </summary>
        /// <param name="codeDomHelper">The code DOM helper.</param>
        public SpecBindTestGeneratorProvider(CodeDomHelper codeDomHelper)
            : base(codeDomHelper)
        {
        }

        /// <summary>
        /// Sets the test class.
        /// </summary>
        /// <param name="generationContext">The generation context.</param>
        /// <param name="featureTitle">The feature title.</param>
        /// <param name="featureDescription">The feature description.</param>
        public override void SetTestClass(TestClassGenerationContext generationContext, string featureTitle, string featureDescription)
        {
            base.SetTestClass(generationContext, featureTitle, featureDescription);

            foreach (var customAttribute in generationContext.TestClass
                .CustomAttributes
                .Cast<CodeAttributeDeclaration>()
                .Where(customAttribute => string.Equals(customAttribute.Name, TestClassAttribute)))
            {
                generationContext.TestClass.CustomAttributes.Remove(customAttribute);
                break;
            }

            generationContext.TestClass.CustomAttributes.Add(
                new CodeAttributeDeclaration(new CodeTypeReference(CodedUiTestClassAttribute)));
        }
    }
}
