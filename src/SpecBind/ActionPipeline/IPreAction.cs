using SpecBind.Actions;

namespace SpecBind.ActionPipeline
{
    public interface IPreAction
    {
        /// <summary>
        /// Performs the pre-execute action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The action context.</param>
	    void PerformPreAction(IAction action, ActionContext context);
    }
}