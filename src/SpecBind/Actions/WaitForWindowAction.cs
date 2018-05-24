using System;
using SpecBind.ActionPipeline;
using SpecBind.Window;
using SpecBind.Helpers;
using SpecBind.Logging;
using SpecBind.PropertyHandlers;
using SpecBind.CodedUI;

namespace SpecBind.Actions
{
    public class WaitForWindowAction : ContextActionBase<WaitForWindowAction.WaitForWindowActionContext>
    {
        private readonly ILogger logger;

        public WaitForWindowAction()
        {
            DefaultTimeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitForWindowAction" /> class.
        /// </summary>
        /// <param name="windowMapper">The window mapper.</param>
        public WaitForWindowAction(ILogger logger)
            : this()
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the default timeout to wait, if none is specified.
        /// </summary>
        /// <value>
        /// The default timeout, 30 seconds.
        /// </value>
        public static TimeSpan DefaultTimeout { get; set; }

        protected override ActionResult Execute(WaitForWindowActionContext actionContext)
        {
            var window = actionContext.Window;
            string propertyName = actionContext.PropertyName;

            var timeout = actionContext.Timeout.GetValueOrDefault(WaitForWindowAction.DefaultTimeout);
            var waitInterval = TimeSpan.FromMilliseconds(200);
            var waiter = new Waiter(timeout, waitInterval);

            try
            {
                IWindow childWindow = null;
                IPropertyData propertyData = null;

                waiter.WaitFor(() =>
                {
                    try
                    {
                        bool result = this.WindowLocator.TryGetWindow(propertyName, out propertyData);

                        if (result)
                        {
                            childWindow = this.CheckForWindow(window, propertyData.PropertyType);
                        }

                        switch (actionContext.WaitCondition)
                        {
                            case WaitConditions.Exists:
                                return childWindow != null;
                            case WaitConditions.NotExists:
                                return childWindow == null;
                            default:
                                throw new NotImplementedException($"Wait condition: {Enum.GetName(typeof(WaitConditions), actionContext.WaitCondition)}");
                        }
                    }
                    catch (ControlNotAvailableException)
                    {
                        if (actionContext.WaitCondition == WaitConditions.NotExists)
                        {
                            return true;
                        }

                        return false;
                    }
                });

                return ActionResult.Successful(childWindow);
            }
            catch (TimeoutException)
            {
                if (actionContext.WaitCondition == WaitConditions.NotExists)
                {
                    return ActionResult.Successful(null);
                }

                var exception = new Exception($"Application did not resolve to the '{propertyName}' window in {timeout}");
                return ActionResult.Failure(exception);
            }
        }

        /// <summary>
        /// Checks for window.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        /// <returns>The window object once the item is located.</returns>
        private IWindow CheckForWindow(IWindow window, Type windowType)
        {
            IWindow childWindow;

            while (true)
            {
                try
                {
                    window.FindWindow(windowType, out childWindow);
                    return childWindow;
                }
                catch (Exception ex)
                {
                    this.logger.Debug("Window is not visible. Details: {0}", ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// An action context for the action.
        /// </summary>
        public class WaitForWindowActionContext : ActionContext
        {
            public WaitForWindowActionContext(IWindow window, string propertyName, WaitConditions waitCondition, TimeSpan? timeout)
                : base(propertyName)
            {
                this.Window = window;
                this.WaitCondition = waitCondition;
                this.Timeout = timeout;
            }

            public IWindow Window { get; private set; }

            /// <summary>
            /// Gets the timeout.
            /// </summary>
            /// <value>The timeout.</value>
            public TimeSpan? Timeout { get; private set; }

            public WaitConditions WaitCondition { get; private set; }
        }
    }
}