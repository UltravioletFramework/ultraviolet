using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents one of the common <see cref="IStringSource{TChar}"/> types.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct StringSourceUnion
    {
        /// <summary>
        /// Implicitly converts a <see cref="Core.Text.StringSource"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="Core.Text.StringSource"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(StringSource src)
        {
            var result = new StringSourceUnion();
            result.ValueType = StringSourceUnionValueType.String;
            result.StringSource = src;
            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Core.Text.StringBuilderSource"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="Core.Text.StringBuilderSource"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(StringBuilderSource src)
        {
            var result = new StringSourceUnion();
            result.ValueType = StringSourceUnionValueType.StringBuilder;
            result.StringBuilderSource = src;
            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Core.Text.StringSegmentSource"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="Core.Text.StringSegmentSource"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(StringSegmentSource src)
        {
            var result = new StringSourceUnion();
            result.ValueType = StringSourceUnionValueType.StringSegment;
            result.StringSegmentSource = src;
            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="ShapedString"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="ShapedString"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(ShapedString src)
        {
            var result = new StringSourceUnion();
            result.IsShaped = true;
            result.ValueType = StringSourceUnionValueType.ShapedString;
            result.ShapedStringSource = src;
            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="ShapedStringBuilder"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="ShapedStringBuilder"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(ShapedStringBuilder src)
        {
            var result = new StringSourceUnion();
            result.IsShaped = true;
            result.ValueType = StringSourceUnionValueType.ShapedStringBuilder;
            result.ShapedStringBuilderSource = src;
            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="ShapedStringSegment"/> instance to a <see cref="StringSourceUnion"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="ShapedStringBuilder"/> instance to convert.</param>
        public static implicit operator StringSourceUnion(ShapedStringSegment src)
        {
            var result = new StringSourceUnion();
            result.IsShaped = true;
            result.ValueType = StringSourceUnionValueType.ShapedStringSegment;
            result.ShapedStringSegmentSource = src;
            return result;
        }

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents this string source.
        /// </summary>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        public StringSegment CreateStringSegment()
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                    return StringSource.CreateStringSegment();

                case StringSourceUnionValueType.StringBuilder:
                    return StringBuilderSource.CreateStringSegment();

                case StringSourceUnionValueType.StringSegment:
                    return StringSegmentSource.CreateStringSegment();

                case StringSourceUnionValueType.ShapedString:
                case StringSourceUnionValueType.ShapedStringBuilder:
                case StringSourceUnionValueType.ShapedStringSegment:
                    throw new NotSupportedException();

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents a substring of
        /// this string source.
        /// </summary>
        /// <param name="start">The index of the first character in the substring that will 
        /// be represented by the string segment.</param>
        /// <param name="length">The length of the substring that will be represented by 
        /// the string segment.</param>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        public StringSegment CreateStringSegmentFromSubstring(Int32 start, Int32 length)
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                    return StringSource.CreateStringSegmentFromSubstring(start, length);

                case StringSourceUnionValueType.StringBuilder:
                    return StringBuilderSource.CreateStringSegmentFromSubstring(start, length);

                case StringSourceUnionValueType.StringSegment:
                    return StringSegmentSource.CreateStringSegmentFromSubstring(start, length);

                case StringSourceUnionValueType.ShapedString:
                case StringSourceUnionValueType.ShapedStringBuilder:
                case StringSourceUnionValueType.ShapedStringSegment:
                    throw new NotSupportedException();

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure with the same origin as this 
        /// string source but a different character range. This method only differs from
        /// the <see cref="CreateStringSegmentFromSubstring(Int32, Int32)"/> method if this
        /// string source represents a substring of some other, larger string.
        /// </summary>
        /// <param name="start">The index of the first character in the created segment.</param>
        /// <param name="length">The number of characters in the created segment.</param>
        /// <returns>The <see cref="StringSegment"/> structure that was created.</returns>
        public StringSegment CreateStringSegmentFromSameOrigin(Int32 start, Int32 length)
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                    return StringSource.CreateStringSegmentFromSameOrigin(start, length);

                case StringSourceUnionValueType.StringBuilder:
                    return StringBuilderSource.CreateStringSegmentFromSameOrigin(start, length);

                case StringSourceUnionValueType.StringSegment:
                    return StringSegmentSource.CreateStringSegmentFromSameOrigin(start, length);

                case StringSourceUnionValueType.ShapedString:
                case StringSourceUnionValueType.ShapedStringBuilder:
                case StringSourceUnionValueType.ShapedStringSegment:
                    throw new NotSupportedException();

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure that represents this string source.
        /// </summary>
        /// <returns>The <see cref="ShapedStringSegment"/> that was created.</returns>
        public ShapedStringSegment CreateShapedStringSegment()
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                case StringSourceUnionValueType.StringBuilder:
                case StringSourceUnionValueType.StringSegment:
                    throw new NotSupportedException();

                case StringSourceUnionValueType.ShapedString:
                    return ShapedStringSource.CreateShapedStringSegment();

                case StringSourceUnionValueType.ShapedStringBuilder:
                    return ShapedStringBuilderSource.CreateShapedStringSegment();

                case StringSourceUnionValueType.ShapedStringSegment:
                    return ShapedStringSegmentSource.CreateShapedStringSegment();

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure that represents a substring of
        /// this string source.
        /// </summary>
        /// <param name="start">The index of the first character in the substring that will 
        /// be represented by the string segment.</param>
        /// <param name="length">The length of the substring that will be represented by 
        /// the string segment.</param>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        public ShapedStringSegment CreateShapedStringSegmentFromSubstring(Int32 start, Int32 length)
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                case StringSourceUnionValueType.StringBuilder:
                case StringSourceUnionValueType.StringSegment:
                    throw new NotSupportedException();

                case StringSourceUnionValueType.ShapedString:
                    return ShapedStringSource.CreateShapedStringSegmentFromSubstring(start, length);

                case StringSourceUnionValueType.ShapedStringBuilder:
                    return ShapedStringBuilderSource.CreateShapedStringSegmentFromSubstring(start, length);

                case StringSourceUnionValueType.ShapedStringSegment:
                    return ShapedStringSegmentSource.CreateShapedStringSegmentFromSubstring(start, length);

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure with the same origin as this 
        /// string source but a different character range. This method only differs from
        /// the <see cref="CreateShapedStringSegmentFromSubstring(Int32, Int32)"/> method if this
        /// string source represents a substring of some other, larger string.
        /// </summary>
        /// <param name="start">The index of the first character in the created segment.</param>
        /// <param name="length">The number of characters in the created segment.</param>
        /// <returns>The <see cref="ShapedStringSegment"/> structure that was created.</returns>
        public ShapedStringSegment CreateShapedStringSegmentFromSameOrigin(Int32 start, Int32 length)
        {
            switch (ValueType)
            {
                case StringSourceUnionValueType.String:
                case StringSourceUnionValueType.StringBuilder:
                case StringSourceUnionValueType.StringSegment:
                    throw new NotSupportedException();

                case StringSourceUnionValueType.ShapedString:
                    return ShapedStringSource.CreateShapedStringSegmentFromSameOrigin(start, length);

                case StringSourceUnionValueType.ShapedStringBuilder:
                    return ShapedStringBuilderSource.CreateShapedStringSegmentFromSameOrigin(start, length);

                case StringSourceUnionValueType.ShapedStringSegment:
                    return ShapedStringSegmentSource.CreateShapedStringSegmentFromSameOrigin(start, length);

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// A <see cref="StringSourceUnionValueType"/> value which indicates the kind of source
        /// which is represented by this object.
        /// </summary>
        [FieldOffset(0)]
        public StringSourceUnionValueType ValueType;

        /// <summary>
        /// A value indicating whether the source contains shaped text.
        /// </summary>
        [FieldOffset(4)]
        public Boolean IsShaped;

        /// <summary>
        /// The <see cref="Core.Text.StringSource"/> value which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public StringSource StringSource;

        /// <summary>
        /// The <see cref="Core.Text.StringBuilderSource"/> value which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public StringBuilderSource StringBuilderSource;

        /// <summary>
        /// The <see cref="Core.Text.StringSegmentSource"/> value which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public StringSegmentSource StringSegmentSource;

        /// <summary>
        /// The <see cref="ShapedString"/> value which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public ShapedString ShapedStringSource;

        /// <summary>
        /// The <see cref="ShapedStringBuilder"/> which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public ShapedStringBuilder ShapedStringBuilderSource;

        /// <summary>
        /// The <see cref="ShapedStringSegment"/> which is represented by this object.
        /// </summary>
        [FieldOffset(8)]
        public ShapedStringSegment ShapedStringSegmentSource;
    }
}
