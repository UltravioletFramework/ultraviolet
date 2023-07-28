using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a vertex declaration, which defines the structure of vertex data.
    /// </summary>
    public partial class VertexDeclaration : IEnumerable<VertexElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        /// <param name="elements">The vertex declaration's elements.</param>
        public VertexDeclaration(IEnumerable<VertexElement> elements)
        {
            Contract.Require(elements, nameof(elements));

            this.elements = elements.ToList();

            foreach (var element in elements)
            {
                switch (element.Usage)
                {
                    case VertexElementUsage.Position:
                        HasPosition = true;
                        break;

                    case VertexElementUsage.Color:
                        HasColor = true;
                        break;

                    case VertexElementUsage.TextureCoordinate:
                        HasTexture = true;
                        break;

                    case VertexElementUsage.Normal:
                        HasNormal = true;
                        break;

                    case VertexElementUsage.Tangent:
                        HasTangent = true;
                        break;

                    case VertexElementUsage.BlendIndices:
                    case VertexElementUsage.BlendWeight:
                        HasBlend = true;
                        break;
                }
            }

            this.VertexStride = CalculateStride();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the vertex declaration.
        /// </summary>
        /// <returns>An enumerator that iterates through the vertex declaration.</returns>
        public List<VertexElement>.Enumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the vertex declaration.
        /// </summary>
        /// <returns>An enumerator that iterates through the vertex declaration.</returns>
        IEnumerator<VertexElement> IEnumerable<VertexElement>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the vertex declaration.
        /// </summary>
        /// <returns>An enumerator that iterates through the vertex declaration.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the vertex stride in bytes.
        /// </summary>
        public Int32 VertexStride { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any position elements.
        /// </summary>
        public Boolean HasPosition { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any color elements.
        /// </summary>
        public Boolean HasColor { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any texture elements.
        /// </summary>
        public Boolean HasTexture { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any normal elements.
        /// </summary>
        public Boolean HasNormal { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any tangent elements.
        /// </summary>
        public Boolean HasTangent { get; }

        /// <summary>
        /// Gets a value indicating whether this declaration has any blend elements.
        /// </summary>
        public Boolean HasBlend { get; }

        /// <summary>
        /// Calculates the stride of a vertex in bytes.
        /// </summary>
        /// <returns>The stride of a vertex in bytes.</returns>
        private Int32 CalculateStride()
        {
            var value = 0;
            foreach (var element in elements)
            {
                switch (element.Format)
                {
                    case VertexElementFormat.Single:
                        value += sizeof(Single);
                        break;

                    case VertexElementFormat.Vector2:
                        value += sizeof(Single) * 2;
                        break;

                    case VertexElementFormat.Vector3:
                        value += sizeof(Single) * 3;
                        break;

                    case VertexElementFormat.Vector4:
                        value += sizeof(Single) * 4;
                        break;

                    case VertexElementFormat.Color:
                        value += sizeof(Byte) * 4;
                        break;

                    case VertexElementFormat.Short2:
                    case VertexElementFormat.UnsignedShort2:
                    case VertexElementFormat.NormalizedShort2:
                    case VertexElementFormat.NormalizedUnsignedShort2:
                        value += sizeof(Int16) * 2;
                        break;

                    case VertexElementFormat.Short4:
                    case VertexElementFormat.UnsignedShort4:
                    case VertexElementFormat.NormalizedShort4:
                    case VertexElementFormat.NormalizedUnsignedShort4:
                        value += sizeof(Int16) * 4;
                        break;
						
                    case VertexElementFormat.Byte4:
                        value += sizeof(Byte) * 4;
                        break;

                    default:
                        throw new InvalidOperationException(UltravioletStrings.UnsupportedVertexFormat);
                }
            }
            return value;
        }

        // State values.
        private readonly List<VertexElement> elements;
    }
}
