using System;
using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Loads font assets.
    /// </summary>
    [ContentProcessor]
    public sealed class UltravioletFontProcessorFromXDocument : ContentProcessor<XDocument, UltravioletFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete) =>
            GetSpriteFontProcessor().ExportPreprocessed(manager, metadata, writer, input, delete);

        /// <inheritdoc/>
        public override UltravioletFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            GetSpriteFontProcessor().ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override UltravioletFont Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input) =>
            GetSpriteFontProcessor().Process(manager, metadata, input);

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => 
            GetSpriteFontProcessor().SupportsPreprocessing;

        /// <summary>
        /// Gets the registered content processor for loading <see cref="SpriteFont"/> instances from XML documents.
        /// </summary>
        private static ContentProcessor<XDocument, SpriteFont> GetSpriteFontProcessor()
        {
            var impl = UltravioletContext.DemandCurrent().GetContent().Processors.FindProcessor(typeof(XDocument), typeof(SpriteFont));
            if (impl == null)
                throw new InvalidOperationException(FreeTypeStrings.ContentRedirectionError.Format(typeof(XDocument).Name, typeof(SpriteFont).Name));

            return (ContentProcessor<XDocument, SpriteFont>)impl;
        }
    }
}
