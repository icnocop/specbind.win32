using SpecBind.Window;

namespace SpecBind.Application
{
    public interface IApplication : IWindow
    {
        bool IsRunning { get; }

        int ExitCode { get; }
    }
}