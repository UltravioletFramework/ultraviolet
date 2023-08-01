
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Provides usage hints as to the intended purpose of a vertex element.
    /// </summary>
    public enum VertexElementUsage
    {
        /// <summary>
        /// The element provides vertex position data.
        /// </summary>
        Position,

        /// <summary>
        /// The element provides color data.
        /// </summary>
        Color,

        /// <summary>
        /// The element provides texture coordinate data.
        /// </summary>
        TextureCoordinate,

        /// <summary>
        /// The element provides normal data.
        /// </summary>
        Normal,

        /// <summary>
        /// The element provides tangent data.
        /// </summary>
        Tangent,

        /// <summary>
        /// The element provides blending indices data.
        /// </summary>
        BlendIndices,

        /// <summary>
        /// The element providces blending weight data.
        /// </summary>
        BlendWeight
    }
}
