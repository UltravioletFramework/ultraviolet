using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
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
            Contract.Require(elements, "elements");

            this.elements = elements.ToList();
            this.stride = CalculateStride();
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
        public Int32 VertexStride
        {
            get { return stride; }        
        }

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
                    case VertexFormat.Single:
                        value += sizeof(Single);
                        break;

                    case VertexFormat.Vector2:
                        value += sizeof(Single) * 2;
                        break;

                    case VertexFormat.Vector3:
                        value += sizeof(Single) * 3;
                        break;

                    case VertexFormat.Vector4:
                        value += sizeof(Single) * 4;
                        break;

                    case VertexFormat.Color:
                        value += sizeof(Byte) * 4;
                        break;

                    default:
                        throw new InvalidOperationException(UltravioletStrings.UnsupportedVertexFormat);
                }
            }
            return value;
        }

        // Property values.
        private readonly Int32 stride;

        // State values.
        private readonly List<VertexElement> elements;
    }
}
