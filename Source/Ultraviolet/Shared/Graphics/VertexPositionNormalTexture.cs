using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a vertex containing 3D position, normal, and texture coordinate data.
    /// </summary>
    public struct VertexPositionNormalTexture : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionNormalTexture"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="textureCoordinate">The texture coordinate.</param>
        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            this.Position = position;
            this.Normal = normal;
            this.TextureCoordinate = textureCoordinate;
        }

        /// <summary>
        /// Gets the vertex declaration.
        /// </summary>
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        /// <summary>
        /// The vertex declaration.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new[] {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(Single) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(Single) * 3 + sizeof(Single) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
        });

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The vertex normal.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// The texture coordinate.
        /// </summary>
        public Vector2 TextureCoordinate;
    }
}