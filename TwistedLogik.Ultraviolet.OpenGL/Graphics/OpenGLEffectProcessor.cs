using System;
using System.IO;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLEffectProcessor : ContentProcessor<XDocument, Effect>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            implProcessor.ExportPreprocessed(manager, metadata, writer, input, delete);
        }

        /// <inheritdoc/>
        public override Effect ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var impl = implProcessor.ImportPreprocessed(manager, metadata, reader);
            return Effect.Create(impl);
        }

        /// <inheritdoc/>
        public override Effect Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var impl = implProcessor.Process(manager, metadata, input);
            return Effect.Create(impl);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }

        // State values.
        private readonly OpenGLEffectImplementationProcessor implProcessor =
            new OpenGLEffectImplementationProcessor();
    }
}
