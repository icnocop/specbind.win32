using System.Collections.Generic;

namespace SpecBind.Control
{
    public interface IControlProvider
    {
        /// <summary>
        /// Gets the control descriptions on the control.
        /// </summary>
        /// <returns>A collection of descriptions.</returns>
        IEnumerable<ControlDescription> GetControls();
    }
}
