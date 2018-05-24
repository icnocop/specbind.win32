using SpecBind.Application;
using SpecBind.Logging;
using System;
using System.Reflection;
using System.Linq;

namespace SpecBind.CodedUI
{
    public class CodedUIApplicationFactory : ApplicationFactory
    {
        public CodedUIApplicationFactory()
        {
            this.BaseType = typeof(CodedUIApplicationLauncher<>);
            this.BaseWindowType = typeof(CodedUIWindow<>);
        }

        public override IApplication AttachApplication(ILogger logger, Type type)
        {
            var applicationLauncher = Activator.CreateInstance(type);
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(x => x.Name == "Attach"
                   && x.GetParameters().Count() == 0);

            return (IApplication)method.Invoke(applicationLauncher, null);
        }

        public override IApplication LaunchApplication(ILogger logger, Type type)
        {
            var applicationLauncher = Activator.CreateInstance(type);
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(x => x.Name == "Launch"
                   && x.GetParameters().Count() == 0);

            return (IApplication)method.Invoke(applicationLauncher, null);
        }
    }
}
