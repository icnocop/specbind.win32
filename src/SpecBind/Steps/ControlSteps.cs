using SpecBind.ActionPipeline;
using SpecBind.Actions;
using SpecBind.Context;
using SpecBind.Extensions;
using TechTalk.SpecFlow;

namespace SpecBind.Steps
{
    [Binding]
    public class ControlSteps : StepBase
    {
        private readonly IActionPipelineService actionPipelineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageNavigationSteps" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="actionPipelineService">The action pipeline service.</param>
        /// <param name="tokenManager">The token manager.</param>
        public ControlSteps(IContext context, IActionPipelineService actionPipelineService)
            : base(context)
        {
            this.actionPipelineService = actionPipelineService;
        }

        /// <summary>
        /// Given I clicked control.
        /// When I click control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        [Given("I clicked (.*)")]
        [When("I click (.*)")]
        public void IClickedOnControl(string controlName)
        {
            var application = this.GetApplicationFromContext();
            var window = this.GetWindowFromContext();

            var context = new ActionContext(controlName.ToLookupKey());

            this.actionPipelineService
                    .PerformAction<ControlClickAction>(application, window, context)
                    .CheckResult();
        }
    }
}
