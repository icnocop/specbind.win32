using SpecBind.Application;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace SpecBind.CodedUI.IntegrationTests.Applications
{
    public class Win32ApplicationLauncher : CodedUIApplicationLauncher<Win32Application>
    {
        public override IApplication Attach()
        {
            Process process = Process.GetProcessesByName("SpecBind.Win32App").First();

            return this.Attach(process);
        }

        public override IApplication Launch()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string exeFilePath = Path.Combine(currentDirectory, "SpecBind.Win32App.exe");

            return this.Launch(exeFilePath);
        }
    }
}
