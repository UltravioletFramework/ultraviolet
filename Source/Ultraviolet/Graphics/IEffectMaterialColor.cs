using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes parameters for representing a material color.
    /// </summary>
    public interface IEffectMaterialColor
    {
        /// <summary>
        /// Gets or sets a value indicating whether this effect should use sRGB color.
        /// </summary>
        Boolean SrgbColor { get; set; }

        /// <summary>
        /// Gets or sets the material's alpha value.
        /// </summary>
        Single Alpha { get; set; }

        /// <summary>
        /// Gets or sets the material's diffuse color.
        /// </summary>
        Color DiffuseColor { get; set; }

        /// <summary>
        /// Gets or sets the material's emissive color.
        /// </summary>
        Color EmissiveColor { get; set; }

        /// <summary>
        /// Gets or sets the material's specular color.
        /// </summary>
        Color SpecularColor { get; set; }

        /// <summary>
        /// Gets or sets the material's specular power.
        /// </summary>
        Single SpecularPower { get; set; }
    }
}
