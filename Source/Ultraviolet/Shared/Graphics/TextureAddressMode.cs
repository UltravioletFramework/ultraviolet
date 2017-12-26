
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the supported texture addressing modes.
    /// </summary>
    public enum TextureAddressMode
    {
        /// <summary>
        /// Clamps texture coordinates to the range [0.0, 1.0].
        /// </summary>
        Clamp,

        /// <summary>
        /// Wraps texture coordinates which exceed the range [0.0, 1.0].
        /// </summary>
        Wrap,

        /// <summary>
        /// Mirrors texture coordinates at every integer junction.
        /// </summary>
        Mirror,
    }
}
