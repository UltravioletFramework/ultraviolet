using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes parameters for rendering fog.
    /// </summary>
    public interface IEffectFog
    {
        /// <summary>
        /// Gets or sets a value indicating whether fog is enabled for this effect.
        /// </summary>
        Boolean FogEnabled { get; set; }

        /// <summary>
        /// Gets or sets the material's fog color.
        /// </summary>
        Color FogColor { get; set; }

        /// <summary>
        /// Gets or sets the minimum z-value for fog.
        /// </summary>
        Single FogStart { get; set; }

        /// <summary>
        /// Gets or sets the maximum z-value for fog.
        /// </summary>
        Single FogEnd { get; set; }
    }
}
