using System;
using SpecBind.ActionPipeline;
using SpecBind.PropertyHandlers;
using SpecBind.Helpers;

namespace SpecBind.Actions
{
    /// <summary>
    /// An action that enters data into a field.
    /// </summary>
    internal class EnterDataAction : ContextActionBase<EnterDataAction.EnterDataContext>
    {
        private readonly ITokenManager tokenManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterDataAction" /> class.
        /// </summary>
        /// <param name="tokenManager">The token manager.</param>
        public EnterDataAction(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        /// <summary>
        /// Executes the specified action using the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The result of the action.</returns>
        protected override ActionResult Execute(EnterDataContext context)
        {
            // First look for an element
            IPropertyData item;
            if (!this.ControlLocator.TryGetControl(context.PropertyName, out item))
            {
                // Try to get a property and check to make sure it's a string for now
                item = this.ControlLocator.GetControl(context.PropertyName);
            }

            var fieldValue = this.tokenManager.SetToken(context.Data);

            item.FillData(fieldValue);

            return ActionResult.Successful();
        }

        /// <summary>
        /// An extended context class to pass fill data.
        /// </summary>
        public class EnterDataContext : ActionContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnterDataContext"/> class.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="data">The data.</param>
            public EnterDataContext(string propertyName, string data)
                : base(propertyName)
            {
                this.Data = data;
            }

            /// <summary>
            /// Gets the data that should be entered.
            /// </summary>
            /// <value>The data that should be entered.</value>
            public string Data { get; private set; }
        }
    }
}