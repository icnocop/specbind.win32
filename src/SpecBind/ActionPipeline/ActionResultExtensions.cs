using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBind.ActionPipeline
{
    public static class ActionResultExtensions
    {
        /// <summary>
        /// Checks the result to ensure there are no errors and returns the value.
        /// </summary>
        /// <typeparam name="T">The type the result should be converted to.</typeparam>
        /// <param name="result">The action result.</param>
        /// <returns>The resulting object.</returns>
        public static T CheckResult<T>(this ActionResult result) where T : class
        {
            return CheckActionResult(result) as T;
        }

        /// <summary>
        /// Checks the result to ensure there are no errors.
        /// </summary>
        /// <param name="result">The action result.</param>
        public static void CheckResult(this ActionResult result)
        {
            CheckActionResult(result);
        }

        /// <summary>
        /// Checks the action result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The value of the result.</returns>
        private static object CheckActionResult(ActionResult result)
        {
            if (result == null)
            {
                return null;
            }

            if (!result.Success && result.Exception != null)
            {
                throw new Exception($"The Action '{result.ActionType}' failed to execute.", result.Exception);
            }

            return result.Result;
        }
    }
}
