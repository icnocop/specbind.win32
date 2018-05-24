using SpecBind.Factory;
using SpecBind.Logging;
using System;

namespace SpecBind.Application
{
    public abstract class ApplicationFactory : FactoryBase<ApplicationFactory>, IFactory
    {
        public Type BaseWindowType { get; set; }

        public abstract IApplication LaunchApplication(ILogger logger, Type type);

        public abstract IApplication AttachApplication(ILogger logger, Type type);
    }
}
