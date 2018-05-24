namespace SpecBind.PropertyHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpecBind.Actions;
    using SpecBind.Control;
    using SpecBind.Validation;
    using Window;

    /// <summary>
    /// A base class to define all the properties.
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    public abstract class PropertyDataBase<TControl> : IPropertyData
    {
        private readonly IWindowControlHandler<TControl> controlHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDataBase{TControl}" /> class.
        /// </summary>
        /// <param name="controlHandler">The control handler.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        protected PropertyDataBase(IWindowControlHandler<TControl> controlHandler, string name, Type propertyType)
        {
            this.controlHandler = controlHandler;
            this.Name = name;
            this.PropertyType = propertyType;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance represents a window control.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is a window control; otherwise, <c>false</c>.
        /// </value>
        public bool IsControl { get; protected set; }

        public bool IsWindow { get; protected set; }

        public bool IsApplication { get; protected set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType { get; private set; }

        /// <summary>
        /// Gets the control handler.
        /// </summary>
        /// <value>The control handler.</value>
        protected IWindowControlHandler<TControl> ControlHandler
        {
            get
            {
                return this.controlHandler;
            }
        }

        /// <summary>
        /// Clears the data for the control that this property represents.
        /// </summary>
        public virtual void ClearData()
        {
            throw this.CreateNotSupportedException("Clearing an control");
        }

        /// <summary>
        /// Clicks the control that this property represents.
        /// </summary>
        public virtual void ClickControl()
        {
            throw this.CreateNotSupportedException("Clicking an control");
        }

        /// <summary>
        /// Checks to see if the control is enabled.
        /// </summary>
        /// <returns><c>true</c> if the control is enabled.</returns>
        public virtual bool CheckControlEnabled()
        {
            throw this.CreateNotSupportedException("Checking for an control being enabled");
        }

        /// <summary>
        /// Checks to see if the control exists.
        /// </summary>
        /// <returns><c>true</c> if the control exists.</returns>
        public virtual bool CheckControlExists()
        {
            throw this.CreateNotSupportedException("Checking for an control existing");
        }

        /// <summary>
        /// Checks to see if the control does not exist.
        /// </summary>
        /// <returns><c>true</c> if the control exists.</returns>
        public virtual bool CheckControlNotExists()
        {
            throw this.CreateNotSupportedException("Checking for an control not existing");
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void FillData(string data)
        {
            throw this.CreateNotSupportedException("Filling in data");
        }

        /// <summary>
        /// Gets the current value of the property.
        /// </summary>
        /// <returns>The current value as a string.</returns>
        public virtual string GetCurrentValue()
        {
            throw this.CreateNotSupportedException("Getting the current value");
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The item at the specified index; otherwise <c>null</c>.
        /// </returns>
        public virtual IWindow GetItemAtIndex(int index)
        {
            throw this.CreateNotSupportedException("Getting an item at a given index");
        }

        /// <summary>
        /// Gets the item as a window.
        /// </summary>
        /// <returns>
        /// The item as a window.
        /// </returns>
        public virtual IWindow GetItemAsWindow()
        {
            throw this.CreateNotSupportedException("Getting a property as a window item");
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public virtual void Highlight()
        {
        }

        /// <summary>
        /// Validates the item or property matches the expected expression.
        /// </summary>
        /// <param name="validation">The validation item.</param>
        /// <param name="actualValue">The actual value if validation fails.</param>
        /// <returns>
        ///   <c>true</c> if the items are valid; otherwise <c>false</c>.
        /// </returns>
        public abstract bool ValidateItem(ItemValidation validation, out string actualValue);

        /// <summary>
        /// Waits for the control condition to be met.
        /// </summary>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The timeout to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        public virtual bool WaitForControlCondition(WaitConditions waitCondition, TimeSpan? timeout)
        {
            throw this.CreateNotSupportedException("Waiting for an element");
        }

        /// <summary>
        /// Compares the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="validation">The validation.</param>
        /// <param name="actualValue">The actual value.</param>
        /// <returns>
        ///   <c>true</c> if the comparison is valid.
        /// </returns>
        protected bool ComparePropertyValue(object propertyValue, ItemValidation validation, out string actualValue)
        {
            var stringItems = propertyValue as IEnumerable<string>;
            if (stringItems != null)
            {
                var list = stringItems.ToList();
                actualValue = string.Join(",", list);
                return list.Any(s => validation.Compare(this, s));
            }

            actualValue = propertyValue != null ? propertyValue.ToString() : null;
            return validation.Compare(this, actualValue);
        }

        /// <summary>
        /// Creates the not supported exception.
        /// </summary>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>The created exception.</returns>
        private Exception CreateNotSupportedException(string operationName)
        {
            return new NotSupportedException(string.Format("{0} is not suppported by property type '{1}'", operationName, this.GetType().Name));
        }
    }
}
