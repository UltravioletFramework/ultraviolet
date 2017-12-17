using System;
using Newtonsoft.Json;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains metadata for OpenGLTexture3DProcessor.
    /// </summary>
    internal sealed class OpenGLTexture3DProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3DProcessorMetadata"/> class.
        /// </summary>
        public OpenGLTexture3DProcessorMetadata()
        {
            PremultiplyAlpha = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3DProcessorMetadata"/> class.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the texture's alpha.</param>
        [JsonConstructor]
        public OpenGLTexture3DProcessorMetadata(Boolean premultiplyAlpha)
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
