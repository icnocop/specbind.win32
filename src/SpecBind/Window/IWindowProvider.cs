using SpecBind.Control;
using System.Collections.Generic;

namespace SpecBind.Window
{
    public interface IWindowProvider
    {
        /// <summary>
        /// Gets the control descriptions on the control.
        /// </summary>
        /// <returns>A collection of descriptions.</returns>
        IEnumerable<ControlDescription> GetWindows();
    }
}