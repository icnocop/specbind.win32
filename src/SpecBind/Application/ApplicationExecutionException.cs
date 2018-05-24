using System;

namespace SpecBind.Application
{
    [Serializable]
    internal class ApplicationExecutionException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationExecutionException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments for the format string.</param>
        public ApplicationExecutionException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}