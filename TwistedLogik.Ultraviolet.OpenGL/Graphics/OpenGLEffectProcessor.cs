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
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input)
        {
            implProcessor.ExportPreprocessed(manager, metadata, writer, input);
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        public override Effect ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var impl = implProcessor.ImportPreprocessed(manager, metadata, reader);
            return Effect.Create(impl);
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Effect Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var impl = implProcessor.Process(manager, metadata, input);
            return Effect.Create(impl);
        }

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }

        // State values.
        private readonly OpenGLEffectImplementationProcessor implProcessor =
            new OpenGLEffectImplementationProcessor();
    }
}
