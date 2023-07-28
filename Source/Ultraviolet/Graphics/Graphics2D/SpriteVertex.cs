using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a vertex containing 2D position, color, and texture coordinate data, which is
    /// used by the <see cref="Ultraviolet.Graphics.Graphics2D.SpriteBatch"/> class
    /// to render standard sprites.
    /// </summary>
    public struct SpriteVertex : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColorTexture"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="color">The vertex color.</param>
        /// <param name="u">The u component of the vertex's texture coordinate.</param> 
        /// <param name="v">The v component of the vertex's texture coordinate.</param> 
        [CLSCompliant(false)]
        public SpriteVertex(Vector2 position, Color color, UInt16 u, UInt16 v)
        {
            this.Position = position;
            this.Color = color;
            this.U = u;
            this.V = v;
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
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(Single) * 2, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(Single) * 2 + sizeof(Byte) * 4, VertexElementFormat.UnsignedShort2, VertexElementUsage.TextureCoordinate, 0),
        });

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The vertex color.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The u-component of the texture coordinate.
        /// </summary>
        [CLSCompliant(false)]
        public UInt16 U;

        /// <summary>
        /// The v-component of the texture coordinate.
        /// </summary>
        [CLSCompliant(false)]
        public UInt16 V;
    }
}
