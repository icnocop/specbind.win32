using System;

namespace SpecBind.Control
{
    internal class ControlLocatorException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlLocatorException" /> class.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        public ControlLocatorException(Type windowType)
            : base(string.Format("Cannot find window: {0}", windowType.Name))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlLocatorException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments for the format string.</param>
        public ControlLocatorException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}