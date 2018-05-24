using SpecBind.Actions;
using SpecBind.Application;
using SpecBind.Control;
using SpecBind.Window;

namespace SpecBind.ActionPipeline
{
    public interface IActionPipelineService
    {
        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <typeparam name="TAction">The type of the action that inherits from <see cref="IAction" />.</typeparam>
        /// <param name="application">The application.</param>
        /// <param name="window">The window.</param>
        /// <param name="context">The context.</param>
        /// <returns>The result of the action.</returns>
        ActionResult PerformAction<TAction>(IApplication application, IWindow window, ActionContext context)
            where TAction : IAction;

        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="window">The window.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <returns>The result of the action.</returns>
        ActionResult PerformAction(IApplication application, IWindow window, IAction action, ActionContext context);
    }
}