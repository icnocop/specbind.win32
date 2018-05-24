using SpecBind.Mapper;

namespace SpecBind.Window
{
    public class WindowMapper : MapperBase, IWindowMapper
    {
        private const string ClassNameSuffix = "Window";

        public WindowMapper()
            : base(ClassNameSuffix)
        {
        }
    }
}
