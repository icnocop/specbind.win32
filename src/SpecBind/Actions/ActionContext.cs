using SpecBind.Application;

namespace SpecBind.Actions
{
    public class ActionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ActionContext(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }
    }
}