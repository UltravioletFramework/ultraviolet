
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the supported render buffer formats.
    /// </summary>
    public enum RenderBufferFormat
    {
        /// <summary>
        /// The buffer contains 32 bits of color data.
        /// </summary>
        Color,

        /// <summary>
        /// The buffer contains 24 bits of depth data and 8 bits of stencil data.
        /// </summary>
        Depth24Stencil8,

        /// <summary>
        /// The buffer contains 32 bits of depth data.
        /// </summary>
        Depth32,        

        /// <summary>
        /// The buffer contains 16 bits of depth data.
        /// </summary>
        Depth16,

        /// <summary>
        /// The buffer contains 8 bits of stencil data.
        /// </summary>
        Stencil8,
    }
}
