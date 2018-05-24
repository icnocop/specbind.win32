namespace SpecBind.Steps
{
    using ActionPipeline;
    using Actions;
    using Application;
    using Context;
    using System;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Application Steps.
    /// </summary>
    [Binding]
    public class ApplicationSteps : StepBase
    {
        private readonly IActionPipelineService actionPipelineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSteps" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="actionPipelineService">The action pipeline service.</param>
        public ApplicationSteps(IContext context, IActionPipelineService actionPipelineService)
            : base(context)
        {
            this.actionPipelineService = actionPipelineService;
        }

        /// <summary>
        /// Given I launched the application.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        [Given(@"I launched the (.*) application")]
        public void GivenILaunchedTheApplication(string applicationName)
        {
            var context = new ApplicationExecuteAction.ApplicationExecuteActionContext(applicationName);
            var application = this.actionPipelineService.PerformAction<ApplicationExecuteAction>(null, null, context)
                .CheckResult<IApplication>();

            this.UpdateApplicationContext(application);
        }

        [Given(@"I attached to the (.*) application")]
        public void AttachToTheApplication(string applicationName)
        {
            var context = new ApplicationAttachAction.ApplicationAttachActionContext(applicationName);
            var application = this.actionPipelineService.PerformAction<ApplicationAttachAction>(null, null, context)
                .CheckResult<IApplication>();

            this.UpdateApplicationContext(application);
        }

        [Then(@"the (.+) application will exit")]
        public void ThenTheApplicationWillExit(string propertyName)
        {
            // TODO: get specific application
            IApplication application = this.GetApplicationFromContext();
            if (application.IsRunning)
            {
                throw new Exception($"Application '{propertyName}' is still running.");
            }
        }

        [Then(@"the (.*) application will exit with code (.*)")]
        public void ThenTheApplicationWillExitWithCode(string propertyName, int exitCode)
        {
            // TODO: get specific application
            IApplication application = this.GetApplicationFromContext();
            if (application.IsRunning)
            {
                throw new Exception($"Application '{propertyName}' is still running.");
            }

            if (application.ExitCode != exitCode)
            {
                throw new Exception($"Application '{propertyName}' exited with code {application.ExitCode}. Expected {exitCode}.");
            }
        }
    }
}
