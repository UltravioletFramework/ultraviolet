using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Loads font assets.
    /// </summary>
    [ContentProcessor]
    public sealed class UltravioletFontProcessorFromFreeType : ContentProcessor<FreeTypeFontInfo, UltravioletFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, FreeTypeFontInfo input, Boolean delete) =>
            GetFreeTypeFontProcessor().ExportPreprocessed(manager, metadata, writer, input, delete);

        /// <inheritdoc/>
        public override UltravioletFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            GetFreeTypeFontProcessor().ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override UltravioletFont Process(ContentManager manager, IContentProcessorMetadata metadata, FreeTypeFontInfo input) =>
            GetFreeTypeFontProcessor().Process(manager, metadata, input);

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => 
            GetFreeTypeFontProcessor().SupportsPreprocessing;

        /// <summary>
        /// Gets the registered content processor for loading <see cref="FreeTypeFont"/> instances from <see cref="FreeTypeFontInfo"/> objects.
        /// </summary>
        private static ContentProcessor<FreeTypeFontInfo, FreeTypeFont> GetFreeTypeFontProcessor()
        {
            var impl = UltravioletContext.DemandCurrent().GetContent().Processors.FindProcessor(typeof(FreeTypeFontInfo), typeof(FreeTypeFont));
            if (impl == null)
                throw new InvalidOperationException(FreeTypeStrings.ContentRedirectionError.Format(typeof(FreeTypeFontInfo).Name, typeof(FreeTypeFont).Name));

            return (ContentProcessor<FreeTypeFontInfo, FreeTypeFont>)impl;
        }
    }
}
