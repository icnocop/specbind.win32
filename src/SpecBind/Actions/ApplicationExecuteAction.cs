using System;
using SpecBind.ActionPipeline;
using SpecBind.Application;
using SpecBind.Logging;

namespace SpecBind.Actions
{
    public class ApplicationExecuteAction : ContextActionBase<ApplicationExecuteAction.ApplicationExecuteActionContext>
    {
        private readonly ILogger logger;
        private readonly IApplicationMapper applicationMapper;
        private readonly ApplicationFactory applicationFactory;

        public ApplicationExecuteAction(
            ILogger logger,
            IApplicationMapper applicationMapper,
            ApplicationFactory applicationFactory)
        {
            this.logger = logger;
            this.applicationMapper = applicationMapper;
            this.applicationFactory = applicationFactory;
        }

        protected override ActionResult Execute(ApplicationExecuteActionContext context)
        {
            var propertyName = context.PropertyName;
            var type = this.applicationMapper.GetTypeFromName(propertyName);

            if (type == null)
            {
                return ActionResult.Failure(new ApplicationExecutionException(
                    "Cannot locate an application launcher for name: {0}. Check application aliases in the test assembly.", propertyName));
            }

            this.logger.Debug("Launching application: {0} ({1})", propertyName, type.FullName);

            IApplication application = this.applicationFactory.LaunchApplication(this.logger, type);

            return ActionResult.Successful(application);
        }

        public class ApplicationExecuteActionContext : ActionContext
        {
            public ApplicationExecuteActionContext(string propertyName)
                : base(propertyName)
            {
            }
        }
    }
}