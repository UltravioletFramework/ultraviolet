using System;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains metadata for OpenGLTexture2DProcessor.
    /// </summary>
    internal sealed class OpenGLTexture2DProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2DProcessorMetadata class.
        /// </summary>
        public OpenGLTexture2DProcessorMetadata()
        {
            PremultiplyAlpha = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to premultiply the texture's alpha.
        /// </summary>
        public Boolean PremultiplyAlpha { get; private set; }
    }
}
