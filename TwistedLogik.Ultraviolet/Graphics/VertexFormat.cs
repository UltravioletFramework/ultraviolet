
namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the supported formats for vertex data.
    /// </summary>
    public enum VertexFormat
    {
        /// <summary>
        /// 32-bit floating point with one component.
        /// </summary>
        Single,

        /// <summary>
        /// 32-bit floating point with two components.
        /// </summary>
        Vector2,

        /// <summary>
        /// 32-bit floating point with three components.
        /// </summary>
        Vector3,

        /// <summary>
        /// 32-bit floating point with three components.
        /// </summary>
        Vector4,

        /// <summary>
        /// 8-bit unsigned byte with four components.
        /// </summary>
        Color,

        /// <summary>
        /// Packed vector containing two 16-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedShort2,

        /// <summary>
        /// Packed vector containing four 16-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedShort4,

        /// <summary>
        /// Packed vector containing two 16-bit signed normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort2,

        /// <summary>
        /// Packed vector containing four 16-bit signed normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort4,

        /// <summary>
        /// Packed vector containing two 16-bit signed normalized integer values.
        /// </summary>
        Short2,

        /// <summary>
        /// Packed vector containing two 16-bit signed normalized integer values.
        /// </summary>
        Short4,

        /// <summary>
        /// Packed vector containing two 16-bit unsigned normalized integer values.
        /// </summary>
        UnsignedShort2,

        /// <summary>
        /// Packed vector containing four 16-bit unsigned normalized integer values.
        /// </summary>
        UnsignedShort4,
    }
}
