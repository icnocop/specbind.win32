using SpecBind.PropertyHandlers;
using SpecBind.Control;
using System;
using System.Collections.Generic;
using SpecBind.Window;

namespace SpecBind.ActionPipeline
{
    internal class ControlLocater : IControlLocator
    {
        private readonly List<ILocatorAction> filterActions;
        private readonly IWindow window;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlLocater" /> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="filterActions">The filter actions.</param>
        public ControlLocater(IWindow window, IEnumerable<ILocatorAction> filterActions)
        {
            this.window = window;
            this.filterActions = new List<ILocatorAction>(filterActions);
        }

        /// <summary>
        /// Gets the control from the context.
        /// </summary>
        /// <param name="propertyName">The property name to locate.</param>
        /// <returns>The resulting property data.</returns>
        /// <exception cref="ControlExecuteException">Thrown when the control could not be found.</exception>
        public IPropertyData GetControl(string propertyName)
        {
            IPropertyData propertyData;
            if (!this.TryGetControl(propertyName, out propertyData))
            {
                throw GetControlNotFoundException(this.window, propertyName, f => f.IsControl);
            }

            return propertyData;
        }

        /// <summary>
        /// Tries the get the control.
        /// </summary>
        /// <param name="propertyName">The key.</param>
        /// <param name="propertyData">The property data.</param>
        /// <returns><c>true</c> if the control exists; otherwise <c>false</c>.</returns>
        public bool TryGetControl(string propertyName, out IPropertyData propertyData)
        {
            foreach (var locatorAction in this.filterActions)
            {
                locatorAction.OnLocate(propertyName);
            }

            var result = this.window.TryGetControl(propertyName, out propertyData, (x) => { return x.IsControl; });

            foreach (var locatorAction in this.filterActions)
            {
                locatorAction.OnLocateComplete(propertyName, propertyData);
            }

            return result;
        }

        /// <summary>
        /// Gets the control not found exception.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The created exception.
        /// </returns>
        private static ControlExecuteException GetControlNotFoundException(IWindow window, string fieldName, Func<IPropertyData, bool> filter)
        {
            string availableFields = null;

            var builder = new System.Text.StringBuilder(" Available Fields: ");
            builder.AppendLine();

            foreach (var field in window.GetPropertyNames(filter))
            {
                builder.AppendLine(field);
            }

            availableFields = builder.ToString();

            return new ControlExecuteException("Could not locate property '{0}' on window {1}.{2}", fieldName, window.WindowType.Name, availableFields);
        }
    }
}
