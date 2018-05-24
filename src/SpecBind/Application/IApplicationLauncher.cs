namespace SpecBind.Application
{
    public interface IApplicationLauncher<T>
        where T : class
    {
        IApplication Launch();

        IApplication Attach();
    }
}
