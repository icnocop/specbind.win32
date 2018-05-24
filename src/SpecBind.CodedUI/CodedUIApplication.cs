using Microsoft.VisualStudio.TestTools.UITesting;
using SpecBind.Application;

namespace SpecBind.CodedUI
{
    public class CodedUIApplication<TApplication> : CodedUIWindow<TApplication>, IApplication
        where TApplication : ApplicationUnderTest
    {
        private readonly ApplicationUnderTest applicationUnderTest;
        private readonly TApplication application;

        public CodedUIApplication(ApplicationUnderTest applicationUnderTest, TApplication application)
            : base(applicationUnderTest, application, application.GetType(), null, applicationUnderTest)
        {
            this.applicationUnderTest = applicationUnderTest;
            this.application = application;
        }

        public bool IsRunning
        {
            get
            {
                return !this.applicationUnderTest.Process.HasExited;
            }
        }

        public int ExitCode
        {
            get
            {
                return this.applicationUnderTest.Process.ExitCode;
            }
        }

        public override void Highlight()
        {
            // even though we can try to highlight the main window of the application here,
            // it should actually be done on the window itself
        }
    }
}
