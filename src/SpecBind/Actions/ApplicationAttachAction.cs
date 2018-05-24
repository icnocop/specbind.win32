using SpecBind.ActionPipeline;
using SpecBind.Application;
using SpecBind.Logging;

namespace SpecBind.Actions
{
    public class ApplicationAttachAction : ContextActionBase<ApplicationAttachAction.ApplicationAttachActionContext>
    {
        private readonly ILogger logger;
        private readonly IApplicationMapper applicationMapper;
        private readonly ApplicationFactory applicationFactory;

        public ApplicationAttachAction(
            ILogger logger,
            IApplicationMapper applicationMapper,
            ApplicationFactory applicationFactory)
        {
            this.logger = logger;
            this.applicationMapper = applicationMapper;
            this.applicationFactory = applicationFactory;
        }

        protected override ActionResult Execute(ApplicationAttachActionContext context)
        {
            var propertyName = context.PropertyName;
            var type = this.applicationMapper.GetTypeFromName(propertyName);

            if (type == null)
            {
                return ActionResult.Failure(new ApplicationExecutionException(
                    "Cannot locate an application launcher for name: {0}. Check application aliases in the test assembly.", propertyName));
            }
            
            this.logger.Debug("Attaching to application: {0} ({1})", propertyName, type.FullName);

            IApplication application = this.applicationFactory.AttachApplication(this.logger, type);

            return ActionResult.Successful(application);
        }

        public class ApplicationAttachActionContext : ActionContext
        {
            public ApplicationAttachActionContext(string propertyName)
                : base(propertyName)
            {
            }
        }
    }
}