namespace SpecBind.Logging
{
    /// <summary>
    /// A logger for marking diagnostics in the system.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the debug level message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments for the message.</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// Logs the information level message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments for the message.</param>
        void Info(string format, params object[] args);
    }
}