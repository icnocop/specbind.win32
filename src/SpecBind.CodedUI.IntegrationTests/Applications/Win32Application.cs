using Microsoft.VisualStudio.TestTools.UITesting;
using SpecBind.CodedUI.IntegrationTests.Windows;
using SpecBind.Control;

namespace SpecBind.CodedUI.IntegrationTests.Applications
{
    public class Win32Application : ApplicationUnderTest
    {
        [ControlLocator(Title = "SpecBind.Win32App")]
        public MainWindow Main { get; set; }
    }
}
