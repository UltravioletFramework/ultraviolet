using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a single element in a vertex declaration.
    /// </summary>
    public struct VertexElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement"/> structure.
        /// </summary>
        /// <param name="position">The element's position within the vertex data, in bytes.</param>
        /// <param name="format">The element's vertex format.</param>
        /// <param name="usage">The element's usage hint.</param>
        /// <param name="index">The element's usage index.</param>
        public VertexElement(Int32 position, VertexFormat format, VertexUsage usage, Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < UsageIndexCount, "index");

            this.position = position;
            this.format = format;
            this.usage = usage;
            this.index = index;
        }

        /// <summary>
        /// Gets the maximum number of vertex usage indices allowed by the Ultraviolet Framework.
        /// </summary>
        public static Int32 UsageIndexCount
        {
            get { return 16; }
        }

        /// <summary>
        /// Gets the element's position within its vertex data, in bytes.
        /// </summary>
        public Int32 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the element's vertex format.
        /// </summary>
        public VertexFormat Format
        {
            get { return format; }
        }

        /// <summary>
        /// Gets the element's usage hint.
        /// </summary>
        public VertexUsage Usage
        {
            get { return usage; }
        }

        /// <summary>
        /// Gets the element's usage index.
        /// </summary>
        public Int32 Index
        {
            get { return index; }
        }

        // Property values.
        private readonly Int32 position;
        private readonly VertexFormat format;
        private readonly VertexUsage usage;
        private readonly Int32 index;
    }
}
