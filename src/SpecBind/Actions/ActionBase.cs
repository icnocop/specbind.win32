using SpecBind.ActionPipeline;

namespace SpecBind.Actions
{
    public abstract class ActionBase : IAction
    {
        /// <summary>
        /// Gets or sets the control locator.
        /// </summary>
        /// <value>The control locator.</value>
        public IControlLocator ControlLocator { protected get; set; }

        public IWindowLocator WindowLocator { protected get; set; }

        /// <summary>
        /// Executes this instance action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The result of the action.</returns>
        public abstract ActionResult Execute(ActionContext actionContext);
    }
}