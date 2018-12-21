using Ultraviolet.Graphics;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents the vertex format used by ImGui.
    /// </summary>
    public struct ImGuiVertex : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiVertex"/> structure.
        /// </summary>
        /// <param name="position">The vertex's position.</param>
        /// <param name="textureCoordinate">The vertex's texture coordinate.</param>
        /// <param name="color">The vertex's color.</param>
        public ImGuiVertex(Vector2 position, Vector2 textureCoordinate, Color color)
        {
            this.Position = position;
            this.TextureCoordinate = textureCoordinate;
            this.Color = color;
        }

        /// <inheritdoc/>
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// The vertex declaration for this vertex type.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new[] {
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0),
        });

        /// <summary>
        /// The vertex's position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The vertex's texture coordinate.
        /// </summary>
        public Vector2 TextureCoordinate;

        /// <summary>
        /// The vertex's color.
        /// </summary>
        public Color Color;
    }
}
