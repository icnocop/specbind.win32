using System;

namespace SpecBind.Actions
{
    internal class WindowLocatorException : ApplicationException
    {
        public WindowLocatorException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}