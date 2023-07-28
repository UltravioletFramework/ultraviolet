using System;
using System.Linq;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for analyzing types that appear in UVML.
    /// </summary>
    internal static class UvmlTypeAnalysis
    {
        /// <summary>
        /// Gets the type associated with the specified placeholder string in the context of the specified type.
        /// </summary>
        /// <param name="type">The type for which to evaluate placeholder substitutions.</param>
        /// <param name="placeholder">The placeholder string for which to evaluate substitutions.</param>
        /// <returns>The type associated with the specified placeholder, or <see langword="null"/> if there is no such placeholder.</returns>
        public static Type GetPlaceholderType(Type type, String placeholder)
        {
            var attr = type.GetCustomAttributes(typeof(UvmlPlaceholderAttribute), true).Cast<UvmlPlaceholderAttribute>()
                .Where(x => String.Equals(placeholder, x.Placeholder, StringComparison.Ordinal)).SingleOrDefault();
            if (attr != null)
            {
                return attr.Type;
            }
            return null;
        }
    }
}
