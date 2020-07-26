using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes parameters for a standard lighting setup.
    /// </summary>
    public interface IEffectLights
    {
        /// <summary>
        /// Gets or sets a value indicating whether lighting is enabled for this effect.
        /// </summary>
        Boolean LightingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the material's ambient light color.
        /// </summary>
        Color AmbientLightColor { get; set; }

        /// <summary>
        /// Gets or sets the material's first directional light.
        /// </summary>
        DirectionalLight DirectionalLight0 { get; }

        /// <summary>
        /// Gets or sets the material's second directional light.
        /// </summary>
        DirectionalLight DirectionalLight1 { get; }

        /// <summary>
        /// Gets or sets the material's third directional light.
        /// </summary>
        DirectionalLight DirectionalLight2 { get; }
    }
}
