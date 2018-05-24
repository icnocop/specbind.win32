using SpecBind.ActionPipeline;
using SpecBind.Actions;
using SpecBind.Application;
using SpecBind.Context;
using TechTalk.SpecFlow;
using SpecBind.Window;
using System;

namespace SpecBind.Steps
{
    [Binding]
    public class WindowSteps : StepBase
    {
        private readonly IActionPipelineService actionPipelineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSteps" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="actionPipelineService">The action pipeline service.</param>
        /// <param name="tokenManager">The token manager.</param>
        public WindowSteps(IContext context, IActionPipelineService actionPipelineService)
            : base(context)
        {
            this.actionPipelineService = actionPipelineService;
        }

        /// <summary>
        /// Given the x window was displayed.
        /// </summary>
        /// <param name="propertyName">Name of the property that represents the window.</param>
        [Given(@"the (.+) window was displayed")]
        [Then(@"the (.+) window is displayed")]
        public void TheWindowWasDisplayed(string propertyName)
        {
            IApplication application = this.GetApplicationFromContext();
            IWindow window = this.GetWindowFromContext(true);

            var context = new WindowLocatorAction.WindowLocatorActionContext(application, window, propertyName);
            var childWindow = this.actionPipelineService.PerformAction<WindowLocatorAction>(application, window, context)
                .CheckResult<IWindow>();

            this.UpdateWindowContext(childWindow);
        }

        [Given(@"the (.+) window was closed")]
        [Then(@"the (.+) window will close")]
        public void TheWindowWasClosed(string propertyName)
        {
            IWindow window = this.GetWindowFromContext();

            // TODO: verify window was closed

            this.UpdateWindowContext(window.ParentWindow);
        }

        /// <summary>
        /// A step that waits for a window to become visible.
        /// </summary>
        /// <param name="propertyName">Name of the window.</param>
        [Given(@"I waited for the (.+) window to be displayed")]
        [When(@"I wait for the (.+) window to display")]
        [Then(@"the (.+) window will be displayed")]
        public void WaitForWindowToBeDisplayed(string propertyName)
        {
            this.WaitForWindow(propertyName, WaitConditions.Exists);
        }

        [Given(@"I waited for the (.+) window to be closed")]
        [Given(@"I waited for the (.+) window to close")]
        [When(@"I wait for the (.+) window to close")]
        public void WaitForTheWindowToClose(string propertyName)
        {
            this.WaitForWindow(propertyName, WaitConditions.NotExists);
        }

        private void WaitForWindow(string propertyName, WaitConditions waitCondition)
        {
            IApplication application = this.GetApplicationFromContext();
            IWindow currentWindow = this.GetWindowFromContext(true);

            IWindow parentWindow;
            switch (waitCondition)
            {
                case WaitConditions.Exists:
                    // wait for a child window
                    parentWindow = currentWindow;
                    break;
                case WaitConditions.NotExists:
                    // wait for the current window
                    parentWindow = currentWindow.ParentWindow;
                    break;
                default:
                    throw new NotImplementedException($"Wait condition: {Enum.GetName(typeof(WaitConditions), waitCondition)}");
            }

            IWindow window = null;
            try
            {
                var context = new WaitForWindowAction.WaitForWindowActionContext(parentWindow ?? application, propertyName, waitCondition, null);
                window = this.actionPipelineService.PerformAction<WaitForWindowAction>(application, parentWindow ?? application, context)
                    .CheckResult<IWindow>();
            }
            finally
            {
                this.UpdateWindowContext(window ?? currentWindow.ParentWindow ?? application);
            }
        }
    }
}
