using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using SpecBind.Control;

namespace SpecBind.CodedUI.IntegrationTests.Windows
{
    public class MainWindow : WinWindow
    {
        public MainWindow(UITestControl parent)
            : base(parent)
        {
        }

        [ControlLocator(Name = "Display child dialog")]
        public WinButton DisplayChildDialog { get; set; }

        [ControlLocator(Name = "OK")]
        public WinButton OK { get; set; }

        [ControlLocator(Title = "Child Dialog")]
        public ChildWindow Child { get; set; }
    }
}
