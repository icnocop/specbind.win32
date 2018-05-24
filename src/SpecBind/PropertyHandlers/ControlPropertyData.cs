namespace SpecBind.PropertyHandlers
{
    using System;

    using SpecBind.Actions;
    using SpecBind.Control;
    using SpecBind.Validation;
    using Window;

    /// <summary>
    /// The property data for a given property.
    /// </summary>
    /// <typeparam name="TControl">The propertyValue of the control.</typeparam>
    internal class ControlPropertyData<TControl> : PropertyDataBase<TControl>
    {
        private readonly Func<IWindow, Func<TControl, bool>, bool> controlAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDataBase{TControl}" /> class.
        /// </summary>
        /// <param name="controlHandler">The control handler.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="controlAction">The control action.</param>
        public ControlPropertyData(IWindowControlHandler<TControl> controlHandler, string name, Type propertyType, Func<IWindow, Func<TControl, bool>, bool> controlAction)
            : base(controlHandler, name, propertyType)
        {
            this.controlAction = controlAction;
            this.IsControl = true;
        }

        /// <summary>
        /// Clicks the control that this property represents.
        /// </summary>
        public override void ClickControl()
        {
            this.ThrowIfControlDoesNotExist();
            var success = this.controlAction(this.ControlHandler, this.ControlHandler.ClickControl);

            if (!success)
            {
                throw new ControlExecuteException("Click Action for property '{0}' failed!", this.Name);
            }
        }

        /// <summary>
        /// Checks to see if the control is enabled.
        /// </summary>
        /// <returns><c>true</c> if the control is enabled.</returns>
        public override bool CheckControlEnabled()
        {
            return this.controlAction(this.ControlHandler, this.ControlHandler.ControlEnabledCheck);
        }

        /// <summary>
        /// Checks to see if the control exists.
        /// </summary>
        /// <returns><c>true</c> if the control exists.</returns>
        public override bool CheckControlExists()
        {
            return this.controlAction(this.ControlHandler, this.ControlHandler.ControlExistsCheck);
        }

        /// <summary>
        /// Clears the data for the control that this property represents.
        /// </summary>
        public override void ClearData()
        {
            this.ThrowIfControlDoesNotExist();

            var clearMethod = this.ControlHandler.GetClearMethod(this.PropertyType);
            if (clearMethod == null)
            {
                throw new ControlExecuteException(
                    "Cannot find input handler for property '{0}' on window {1}. Control propertyValue was: {2}",
                    this.Name,
                    this.ControlHandler.WindowType.Name,
                    this.PropertyType.Name);
            }

            this.controlAction(
                this.ControlHandler,
                e =>
                    {
                        clearMethod(e);
                        return true;
                    });
        }

        /// <summary>
        /// Checks to see if the control does not exist.
        /// Unlike ControlExistsCheck() this, doesn't let the web driver wait first for the control to exist.
        /// </summary>
        /// <returns><c>true</c> if the control exists.</returns>
        public override bool CheckControlNotExists()
        {
            return this.controlAction(this.ControlHandler, this.ControlHandler.ControlNotExistsCheck);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void FillData(string data)
        {
            this.ThrowIfControlDoesNotExist();

            var fillMethod = this.ControlHandler.GetWindowFillMethod(this.PropertyType);
            if (fillMethod == null)
            {
                throw new ControlExecuteException(
                    "Cannot find input handler for property '{0}' on window {1}. Control propertyValue was: {2}",
                    this.Name,
                    this.ControlHandler.WindowType.Name,
                    this.PropertyType.Name);
            }

            this.controlAction(
                this.ControlHandler,
                e =>
                    {
                        fillMethod(e, data);
                        return true;
                    });
        }

        /// <summary>
        /// Gets the control text.
        /// </summary>
        /// <param name="validation">The validation.</param>
        /// <param name="control">The control.</param>
        /// <returns>The cleaned text from the control.</returns>
        protected string GetControlText(ItemValidation validation, TControl control)
        {
            if (!validation.RequiresFieldValue)
            {
                return null;
            }

            var text = this.ControlHandler.GetControlText(control);

            // Trim whitespace from text since the tables in SpecFlow will anyway.
            if (text != null)
            {
                text = text.Trim();
                text = text.Replace(Environment.NewLine, " ");
            }

            return text;
        }

        /// <summary>
        /// Gets the current value of the property.
        /// </summary>
        /// <returns>The current value as a string.</returns>
        public override string GetCurrentValue()
        {
            string fieldValue = null;

            this.ThrowIfControlDoesNotExist();
            this.controlAction(this.ControlHandler,
                prop =>
                {
                    fieldValue = this.ControlHandler.GetControlText(prop);
                    return true;
                });

            return fieldValue;
        }

        /// <summary>
        /// Gets the item as window.
        /// </summary>
        /// <returns>
        /// The item as a window.
        /// </returns>
        public override IWindow GetItemAsWindow()
        {
            var item = default(TControl);
            var findItem = new Func<TControl, bool>(
                prop =>
                    {
                        item = prop;
                        return true;
                    });

            this.controlAction(this.ControlHandler, findItem);
            return this.ControlHandler.GetWindowFromControl(item);
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public override void Highlight()
        {
            this.controlAction(
                this.ControlHandler,
                e =>
                    {
                        this.ControlHandler.Highlight(e);
                        return true;
                    });
        }

        /// <summary>
        /// Validates the item or property matches the expected expression.
        /// </summary>
        /// <param name="validation">The validation item.</param>
        /// <param name="actualValue">The actual value if validation fails.</param>
        /// <returns>
        ///   <c>true</c> if the items are valid; otherwise <c>false</c>.
        /// </returns>
        public override bool ValidateItem(ItemValidation validation, out string actualValue)
        {
            string realValue = null;
            var compareWrapper = new Func<TControl, bool>(
                e =>
                    {
                        var text = this.GetControlText(validation, e);

                        realValue = text;
                        return validation.Compare(this, text);
                    });

            if (validation.CheckControlExistence)
            {
                this.ThrowIfControlDoesNotExist();
            }

            var result = this.controlAction(this.ControlHandler, compareWrapper);

            actualValue = realValue;
            return result;
        }

        /// <summary>
        /// Waits for the control condition to be met.
        /// </summary>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The timeout to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        public override bool WaitForControlCondition(WaitConditions waitCondition, TimeSpan? timeout)
        {
            return this.controlAction(this.ControlHandler, o => this.ControlHandler.WaitForControl(o, waitCondition, timeout));
        }

        /// <summary>
        /// Check to make sure the control exists on the window.
        /// </summary>
        /// <exception cref="ControlExecuteException">Thrown if the control does not exist.</exception>
        private void ThrowIfControlDoesNotExist()
        {
            if (!this.CheckControlExists())
            {
                throw new ControlExecuteException(
                    "Control mapped to property '{0}' does not exist on window {1}.",
                    this.Name,
                    this.ControlHandler.WindowType.Name);
            }
        }
    }
}