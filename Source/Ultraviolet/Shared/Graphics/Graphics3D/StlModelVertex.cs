using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a vertex in an STL model.
    /// </summary>
    public struct StlModelVertex : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StlModelVertex"/> structure.
        /// </summary>
        /// <param name="position">The vertex's position.</param>
        /// <param name="normal">The vertex's normal vector.</param>
        public StlModelVertex(Vector3 position, Vector3 normal)
        {
            this.Position = position;
            this.Normal = normal;
        }

        /// <inheritdoc/>
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// The vertex declaration.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new[] {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(Single) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        });

        /// <summary>
        /// The vertex's position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The vertex's normal vector.
        /// </summary>
        public Vector3 Normal;
    }
}
