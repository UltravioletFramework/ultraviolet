using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the set of options which can be used to modify the behavior of a <see cref="Surface2D"/> or <see cref="Surface3D"/>.
    /// </summary>
    [Flags]
    public enum SurfaceOptions
    {
        /// <summary>
        /// The surface has no special options.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Specifies that the surface will contain linearly encoded color data.
        /// Mutually exclusive with <see cref="SrgbColor"/>.
        /// </summary>
        LinearColor = 0x01,

        /// <summary>
        /// Specifies that the surface will contain sRGB encoded color data.
        /// Mutually exclusive with <see cref="LinearColor"/>.
        /// </summary>
        SrgbColor = 0x02,

        /// <summary>
        /// The default options for a surface.
        /// </summary>
        Default = None,
    }
}
