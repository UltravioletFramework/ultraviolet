namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// An intermediate representation of a single triangle in an STL model file.
    /// </summary>
    public class StlModelTriangleDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StlModelTriangleDescription"/> class.
        /// </summary>
        /// <param name="normal">The triangle's normal vector.</param>
        /// <param name="v1">The position of the triangle's first vertex.</param>
        /// <param name="v2">The position of the triangle's second vertex.</param>
        /// <param name="v3">The position of the triangle's third vertex.</param>
        public StlModelTriangleDescription(Vector3 normal, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.Normal = normal;
            this.Vertex1 = v1;
            this.Vertex2 = v2;
            this.Vertex3 = v3;
        }

        /// <summary>
        /// Gets or sets the triangle's normal vector.
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Gets or sets the position of the triangle's first vertex.
        /// </summary>
        public Vector3 Vertex1 { get; set; }

        /// <summary>
        /// Gets or sets the position of the triangle's second vertex.
        /// </summary>
        public Vector3 Vertex2 { get; set; }

        /// <summary>
        /// Gets or sets the position of the triangle's third vertex.
        /// </summary>
        public Vector3 Vertex3 { get; set; }
    }
}
