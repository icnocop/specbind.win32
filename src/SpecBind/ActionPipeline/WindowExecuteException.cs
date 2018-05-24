using System;

namespace SpecBind.ActionPipeline
{
    [Serializable]
    public class WindowExecuteException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowExecuteException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public WindowExecuteException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}