using System;
using System.IO;
using System.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Loads sprite font assets.
    /// </summary>
    [ContentProcessor]
    public sealed class SpriteFontTextureProcessor : ContentProcessor<PlatformNativeSurface, SpriteFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, PlatformNativeSurface input, Boolean delete)
        {
            if (!metadata.IsFile)
                throw new NotSupportedException();

            var imgData = File.ReadAllBytes(metadata.AssetFilePath);

            writer.Write(metadata.Extension);
            writer.Write(imgData.Length);
            writer.Write(imgData);

            var glyphs = SpriteFontHelper.IdentifyGlyphs(input);

            writer.Write(glyphs.Count());
            writer.Write('?');

            foreach (var glyph in glyphs)
            {
                writer.Write(glyph.X);
                writer.Write(glyph.Y);
                writer.Write(glyph.Width);
                writer.Write(glyph.Height);
            }
        }

        /// <inheritdoc/>
        public override SpriteFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var imgDataExtension = reader.ReadString();
            var imgDataLength = reader.ReadInt32();
            var imgData = reader.ReadBytes(imgDataLength);

            Texture2D texture;
            using (var stream = new MemoryStream(imgData))
                texture = manager.LoadFromStream<Texture2D>(stream, imgDataExtension, metadata.AssetDensity);

            var glyphCount = reader.ReadInt32();
            var glyphSubst = reader.ReadChar();

            var glyphPositions = new Rectangle[glyphCount];
            for (int i = 0; i < glyphCount; i++)
            {
                var glyphX = reader.ReadInt32();
                var glyphY = reader.ReadInt32();
                var glyphWidth = reader.ReadInt32();
                var glyphHeight = reader.ReadInt32();
                glyphPositions[i] = new Rectangle(glyphX, glyphY, glyphWidth, glyphHeight);
            }

            var fontFace = new SpriteFontFace(manager.Ultraviolet, texture, null, glyphPositions, null, glyphSubst, true);
            var font = new SpriteFont(manager.Ultraviolet, fontFace);

            return font;
        }

        /// <inheritdoc/>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var positions = SpriteFontHelper.IdentifyGlyphs(input);
            var texture = manager.Process<PlatformNativeSurface, Texture2D>(input);
            var face = new SpriteFontFace(manager.Ultraviolet, texture, null, positions, null, true);
            return new SpriteFont(manager.Ultraviolet, face);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;
    }
}
        