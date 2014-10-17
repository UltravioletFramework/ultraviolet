using System;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Contains extensions to the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Counts the number of times that the specified character occurs within the string.
        /// </summary>
        /// <param name="str">The string to evaluate.</param>
        /// <param name="c">The character for which to search.</param>
        /// <returns>The number of times that the specified character occurs within the string.</returns>
        public static Int32 Count(this String str, Char c)
        {
            Contract.Require(str, "str");

            var count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Capitalizes the string, converting its first character to uppercase and the
        /// rest of its characters to lowercase.
        /// </summary>
        /// <param name="str">The string that will be capitalized.</param>
        /// <returns>A new string with the first character converted to uppercase and the
        /// rest of its characters converted to lowercase.</returns>
        public static String Capitalize(this String str)
        {
            Contract.Require(str, "str");

            if (str == String.Empty)
                return String.Empty;

            string temp = str.Substring(0, 1);
            return temp.ToUpper() + str.Remove(0, 1).ToLower();
        }
    }
}
