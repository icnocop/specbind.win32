using System;
using SpecBind.Actions;
using System.Collections.Generic;
using System.Linq;
using SpecBind.Control;
using SpecBind.Window;
using SpecBind.Application;

namespace SpecBind.ActionPipeline
{
    internal class ActionPipelineService : IActionPipelineService
    {
        private readonly IActionRepository actionRepository;

        public ActionPipelineService(IActionRepository actionRepository)
        {
            this.actionRepository = actionRepository;
        }

        public ActionResult PerformAction<TAction>(
            IApplication application,
            IWindow window,
            ActionContext context)
            where TAction : IAction
        {
            var action = this.actionRepository.CreateAction<TAction>();
            return this.PerformAction(application, window, action, context);
        }

        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="window">The window.</param>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <returns>The result of the action</returns>
        public ActionResult PerformAction(IApplication application, IWindow window, IAction action, ActionContext context)
        {
            var windowLocater = this.CreateWindowLocator(window ?? application);
            action.WindowLocator = windowLocater;

            var controlLocater = this.CreateControlLocater(window);
            action.ControlLocator = controlLocater;

            var result = this.PerformPreAction(action, context);

            if (result != null)
            {
                return result;
            }

            try
            {
                result = action.Execute(context);
            }
            catch (Exception ex)
            {
                result = ActionResult.Failure(action.GetType(), ex);
            }

            this.PerformPostAction(action, context, result);

            return result;
        }

        /// <summary>
        /// Creates the window locator.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <returns>The window locator interface.</returns>
        private IWindowLocator CreateWindowLocator(IWindow parentWindow)
        {
            var filterActions = this.actionRepository.GetLocatorActions();
            return new WindowLocator(parentWindow, filterActions);
        }

        /// <summary>
        /// Creates the control locater.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>The control locater interface.</returns>
        private IControlLocator CreateControlLocater(IWindow window)
        {
            var filterActions = this.actionRepository.GetLocatorActions();
            return new ControlLocater(window, filterActions);
        }

        /// <summary>
        /// Performs any actions ahead of the actual action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The action context.</param>
        /// <returns>If successful <c>null</c>, otherwise an ActionResult with the error.</returns>
        private ActionResult PerformPreAction(IAction action, ActionContext context)
        {
            var exceptions = new List<Exception>();

            foreach (var preAction in this.actionRepository.GetPreActions())
            {
                try
                {
                    preAction.PerformPreAction(action, context);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            switch (exceptions.Count)
            {
                case 0:
                    return null;
                case 1:
                    return ActionResult.Failure(exceptions.First());
                default:
                    return ActionResult.Failure(new AggregateException(exceptions));
            }
        }

        /// <summary>
        /// Performs any actions after the actual action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The action context.</param>
        /// <param name="result">The result.</param>
        private void PerformPostAction(IAction action, ActionContext context, ActionResult result)
        {
            foreach (var postAction in this.actionRepository.GetPostActions())
            {
                postAction.PerformPostAction(action, context, result);
            }
        }
    }
}