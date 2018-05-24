using System.Collections.Generic;

namespace SpecBind.ActionPipeline
{
    public interface IActionRepository
    {
        /// <summary>
        /// Creates the action.
        /// </summary>
        /// <typeparam name="TAction">The type of the action.</typeparam>
        /// <returns>The created action object.</returns>
        TAction CreateAction<TAction>();

        /// <summary>
        /// Gets the post-execute actions.
        /// </summary>
        /// <returns>An enumerable collection of actions.</returns>
        IEnumerable<IPostAction> GetPostActions();

        /// <summary>
        /// Gets the pre-execute actions.
        /// </summary>
        /// <returns>An enumerable collection of actions.</returns>
        IEnumerable<IPreAction> GetPreActions();

        /// <summary>
        /// Gets the locator actions.
        /// </summary>
        /// <returns>An enumerable collection of actions.</returns>
        IEnumerable<ILocatorAction> GetLocatorActions();
    }
}