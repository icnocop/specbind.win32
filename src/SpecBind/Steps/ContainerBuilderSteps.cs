using BoDi;
using SpecBind.ActionPipeline;
using SpecBind.Application;
using SpecBind.Configuration;
using SpecBind.Context;
using SpecBind.Helpers;
using SpecBind.Logging;
using SpecBind.Window;
using TechTalk.SpecFlow;

namespace SpecBind.Steps
{
    [Binding]
    public class ContainerBuilderSteps
    {
        private readonly IObjectContainer objectContainer;

        public ContainerBuilderSteps(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.objectContainer.RegisterTypeAs<SpecFlowLogger, ILogger>();
            var logger = this.objectContainer.Resolve<ILogger>();

            var settings = Settings.FromConfiguration();
            this.objectContainer.RegisterInstanceAs<ISettings>(settings);

            ApplicationFactory applicationFactory
                = ApplicationFactory.Get(logger, settings.ProviderType);

            this.objectContainer.RegisterInstanceAs(applicationFactory);

            var applicationMapper = new ApplicationMapper();
            applicationMapper.Initialize(applicationFactory.BaseType);
            this.objectContainer.RegisterInstanceAs<IApplicationMapper>(applicationMapper);

            var windowMapper = new WindowMapper();
            windowMapper.Initialize(applicationFactory.BaseWindowType);
            this.objectContainer.RegisterInstanceAs<IWindowMapper>(windowMapper);

            this.objectContainer.RegisterTypeAs<Context.Context, IContext>();
            this.objectContainer.RegisterTypeAs<TokenManager, ITokenManager>();

            var repository = new ActionRepository(this.objectContainer, settings);
            this.objectContainer.RegisterInstanceAs<IActionRepository>(repository);

            this.objectContainer.RegisterTypeAs<ActionPipelineService, IActionPipelineService>();

            // Initialize the repository
            repository.Initialize();
        }
    }
}
