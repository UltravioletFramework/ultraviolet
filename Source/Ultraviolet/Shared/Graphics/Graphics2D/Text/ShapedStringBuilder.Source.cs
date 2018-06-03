using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class ShapedStringBuilder
    {
        /// <inheritdoc/>
        public void GetChar(Int32 index, out ShapedChar ch)
        {
            Contract.EnsureRange(index < Length, nameof(index));

            ch = buffer[index];
        }

        /// <inheritdoc/>
        public Boolean IsNull => false;

        /// <inheritdoc/>
        public Boolean IsEmpty => Length == 0;
    }
}
