using SpecBind.Application;
using SpecBind.Context;
using SpecBind.Extensions;
using SpecBind.Control;
using SpecBind.Window;

namespace SpecBind.Steps
{
    public class StepBase
    {
        /// <summary>
        /// The scenario context key for holding the current window.
        /// </summary>
        public const string CurrentWindowKey = "CurrentWindow";

        public const string CurrentApplicationKey = "CurrentApplication";

        private readonly IContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected StepBase(IContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the application from the context.
        /// </summary>
        /// <returns>The current application.</returns>
        protected IApplication GetApplicationFromContext()
        {
            var application = this.context.GetValue<IApplication>(CurrentApplicationKey);
            if (application == null)
            {
                throw new ApplicationExecutionException("No application has been set as being the current application.");
            }

            return application;
        }

        /// <summary>
        /// Gets the window from the context.
        /// </summary>
        /// <returns>The current window.</returns>
        protected IWindow GetWindowFromContext(bool allowNotSet = false)
        {
            var window = this.context.GetValue<IWindow>(CurrentWindowKey);
            if ((window == null) && (!allowNotSet))
            {
                throw new ApplicationExecutionException("No window has been set as being the current window.");
            }

            return window;
        }

        /// <summary>
        /// Updates the application context with the given <paramref name="application" />.
        /// </summary>
        /// <param name="application">The application.</param>
        protected void UpdateApplicationContext(IApplication application)
        {
            this.context.SetValue(application, CurrentApplicationKey);
            this.UpdateWindowContext(null);
        }

        /// <summary>
        /// Updates the window context with the given <paramref name="window" />.
        /// </summary>
        /// <param name="window">The window.</param>
        protected void UpdateWindowContext(IWindow window)
        {
            this.context.SetValue(window, CurrentWindowKey);
        }
    }
}