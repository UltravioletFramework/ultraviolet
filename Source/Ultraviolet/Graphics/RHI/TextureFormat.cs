using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the set of texture formats supported by <see cref="Texture2D"/> or <see cref="Texture3D"/>.
    /// </summary>
    public enum TextureFormat
    {
        /// <summary>
        /// The texture data is 3 components, stored in RGB order.
        /// </summary>
        RGB,

        /// <summary>
        /// The texture data is 3 components stored in BGR order.
        /// </summary>
        BGR,

        /// <summary>
        /// The texture data is 4 components, stored in RGBA order.
        /// </summary>
        RGBA,

        /// <summary>
        /// The texture data is 4 components stored in BGRA order.
        /// </summary>
        BGRA
    }
}
