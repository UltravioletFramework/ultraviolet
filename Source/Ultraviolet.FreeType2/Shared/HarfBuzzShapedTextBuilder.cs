using System;
using System.Text;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents an implementation of the <see cref="ShapedTextBuilder"/> class based on the HarfBuzz library.
    /// </summary>
    public sealed class HarfBuzzShapedTextBuilder : ShapedTextBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarfBuzzShapedTextBuilder"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public HarfBuzzShapedTextBuilder(UltravioletContext uv)
            : base(uv)
        { }

        /// <inheritdoc/>
        public override void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void AppendUtf16(String str)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void AppendUtf16(String str, Int32 start, Int32 length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void AppendUtf16(StringBuilder str)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void AppendUtf16(StringBuilder str, Int32 start, Int32 length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void AppendUtf16(StringSegment str)
        {
            throw new NotImplementedException();
        }
    }
}
