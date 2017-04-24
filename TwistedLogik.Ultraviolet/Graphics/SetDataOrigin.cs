
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the origin of texture data which is being set onto a texture or render buffer.
    /// If the data source's origin differs from the texture's expected origin, the buffer may be
    /// re-arranged during the copy in order to match what the texture expects.
    /// </summary>
    public enum SetDataOrigin
    {
        /// <summary>
        /// The buffer being copied contains data with a top-left origin.
        /// </summary>
        TopLeft,

        /// <summary>
        /// The buffer being copied contains data with a top-right origin.
        /// </summary>
        TopRight,

        /// <summary>
        /// The buffer being copied contains data with a bottom-left origin.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// The buffer being copied contains data with a bottom-right origin.
        /// </summary>
        BottomRight,
    }
}
