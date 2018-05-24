using SpecBind.ActionPipeline;
using SpecBind.Application;
using SpecBind.Logging;
using SpecBind.Control;
using SpecBind.Window;
using SpecBind.PropertyHandlers;

namespace SpecBind.Actions
{
    public class WindowLocatorAction : ContextActionBase<WindowLocatorAction.WindowLocatorActionContext>
    {
        private readonly ILogger logger;
        private readonly IWindowMapper windowMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLocatorAction" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="windowMapper">The window mapper.</param>
        public WindowLocatorAction(ILogger logger, IWindowMapper windowMapper)
        {
            this.logger = logger;
            this.windowMapper = windowMapper;
        }

        protected override ActionResult Execute(WindowLocatorActionContext context)
        {
            var application = context.Application;
            var parentWindow = context.Window;
            var propertyName = context.PropertyName;

            IPropertyData propertyData = this.WindowLocator.GetWindow(propertyName);

            this.logger.Debug("Ensuring window is displayed: {0} ({1})", propertyName, propertyData.PropertyType.FullName);

            IWindow window = null;
            if (parentWindow == null)
            {
                // find window from the application
                application.FindWindow(propertyData.PropertyType, out window);
            }
            else
            {
                // find child window from the parent window
                parentWindow.FindWindow(propertyData.PropertyType, out window);
            }

            return window == null
                       ? ActionResult.Failure(new ControlExecuteException("Could not retrieve a window from property '{0}'", propertyName))
                       : ActionResult.Successful(window);
        }

        public class WindowLocatorActionContext : ActionContext
        {
            public WindowLocatorActionContext(IApplication application, IWindow window, string propertyName)
                : base(propertyName)
            {
                this.Application = application;
                this.Window = window;
            }

            public IApplication Application { get; private set; }
            public IWindow Window { get; private set; }
        }
    }
}