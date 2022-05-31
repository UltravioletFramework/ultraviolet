using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a vertex containing 3D position and color data.
    /// </summary>
    public struct VertexPositionColor : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColor"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="color">The vertex color.</param>
        public VertexPositionColor(Vector3 position, Color color)
        {
            this.Position = position;
            this.Color = color;
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
            new VertexElement(sizeof(Single) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        });

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The vertex color.
        /// </summary>
        public Color Color;
    }
}
