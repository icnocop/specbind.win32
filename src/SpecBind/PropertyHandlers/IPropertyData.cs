using SpecBind.Actions;
using SpecBind.Validation;
using SpecBind.Control;
using System;
using SpecBind.Window;

namespace SpecBind.PropertyHandlers
{
    public interface IPropertyData
    {
        /// <summary>
        ///     Gets a value indicating whether this instance represents a window control.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is a window control; otherwise, <c>false</c>.
        /// </value>
        bool IsControl { get; }

        bool IsWindow { get; }

        bool IsApplication { get; }

        /// <summary>
        /// Gets the name if the property.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        ///     Gets the type of the property.
        /// </summary>
        /// <value>
        ///     The type of the property.
        /// </value>
        Type PropertyType { get; }

        /// <summary>
        /// Clears the data on the control.
        /// </summary>
        void ClearData();

        /// <summary>
        /// Clicks the control that this property represents.
        /// </summary>
        void ClickControl();

        /// <summary>
        /// Checks to see if the control is enabled.
        /// </summary>
        /// <returns><c>true</c> if the control is enabled.</returns>
        bool CheckControlEnabled();

        /// <summary>
        /// Checks to see if the control exists.
        /// </summary>
        /// <returns><c>true</c> if the control exists.</returns>
        bool CheckControlExists();

        /// <summary>
        /// Checks to see if the control doesn't exist.
        /// </summary>
        /// <returns><c>true</c> if the control doesn't exist.</returns>
        bool CheckControlNotExists();

        /// <summary>
        /// Fills the data on the control.
        /// </summary>
        /// <param name="data">The data.</param>
        void FillData(string data);

        /// <summary>
        /// Gets the current value of the property.
        /// </summary>
        /// <returns>The current value as a string.</returns>
        string GetCurrentValue();

        /// <summary>
        /// Gets the index of the item at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item as an <see cref="IWindow"/> item; otherwise <c>null</c>.</returns>
        IWindow GetItemAtIndex(int index);

        /// <summary>
        /// Gets the item as a window.
        /// </summary>
        /// <returns>The item as a window.</returns>
        IWindow GetItemAsWindow();

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        void Highlight();

        /// <summary>
        /// Validates the item or property matches the expected expression.
        /// </summary>
        /// <param name="validation">The validation item.</param>
        /// <param name="actualValue">The actual value if validation fails.</param>
        /// <returns>
        ///   <c>true</c> if the items are valid; otherwise <c>false</c>.
        /// </returns>
        bool ValidateItem(ItemValidation validation, out string actualValue);

        /// <summary>
        /// Waits for control condition.
        /// </summary>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The timeout to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        bool WaitForControlCondition(WaitConditions waitCondition, TimeSpan? timeout);
    }
}