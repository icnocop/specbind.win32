using System;

namespace SpecBind.ActionPipeline
{
    public class ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResult" /> class.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="success">if set to <c>true</c> the action was successful.</param>
        /// <param name="result">The result.</param>
        /// <param name="exception">The exception.</param>
        private ActionResult(Type actionType, bool success, object result, Exception exception)
        {
            this.ActionType = actionType;
            this.Exception = exception;
            this.Result = result;
            this.Success = success;
        }

        public Type ActionType { get; private set; }

        /// <summary>
        /// Gets the exception if the action fails.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the result of the action.
        /// </summary>
        /// <value>The result of the action.</value>
        public object Result { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ActionResult"/> was successful.
        /// </summary>
        /// <value><c>true</c> was successful; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }

        /// <summary>
        /// Creates a failure action result.
        /// </summary>
        /// <param name="exception">The optional exception.</param>
        /// <returns>A new <see cref="ActionResult" /> object.</returns>
        public static ActionResult Failure(Exception exception = null)
        {
            return Failure(null, exception);
        }

        public static ActionResult Failure(Type type, Exception exception = null)
        {
            return new ActionResult(type, false, null, exception);
        }

        /// <summary>
        /// Creates a successful action result.
        /// </summary>
        /// <param name="result">The optional resulting value.</param>
        /// <returns>A new <see cref="ActionResult"/> object.</returns>
        public static ActionResult Successful(object result = null)
        {
            return new ActionResult(null, true, result, null);
        }
    }
}