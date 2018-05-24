using SpecBind.Actions;
using SpecBind.Control;
using System;

namespace SpecBind.Window
{
    public interface IWindowControlHandler<in TControl> : IWindow
    {
        /// <summary>
        /// Checks if the control is enabled.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control is enabled.</returns>
        bool ControlEnabledCheck(TControl control);

        /// <summary>
        /// Checks if the control exists.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// <c>true</c> if the control exists; otherwise <c>false</c>.
        /// </returns>
        bool ControlExistsCheck(TControl control);

        /// <summary>
        /// Checks if the control doesn't exist.
        /// This doesn't wait for the element to exist.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// <c>true</c> if the control doesn't exist; otherwise <c>false</c>.
        /// </returns>
        bool ControlNotExistsCheck(TControl control);

        /// <summary>
        /// Gets the control attribute value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The attribute's value.</returns>
        string GetControlAttributeValue(TControl control, string attributeName);

        /// <summary>
        /// Gets the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The control's text.</returns>
        string GetControlText(TControl control);

        /// <summary>
        /// Gets the window from the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The window interface.</returns>
        IWindow GetWindowFromControl(TControl control);

        /// <summary>
        /// Clicks the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// <c>true</c> if the click is successful.
        /// </returns>
        bool ClickControl(TControl control);

        /// <summary>
        /// Gets the clears method.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///  The function used to clear the data.
        /// </returns>
        Action<TControl> GetClearMethod(Type propertyType);

        /// <summary>
        /// Gets the window fill method.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        /// The function used to fill the data.
        /// </returns>
        Action<TControl, string> GetWindowFillMethod(Type propertyType);

        /// <summary>
        /// Highlights the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        void Highlight(TControl control);

        /// <summary>
        /// Waits for the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The timeout to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        bool WaitForControl(TControl control, WaitConditions waitCondition, TimeSpan? timeout);
    }
}