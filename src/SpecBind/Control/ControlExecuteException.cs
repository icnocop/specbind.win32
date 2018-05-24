using System;

namespace SpecBind.Control
{
    [Serializable]
    public class ControlExecuteException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlExecuteException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ControlExecuteException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}