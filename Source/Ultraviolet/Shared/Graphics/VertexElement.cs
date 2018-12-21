using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
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
        /// <param name="name">The element's name in shaders, or <see langword="null"/> to use an imlpementation-specific default name.</param>
        public VertexElement(Int32 position, VertexElementFormat format, VertexElementUsage usage, Int32 index, String name = null)
        {
            Contract.EnsureRange(index >= 0 && index < UsageIndexCount, nameof(index));

            this.Name = name;
            this.Position = position;
            this.Index = index;
            this.Format = format;
            this.Usage = usage;
        }

        /// <summary>
        /// Gets the maximum number of vertex usage indices allowed by the Ultraviolet Framework.
        /// </summary>
        public static Int32 UsageIndexCount { get; } = 16;

        /// <summary>
        /// Gets the name which is used to identify this vertex element in shaders, if one has been specified.
        /// If no name is specified, an implementation-specific default is used.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the element's position within its vertex data, in bytes.
        /// </summary>
        public Int32 Position { get; }

        /// <summary>
        /// Gets the element's usage index.
        /// </summary>
        public Int32 Index { get; }

        /// <summary>
        /// Gets the element's vertex format.
        /// </summary>
        public VertexElementFormat Format { get; }

        /// <summary>
        /// Gets the element's usage hint.
        /// </summary>
        public VertexElementUsage Usage { get; }
    }
}
