using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial struct ShapedStringSegment
    {
        /// <inheritdoc/>
        public void GetChar(Int32 index, out ShapedChar ch) => ch = this[index];

        /// <inheritdoc/>
        public Boolean IsNull => false;

        /// <inheritdoc/>
        public Boolean IsEmpty => Length == 0;

        /// <inheritdoc/>
        public ShapedStringSegment CreateShapedStringSegment() =>
            new ShapedStringSegment(this, 0, Length);

        /// <inheritdoc/>
        public ShapedStringSegment CreateShapedStringSegmentFromSubstring(Int32 start, Int32 length) =>
            new ShapedStringSegment(this, start, length);

        /// <inheritdoc/>
        public ShapedStringSegment CreateShapedStringSegmentFromSameOrigin(Int32 start, Int32 length) =>
            new ShapedStringSegment(this, start, length);
    }
}
