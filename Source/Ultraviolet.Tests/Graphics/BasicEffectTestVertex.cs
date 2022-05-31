using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.Tests.Graphics
{
    /// <summary>
    /// Represents a custom vertex type used by the <see cref="BasicEffect"/> tests.
    /// </summary>
    public struct BasicEffectTestVertex : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEffectTestVertex"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="textureCoordinate">The vertex texture coordinate.</param>
        /// <param name="color">The vertex color.</param>
        public BasicEffectTestVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate, Color color)
        {
            this.Position = position;
            this.Normal = normal;
            this.TextureCoordinate = textureCoordinate;
            this.Color = color;
        }

        /// <inheritdoc/>
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// The vertex declaration.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new[] {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(Single) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(Single) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(Single) * 8, VertexElementFormat.Color, VertexElementUsage.Color, 0)
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
        /// The vertex texture coordinate.
        /// </summary>
        public Vector2 TextureCoordinate;

        /// <summary>
        /// The vertex color.
        /// </summary>
        public Color Color;
    }
}
