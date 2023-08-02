using System;
using Newtonsoft.Json;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Contains metadata for Texture3DProcessor.
    /// </summary>
    internal sealed class Texture3DProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DProcessorMetadata"/> class.
        /// </summary>
        public Texture3DProcessorMetadata()
        {
            PremultiplyAlpha = true;
            Opaque = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DProcessorMetadata"/> class.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the texture's alpha.</param>
        /// <param name="opaque">A value indicating whether the texture is opaque and color keying should be disabled.</param>
        [JsonConstructor]
        public Texture3DProcessorMetadata(Boolean premultiplyAlpha, Boolean opaque)
        {
            PremultiplyAlpha = premultiplyAlpha;
            Opaque = opaque;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to premultiply the texture's alpha.
        /// </summary>
        public Boolean PremultiplyAlpha { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the texture is opaque and color keying should be disabled.
        /// </summary>
        public Boolean Opaque { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the texture is SRGB encoded. If no value is provided,
        /// the default specified by the <see cref="UltravioletContextProperties.SrgbDefaultForTexture3D"/>
        /// property is used.
        /// </summary>
        public Boolean? SrgbEncoded { get; private set; }
    }
}
