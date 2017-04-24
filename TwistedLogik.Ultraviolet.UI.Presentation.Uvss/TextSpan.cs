using System;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents a span of text in source code.
    /// </summary>
    public struct TextSpan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextSpan"/> structure.
        /// </summary>
        /// <param name="start">The offset in characters from the beginning of the source
        /// text to the beginning of the span.</param>
        /// <param name="length">The span's length in characters.</param>
        public TextSpan(Int32 start, Int32 length)
        {
            this.Start = start;
            this.Length = length;
        }

        /// <summary>
        /// Gets the offset in characters from the beginning of the source text
        /// to the beginning of the span.
        /// </summary>
        public Int32 Start { get; }

        /// <summary>
        /// Gets the span's length in characters.
        /// </summary>
        public Int32 Length { get; }
    }
}
