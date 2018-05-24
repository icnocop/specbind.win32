using SpecBind.PropertyHandlers;

namespace SpecBind.ActionPipeline
{
    public interface IWindowLocator
    {
        /// <summary>
        /// Gets the window from the context.
        /// </summary>
        /// <param name="propertyName">The property name to locate.</param>
        /// <returns>The resulting property data.</returns>
        /// <exception cref="WindowExecuteException">Thrown when the window could not be found.</exception>
        IPropertyData GetWindow(string propertyName);

        /// <summary>
        /// Tries to the get the window.
        /// </summary>
        /// <param name="propertyName">The key.</param>
        /// <param name="propertyData">The property data.</param>
        /// <returns><c>true</c> if the window exists; otherwise <c>false</c>.</returns>
        bool TryGetWindow(string propertyName, out IPropertyData propertyData);
    }
}
