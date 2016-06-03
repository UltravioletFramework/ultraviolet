using System;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains metadata for OpenGLTexture2DProcessor.
    /// </summary>
    internal sealed class OpenGLTexture2DProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture2DProcessorMetadata"/> class.
        /// </summary>
        public OpenGLTexture2DProcessorMetadata()
        {
            PremultiplyAlpha = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture2DProcessorMetadata"/> class.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the texture's alpha.</param>
        [JsonConstructor]
        public OpenGLTexture2DProcessorMetadata(Boolean premultiplyAlpha)
        {
            PremultiplyAlpha = premultiplyAlpha;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to premultiply the texture's alpha.
        /// </summary>
        [JsonProperty(PropertyName = "premultiplyAlpha")]
        public Boolean PremultiplyAlpha { get; private set; }
    }
}
