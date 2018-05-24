using SpecBind.Application;
using SpecBind.Extensions;
using SpecBind.PropertyHandlers;
using SpecBind.Window;
using System;
using System.Collections.Generic;

namespace SpecBind.ActionPipeline
{
    internal class WindowLocator : IWindowLocator
    {
        private readonly List<ILocatorAction> filterActions;
        private readonly IWindow parentWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLocator" /> class.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="filterActions">The filter actions.</param>
        public WindowLocator(IWindow parentWindow, IEnumerable<ILocatorAction> filterActions)
        {
            this.parentWindow = parentWindow;
            this.filterActions = new List<ILocatorAction>(filterActions);
        }

        /// <summary>
        /// Gets the window from the context.
        /// </summary>
        /// <param name="propertyName">The property name to locate.</param>
        /// <returns>The resulting property data.</returns>
        /// <exception cref="WindowExecuteException">Thrown when the window could not be found.</exception>
        public IPropertyData GetWindow(string propertyName)
        {
            IPropertyData propertyData;
            if (!this.TryGetWindow(propertyName, out propertyData))
            {
                throw GetWindowNotFoundException(this.parentWindow, propertyName, f => f.IsApplication);
            }

            return propertyData;
        }

        /// <summary>
        /// Tries the get the window.
        /// </summary>
        /// <param name="propertyName">The key.</param>
        /// <param name="propertyData">The property data.</param>
        /// <returns><c>true</c> if the window exists; otherwise <c>false</c>.</returns>
        public bool TryGetWindow(string propertyName, out IPropertyData propertyData)
        {
            foreach (var locatorAction in this.filterActions)
            {
                locatorAction.OnLocate(propertyName);
            }

            var result = this.parentWindow.TryGetControl(propertyName.ToLookupKey(), out propertyData, (x) => { return x.IsWindow; });

            foreach (var locatorAction in this.filterActions)
            {
                locatorAction.OnLocateComplete(propertyName, propertyData);
            }

            return result;
        }

        /// <summary>
        /// Gets the window not found exception.
        /// </summary>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The created exception.
        /// </returns>
        private static WindowExecuteException GetWindowNotFoundException(IWindow parentWindow, string fieldName, Func<IPropertyData, bool> filter)
        {
            string availableFields = null;

            var builder = new System.Text.StringBuilder(" Available Fields: ");
            builder.AppendLine();

            foreach (var field in parentWindow.GetPropertyNames(filter))
            {
                builder.AppendLine(field);
            }

            availableFields = builder.ToString();

            return new WindowExecuteException("Could not locate property '{0}' on window {1}.{2}", fieldName, parentWindow.WindowType.Name, availableFields);
        }
    }
}
