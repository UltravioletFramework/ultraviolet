using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a content processor which loads XML definition files as texture atlases.
    /// </summary>
    [ContentProcessor]
    public sealed partial class TextureAtlasProcessorFromJObject : ContentProcessor<JObject, TextureAtlas>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, JObject input, Boolean delete) =>
            internalProcessor.ExportPreprocessed(manager, metadata, writer, CreateTextureAtlasDescription(input), delete);

        /// <inheritdoc/>
        public override TextureAtlas ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            internalProcessor.ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override TextureAtlas Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input) =>
            internalProcessor.Process(manager, metadata, CreateTextureAtlasDescription(input));

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing =>
            internalProcessor.SupportsPreprocessing;
        
        /// <summary>
        /// Creates a texture atlas description from the specified input JSON.
        /// </summary>
        private TextureAtlasDescription CreateTextureAtlasDescription(JObject input)
        {
            var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
            var desc = input.ToObject<TextureAtlasDescription>(serializer);
            desc.Metadata = desc.Metadata ?? new TextureAtlasMetadataDescription();

            return desc;
        }

        // Implements the conversion from TextureAtlasDescription to TextureAtlas.
        private readonly TextureAtlasProcessor internalProcessor = new TextureAtlasProcessor();
    }
}
