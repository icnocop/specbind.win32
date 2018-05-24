namespace SpecBind.Extensions
{
    using System;

    /// <summary>
    /// A set of extension methods to analyze types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified check type is a supported property type.
        /// </summary>
        /// <param name="checkType">The type to check.</param>
        /// <param name="elementType">Type of the elements used in the driver.</param>
        /// <returns><c>true</c> if the type is supported; otherwise, <c>false</c>.</returns>
        public static bool IsSupportedPropertyType(this Type checkType, Type elementType)
        {
            return elementType.IsAssignableFrom(checkType);
        }
    }
}
