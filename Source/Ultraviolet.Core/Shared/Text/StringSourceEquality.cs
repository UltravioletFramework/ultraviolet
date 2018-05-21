using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Contains methods for equality comparisons between string sources.
    /// </summary>
    internal static class StringSourceEquality
    {
        /// <summary>
        /// Compares two string sources for equality.
        /// </summary>
        /// <typeparam name="TSource1">The type of the first string source to compare.</typeparam>
        /// <typeparam name="TSource2">The type of the second string source to compare.</typeparam>
        /// <param name="s1">The first string source to compare.</param>
        /// <param name="s2">The second string source to compare.</param>
        /// <returns><see langword="true"/> if the string sources are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean Equals<TSource1, TSource2>(TSource1 s1, TSource2 s2) 
            where TSource1 : IStringSource<Char>
            where TSource2 : IStringSource<Char>
        {
            var s1IsNull = (s1 == null || s1.IsNull);
            var s2IsNull = (s2 == null || s2.IsNull);
            if (s1IsNull)
                return s2IsNull;
            if (s2IsNull)
                return false;

            if (s1.IsEmpty)
                return s2.IsEmpty;
            if (s2.IsEmpty)
                return false;

            if (s1.Length != s2.Length)
                return false;

            var length = s1.Length;
            for (int i = 0; i < length; i++)
            {
                if (s1[i] != s2[i])
                    return false;
            }
            return true;
        }
    }
}
