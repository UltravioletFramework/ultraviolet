using System;
using System.Text;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Contains methods for generating standard UVSS-style names from C#-style names.
    /// </summary>
    internal static class UvssNameGenerator
    {
        /// <summary>
        /// Generates a UVSS styling name from the specified property name.
        /// </summary>
        /// <param name="name">The property name for which to generate a UVSS styling name.</param>
        /// <returns>The UVSS styling name that was generated from the specified property name.</returns>
        public static String GenerateUvssName(String name)
        {
            var sb = new StringBuilder();
            var offset = 0;

            if (offset == 0)
            {
                offset = name.StartsWith("Is", StringComparison.InvariantCultureIgnoreCase) &&
                name.Length > 2 && Char.IsUpper(name[2]) ? 2 : 0;
            }

            if (offset == 0)
            {
                offset = name.StartsWith("Are", StringComparison.InvariantCultureIgnoreCase) &&
                name.Length > 3 && Char.IsUpper(name[3]) ? 3 : 0;
            }

            for (int i = offset; i < name.Length; i++)
            {
                var c = name[i];

                if (!Char.IsLetterOrDigit(c))
                    continue;

                if (i > offset && Char.IsUpper(c))
                {
                    sb.Append('-');
                    sb.Append(Char.ToLower(c));
                }
                else
                {
                    sb.Append(Char.ToLower(c));
                }
            }

            return sb.ToString();
        }
    }
}
