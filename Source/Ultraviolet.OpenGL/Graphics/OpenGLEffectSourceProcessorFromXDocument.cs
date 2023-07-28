using Newtonsoft.Json.Linq;
using System;
using Ultraviolet.Content;
using System.Xml.Linq;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads effect source assets from XML definition files.
    /// </summary>
    [ContentProcessor]
    public class OpenGLEffectSourceProcessorFromXDocument : ContentProcessor<XDocument, EffectSource>
    {
        /// <inheritdoc/>
        public override EffectSource Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            return new OpenGLEffectSource(OpenGLEffectSourceAssetType.XDocument, manager, metadata, input);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => false;
    }
}
