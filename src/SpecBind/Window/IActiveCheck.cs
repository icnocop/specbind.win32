namespace SpecBind.Window
{
    /// <summary>
    /// An interface that can be applied to a window to allow it to wait for it to become active.
    /// </summary>
    public interface IActiveCheck
    {
        /// <summary>
        /// Waits for the window to become active based on a property.
        /// </summary>
        void WaitForActive();
    }
}
