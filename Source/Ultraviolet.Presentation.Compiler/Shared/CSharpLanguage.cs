using System;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods relating to C# language production.
    /// </summary>
    public static class CSharpLanguage
    {
        /// <summary>
        /// Gets the name of the specified type as a C# identifier.
        /// </summary>
        /// <param name="type">The type for which to retrieve a C# identifier.</param>
        /// <returns>The C# identifier that represents the specified type.</returns>
        public static String GetCSharpTypeName(Type type)
        {
            Contract.Require(type, nameof(type));

            if (type == typeof(void))
                return "void";

            var isByRef = type.IsByRef;
            if (isByRef)
                type = type.GetElementType();

            var name = (type.IsGenericParameter ? type.Name : "global::" + type.FullName) ?? type.Name;
            name = name.Replace('+', '.');

            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                var genericTypeName = genericTypeDef.FullName.Substring(0, genericTypeDef.FullName.IndexOf('`'));
                var genericArguments = type.GetGenericArguments();

                name = String.Format("{0}<{1}>", genericTypeName,
                    String.Join(", ", genericArguments.Select(x => GetCSharpTypeName(x))));
            }
            
            return isByRef ? "ref " + name : name;
        }
    }
}
