using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using SpecBind.Control;

namespace SpecBind.CodedUI.IntegrationTests.Windows
{
    public class ChildWindow : WinWindow
    {
        public ChildWindow(UITestControl parent)
            : base(parent)
        {
        }

        [ControlLocator(Name = "OK")]
        public WinButton OK { get; set; }
    }
}
