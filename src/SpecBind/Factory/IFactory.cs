using SpecBind.Logging;

namespace SpecBind.Factory
{
    public interface IFactory
    {
        ILogger Logger { get; set; }
    }
}