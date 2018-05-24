using SpecBind.Actions;

namespace SpecBind.ActionPipeline
{
    public interface IAction
    {
        /// <summary>
        /// Sets the control locator.
        /// </summary>
        /// <value>The control locator.</value>
        IControlLocator ControlLocator { set; }

        IWindowLocator WindowLocator { set; }

        /// <summary>
        /// Executes this instance action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The result of the action.</returns>
	    ActionResult Execute(ActionContext actionContext);
    }
}