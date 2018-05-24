using SpecBind.Control;
using SpecBind.PropertyHandlers;
using System;
using System.Collections.Generic;

namespace SpecBind.Window
{
    public interface IWindow
    {
        /// <summary>
        /// Gets the type of the window.
        /// </summary>
        /// <value>
        /// The type of the window.
        /// </value>
        Type WindowType { get; }

        IWindow ParentWindow { get; set; }

        /// <summary>
        /// Gets the native window object.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window.</typeparam>
        /// <returns>
        /// The native window object.
        /// </returns>
        TWindow GetNativeWindow<TWindow>() where TWindow : class;

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A list of matching properties.</returns>
        IEnumerable<string> GetPropertyNames(Func<IPropertyData, bool> filter);

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        void Highlight();

        /// <summary>
        /// Tries the get the control.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <returns>
        /// <c>true</c> if the control exists; otherwise <c>false</c>.
        /// </returns>
        bool TryGetControl(string key, out IPropertyData propertyData, Func<IPropertyData, bool> condition);

        /// <summary>
        /// Tries the get the control.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <returns>
        /// <c>true</c> if the control exists; otherwise <c>false</c>.
        /// </returns>
        bool TryGetProperty(string key, out IPropertyData propertyData);

        void FindWindow(Type propertyType, out IWindow window);

        /// <summary>
        /// Waits for the window to become active based on some user content.
        /// </summary>
        void WaitForWindowToBeActive();
    }
}
