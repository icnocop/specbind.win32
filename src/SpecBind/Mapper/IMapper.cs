using System;

namespace SpecBind.Mapper
{
    public interface IMapper
    {
        /// <summary>Gets the window type from the given name</summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The resolved type; otherwise <c>null</c>.</returns>
        Type GetTypeFromName(string typeName);
    }
}
