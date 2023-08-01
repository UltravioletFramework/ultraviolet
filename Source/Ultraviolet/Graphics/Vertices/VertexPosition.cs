namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a vertex containing only 3D position data.
    /// </summary>
    public struct VertexPosition : IVertexType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosition"/> structure.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        public VertexPosition(Vector3 position)
        {
            this.Position = position;
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
        });

        /// <summary>
        /// The vertex position.
        /// </summary>
        public Vector3 Position;
    }
}
