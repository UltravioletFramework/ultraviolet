using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a vertex containing 3D position and texture coordinate data.
    /// </summary>
    public struct VertexPositionTexture : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionTexture"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="textureCoordinate">The texture coordinate.</param>
        public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
        {
            this.Position = position;
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
            new VertexElement(sizeof(Single) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
        });

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The texture coordinate.
        /// </summary>
        public Vector2 TextureCoordinate;
    }
}
