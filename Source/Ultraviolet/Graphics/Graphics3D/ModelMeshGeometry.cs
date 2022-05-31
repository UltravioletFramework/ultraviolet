using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents part of a <see cref="ModelMesh"/> instance's 3D geometry and rendering state.
    /// </summary>
    public sealed class ModelMeshGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMeshGeometry"/> class.
        /// </summary>
        /// <param name="primitiveType">The type of primitive contained by this object.</param>
        /// <param name="geometryStream">The geometry stream that contains the vertex and index data.</param>
        /// <param name="vertexCount">The number of vertices in this geometry object.</param>
        /// <param name="indexCount">The number of indices in this geometry object.</param>
        /// <param name="material">The material with which to draw the geometry.</param>
        public ModelMeshGeometry(PrimitiveType primitiveType, GeometryStream geometryStream, Int32 vertexCount, Int32 indexCount, Material material)
        {
            Contract.Require(geometryStream, nameof(geometryStream));

            this.PrimitiveType = primitiveType;
            this.GeometryStream = geometryStream;
            this.VertexCount = vertexCount;
            this.IndexCount = indexCount;
            this.Material = material;

            var elementCount = (indexCount == 0) ? vertexCount : indexCount;
            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                    this.PrimitiveCount = elementCount / 3;
                    break;

                case PrimitiveType.TriangleStrip:
                    this.PrimitiveCount = elementCount - 2;
                    break;

                case PrimitiveType.LineList:
                    this.PrimitiveCount = elementCount / 2;
                    break;

                case PrimitiveType.LineStrip:
                    this.PrimitiveCount = elementCount - 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveType));
            }
        }

        /// <summary>
        /// Gets the type of primitive contained by this object.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Gets the geometry stream that contains the vertex and index data.
        /// </summary>
        public GeometryStream GeometryStream { get; }

        /// <summary>
        /// Gets the number of vertices in this geometry object.
        /// </summary>
        public Int32 VertexCount { get; }

        /// <summary>
        /// Gets the number of indices in this geometry object.
        /// </summary>
        public Int32 IndexCount { get; }

        /// <summary>
        /// Gets the number of primitives in this geometry object.
        /// </summary>
        public Int32 PrimitiveCount { get; }

        /// <summary>
        /// Gets or sets the material used to draw the geometry.
        /// </summary>
        public Material Material { get; set; }
    }
}