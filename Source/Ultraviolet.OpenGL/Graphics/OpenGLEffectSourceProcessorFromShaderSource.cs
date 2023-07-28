using Newtonsoft.Json.Linq;
using System;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads effect source assets from shader source files.
    /// </summary>
    [ContentProcessor]
    public class OpenGLEffectSourceProcessorFromShaderSource : ContentProcessor<String, EffectSource>
    {
        /// <inheritdoc/>
        public override EffectSource Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new OpenGLEffectSource(OpenGLEffectSourceAssetType.ShaderSource, manager, metadata, input);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => false;
    }
}
