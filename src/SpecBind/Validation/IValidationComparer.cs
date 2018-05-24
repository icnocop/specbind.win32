﻿using SpecBind.PropertyHandlers;
using SpecBind.Control;
using System.Collections.Generic;

namespace SpecBind.Validation
{
    public interface IValidationComparer
    {
        /// <summary>
        /// Gets the rule keys.
        /// </summary>
        /// <value>The rule keys.</value>
        IEnumerable<string> RuleKeys { get; }

        /// <summary>
        /// Gets a value indicating whether the control should be checked for existence.
        /// </summary>
        /// <value><c>true</c> if the control should be checked; otherwise, <c>false</c>.</value>
        bool ShouldCheckControlExistence { get; }

        /// <summary>
        /// Gets a value indicating whether this validation requires a field value.
        /// </summary>
        /// <value><c>true</c> if a field value is required; otherwise, <c>false</c>.</value>
        bool RequiresFieldValue { get; }

        /// <summary>
        /// Compares the values using the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="actualValue">The actual value.</param>
        /// <returns><c>true</c> if the comparison passes, <c>false</c> otherwise.</returns>
        bool Compare(IPropertyData property, string expectedValue, string actualValue);
    }
}