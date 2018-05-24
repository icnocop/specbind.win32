using SpecBind.Actions;

namespace SpecBind.ActionPipeline
{
    public interface IPostAction
    {
        /// <summary>
        /// Performs the post-execute action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The action context.</param>
        /// <param name="result">The result.</param>
	    void PerformPostAction(IAction action, ActionContext context, ActionResult result);
    }
}