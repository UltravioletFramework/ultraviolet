
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the supported primitive types, which specify how vertex data is ordered within a buffer.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// The vertex data is ordered as a sequence of triangles, with each triangle described by three new vertices.
        /// </summary>
        TriangleList,

        /// <summary>
        /// The vertex data is ordered as a sequence of triangles, with each triangle described by three new vertices
        /// and one vertex from the previous triangle.
        /// </summary>
        TriangleStrip,

        /// <summary>
        /// The vertex data is ordered as a sequence of line segments, with each line segment described by two new vertices.
        /// </summary>
        LineList,

        /// <summary>
        /// The vertex data is ordered as a sequence of line segments, with each line segment described by one new vertex
        /// and one vertex from the previous line segment.
        /// </summary>
        LineStrip,
    }
}
