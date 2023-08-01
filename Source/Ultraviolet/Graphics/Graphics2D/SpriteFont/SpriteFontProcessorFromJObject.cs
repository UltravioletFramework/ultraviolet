using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Loads sprite font assets.
    /// </summary>
    [ContentProcessor]
    internal sealed class SpriteFontProcessorFromJObject : ContentProcessor<JObject, SpriteFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, JObject input, Boolean delete) =>
            implProcessor.ExportPreprocessed(manager, metadata, writer, CreateSpriteFontDescription(manager, metadata, input), delete);

        /// <inheritdoc/>
        public override SpriteFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            implProcessor.ImportPreprocessed(manager, metadata, reader);

        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input) =>
            implProcessor.Process(manager, metadata, CreateSpriteFontDescription(manager, metadata, input));

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => implProcessor.SupportsPreprocessing;

        /// <summary>
        /// Creates a <see cref="SpriteFontDescription"/> from the specified input file.
        /// </summary>
        private static SpriteFontDescription CreateSpriteFontDescription(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
            return input.ToObject<SpriteFontDescription>(serializer);
        }
        
        // The internal processor which converts SpriteFontDescription -> SpriteFont.
        private readonly SpriteFontProcessor implProcessor = new SpriteFontProcessor();
    }
}
        