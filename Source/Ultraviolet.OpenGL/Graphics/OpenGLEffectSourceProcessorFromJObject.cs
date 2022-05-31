using Newtonsoft.Json.Linq;
using System;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads effect source assets from JSON definition files.
    /// </summary>
    [ContentProcessor]
    public class OpenGLEffectSourceProcessorFromJObject : ContentProcessor<JObject, EffectSource>
    {
        /// <inheritdoc/>
        public override EffectSource Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            return new OpenGLEffectSource(OpenGLEffectSourceAssetType.JObject, manager, metadata, input);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => false;
    }
}
