using SpecBind.ActionPipeline;

namespace SpecBind.Actions
{
    public class ControlClickAction : ActionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlClickAction" /> class.
        /// </summary>
        public ControlClickAction()
        {
        }

        public override ActionResult Execute(ActionContext actionContext)
        {
            var propertyData = this.ControlLocator.GetControl(actionContext.PropertyName);

            propertyData.ClickControl();
            return ActionResult.Successful();
        }
    }
}
