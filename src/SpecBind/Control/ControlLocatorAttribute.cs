using System;

namespace SpecBind.Control
{
    /// <summary>
    /// An attribute for locating controls on a window.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControlLocatorAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the title to find.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        public string Name { get; set; }

        public string NameContains { get; set; }

        public string ClassName { get; set; }

        public string ClassNameContains { get; set; }
    }
}
