using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the set of options which can be used to modify the behavior of a <see cref="Texture2D"/> or <see cref="Texture3D"/>.
    /// </summary>
    [Flags]
    public enum TextureOptions
    {
        /// <summary>
        /// The render buffer has no special options.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Specifies that the render buffer should use immutable storage, if it is available.
        /// </summary>
        ImmutableStorage = 0x01,

        /// <summary>
        /// Specifies that the texture will contain linearly encoded color data.
        /// Mutually exclusive with <see cref="SrgbColor"/>.
        /// </summary>
        LinearColor = 0x02,

        /// <summary>
        /// Specifies that the texture will contain sRGB encoded color data.
        /// Mutually exclusive with <see cref="LinearColor"/>.
        /// </summary>
        SrgbColor = 0x04,

        /// <summary>
        /// The default options for a texture.
        /// </summary>
        Default = ImmutableStorage,
    }
}
