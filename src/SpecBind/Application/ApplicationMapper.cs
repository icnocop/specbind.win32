using SpecBind.Mapper;

namespace SpecBind.Application
{
    public class ApplicationMapper : MapperBase, IApplicationMapper
    {
        private const string ClassNameSuffix = "ApplicationLauncher";

        public ApplicationMapper()
            : base(ClassNameSuffix)
        {
        }
    }
}
