using SpecBind.Application;
using Microsoft.VisualStudio.TestTools.UITesting;
using SpecBind.Control;
using SpecBind.Extensions;
using System;
using System.Diagnostics;

namespace SpecBind.CodedUI
{
    public abstract class CodedUIApplicationLauncher<T> : IApplicationLauncher<T>
        where T : ApplicationUnderTest, new()
    {
        public virtual IApplication Attach()
        {
            throw new NotImplementedException();
        }

        public virtual IApplication Launch()
        {
            throw new NotImplementedException();
        }

        public CodedUIApplication<T> Attach(Process process)
        {
            ApplicationUnderTest applicationUnderTest = ApplicationUnderTest.FromProcess(process);

            return Create(applicationUnderTest);
        }

        public CodedUIApplication<T> Launch(string exeFilePath)
        {
            ApplicationUnderTest applicationUnderTest = ApplicationUnderTest.Launch(exeFilePath);

            return Create(applicationUnderTest);
        }

        public CodedUIApplication<T> Launch(string exeFilePath, string alternateFileName, string arguments)
        {
            ApplicationUnderTest applicationUnderTest = ApplicationUnderTest.Launch(exeFilePath, alternateFileName, arguments);

            return Create(applicationUnderTest);
        }

        private CodedUIApplication<T> Create(ApplicationUnderTest applicationUnderTest)
        {
            var applicationType = typeof(T);
            ControlLocatorAttribute locatorAttribute;
            if (applicationType.TryGetAttribute(out locatorAttribute))
            {
                WindowBuilder.AssignControlAttributes(applicationUnderTest, locatorAttribute);
            }

            return new CodedUIApplication<T>(applicationUnderTest, new T());
        }
    }
}
