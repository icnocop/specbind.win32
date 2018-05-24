using SpecBind.PropertyHandlers;

namespace SpecBind.ActionPipeline
{
    public interface IControlLocator
    {
        /// <summary>
        /// Gets the control from the context.
        /// </summary>
        /// <param name="propertyName">The property name to locate.</param>
        /// <returns>The resulting property data.</returns>
        /// <exception cref="ControlExecuteException">Thrown when the control could not be found.</exception>
        IPropertyData GetControl(string propertyName);

        /// <summary>
        /// Tries to the get the control.
        /// </summary>
        /// <param name="propertyName">The key.</param>
        /// <param name="propertyData">The property data.</param>
        /// <returns><c>true</c> if the control exists; otherwise <c>false</c>.</returns>
        bool TryGetControl(string propertyName, out IPropertyData propertyData);
    }
}