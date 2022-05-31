using System;

namespace Ultraviolet.Core.Text
{
    partial struct StringBuilderSource : IEquatable<String>, IEquatable<StringSource>, IEquatable<StringBuilderSource>, IEquatable<StringSegmentSource>
    {
        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="String"/> instance
        /// to determine whether they are equal; that is, whether the contain the same characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="String"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringBuilderSource ss, String other) =>
            ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="String"/> instance
        /// to determine whether they are unequal; that is, whether they contain different characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="String"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringBuilderSource ss, String other) =>
            !ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="String"/> instance
        /// to determine whether they are equal; that is, whether the contain the same characters.
        /// </summary>
        /// <param name="str">The <see cref="String"/> instance to compare.</param>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(String str, StringBuilderSource ss) =>
            ss.Equals(str);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="String"/> instance
        /// to determine whether they are unequal; that is, whether they contain different characters.
        /// </summary>
        /// <param name="str">The <see cref="String"/> instance to compare.</param>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(String str, StringBuilderSource ss) =>
            !ss.Equals(str);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringSource"/> instance
        /// to determine whether they are equal; that is, whether the contain the same characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringBuilderSource ss, StringSource other) =>
            ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringSource"/> instance
        /// to determine whether they are unequal; that is, whether the contain different characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringBuilderSource ss, StringSource other) =>
            !ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringBuilderSource"/> instance
        /// to determine whether they are equal; that is, whether the contain the same characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringBuilderSource ss, StringBuilderSource other) =>
            ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringBuilderSource"/> instance
        /// to determine whether they are unequal; that is, whether the contain different characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringBuilderSource ss, StringBuilderSource other) =>
            !ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringSegmentSource"/> instance
        /// to determine whether they are equal; that is, whether the contain the same characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringSegmentSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringBuilderSource ss, StringSegmentSource other) =>
            ss.Equals(other);

        /// <summary>
        /// Compares a <see cref="StringBuilderSource"/> instance with a <see cref="StringSegmentSource"/> instance
        /// to determine whether they are unequal; that is, whether the contain different characters.
        /// </summary>
        /// <param name="ss">The <see cref="StringBuilderSource"/> instance to compare.</param>
        /// <param name="other">The <see cref="StringSegmentSource"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringBuilderSource ss, StringSegmentSource other) =>
            !ss.Equals(other);

        /// <inheritdoc/>
        public override Int32 GetHashCode() =>
            source?.GetHashCode() ?? 0;

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            switch (obj)
            {
                case String sb:
                    return Equals(sb);
                case StringSource ss:
                    return Equals(ss);
                case StringBuilderSource sbs:
                    return Equals(sbs);
                case StringSegmentSource sss:
                    return Equals(sss);
                default:
                    return (obj is IStringSource<Char> iss) ? Equals(iss) : false;
            }
        }

        /// <inheritdoc/>
        public Boolean Equals(String other) =>
            StringSourceEquality.Equals(this, new StringSource(other));

        /// <inheritdoc/>
        public Boolean Equals(StringSource other) =>
            StringSourceEquality.Equals(this, other);

        /// <inheritdoc/>
        public Boolean Equals(StringBuilderSource other) =>
            StringSourceEquality.Equals(this, other);

        /// <inheritdoc/>
        public Boolean Equals(StringSegmentSource other) =>
            StringSourceEquality.Equals(this, other);

        /// <inheritdoc/>
        public Boolean Equals(IStringSource<Char> other) =>
            StringSourceEquality.Equals(this, other);
    }
}
