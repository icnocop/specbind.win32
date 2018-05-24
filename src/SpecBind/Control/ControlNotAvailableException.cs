using System;
using System.Runtime.Serialization;

namespace SpecBind.CodedUI
{
    public class ControlNotAvailableException : Exception
    {
        public ControlNotAvailableException(Exception innerException)
            : base(null, innerException)
        {
        }
    }
}