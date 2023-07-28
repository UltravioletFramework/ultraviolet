using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents what type of string source is being represented by a <see cref="StringSourceUnion"/> structure.
    /// </summary>
    internal enum StringSourceUnionValueType
    {
        /// <summary>
        /// The union represents no value.
        /// </summary>
        None,

        /// <summary>
        /// The union represents a <see cref="StringSource"/> instance.
        /// </summary>
        String,

        /// <summary>
        /// The union represents a <see cref="StringBuilderSource"/> instance.
        /// </summary>
        StringBuilder,

        /// <summary>
        /// The union represents a <see cref="StringSegmentSource"/> instance.
        /// </summary>
        StringSegment,

        /// <summary>
        /// The union represents a <see cref="ShapedString"/> instance.
        /// </summary>
        ShapedString,

        /// <summary>
        /// The union represents a <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        ShapedStringBuilder,

        /// <summary>
        /// The union represents a <see cref="ShapedStringSegment"/> instance.
        /// </summary>
        ShapedStringSegment,
    }
}
