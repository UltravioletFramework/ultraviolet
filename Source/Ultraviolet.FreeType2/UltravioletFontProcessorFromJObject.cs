using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Loads font assets.
    /// </summary>
    [ContentProcessor]
    public sealed class UltravioletFontProcessorFromJObject : ContentProcessor<JObject, UltravioletFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, JObject input, Boolean delete) =>
            GetSpriteFontProcessor().ExportPreprocessed(manager, metadata, writer, input, delete);

        /// <inheritdoc/>
        public override UltravioletFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            GetSpriteFontProcessor().ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override UltravioletFont Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input) =>
            GetSpriteFontProcessor().Process(manager, metadata, input);

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => 
            GetSpriteFontProcessor().SupportsPreprocessing;

        /// <summary>
        /// Gets the registered content processor for loading <see cref="SpriteFont"/> instances from JSON objects.
        /// </summary>
        private static ContentProcessor<JObject, SpriteFont> GetSpriteFontProcessor()
        {
            var impl = UltravioletContext.DemandCurrent().GetContent().Processors.FindProcessor(typeof(JObject), typeof(SpriteFont));
            if (impl == null)
                throw new InvalidOperationException(FreeTypeStrings.ContentRedirectionError.Format(typeof(JObject).Name, typeof(SpriteFont).Name));

            return (ContentProcessor<JObject, SpriteFont>)impl;
        }
    }
}
