using SpecBind.Window;

namespace SpecBind.Application
{
    public abstract class ApplicationBase<TParent, TApplication, TWindow> : WindowBase<TParent, TWindow, TWindow>, IApplication /* IWindowControlHandler<TWindow>*/
        where TParent : class
        where TWindow : class
    {
        public ApplicationBase(TParent parent, TApplication application, IWindow parentWindow)
            : base(parent, parent, application.GetType(), parentWindow)
        {
        }

        public abstract int ExitCode { get; }
        public abstract bool IsRunning { get; }
    }
}
