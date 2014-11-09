using System;
using System.IO;
using System.Linq;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Loads sprite font assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLSpriteFontTextureProcessor : ContentProcessor<SDL_Surface, SpriteFont>
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SDL_Surface input)
        {
            var imgData = File.ReadAllBytes(metadata.AssetFilePath);

            writer.Write(metadata.Extension);
            writer.Write(imgData.Length);
            writer.Write(imgData);

            var glyphs = OpenGLSpriteFontHelper.IdentifyGlyphs(input);

            writer.Write(glyphs.Count());
            writer.Write(' ');
            writer.Write('?');

            foreach (var glyph in glyphs)
            {
                writer.Write(glyph.X);
                writer.Write(glyph.Y);
                writer.Write(glyph.Width);
                writer.Write(glyph.Height);
            }
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        public override SpriteFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var imgDataExtension = reader.ReadString();
            var imgDataLength = reader.ReadInt32();
            var imgData = reader.ReadBytes(imgDataLength);

            Texture2D texture;
            using (var stream = new MemoryStream(imgData))
                texture = manager.LoadFromStream<Texture2D>(stream, imgDataExtension);            

            var glyphCount = reader.ReadInt32();
            var glyphFirst = reader.ReadChar();
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

            var fontFace = new SpriteFontFace(manager.Ultraviolet, texture, new[] { CharacterRegion.Default }, glyphPositions, glyphFirst, glyphSubst, true);
            var font = new SpriteFont(manager.Ultraviolet, fontFace);

            return font;
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, SDL_Surface input)
        {
            var positions = OpenGLSpriteFontHelper.IdentifyGlyphs(input);
            var texture = manager.Process<SDL_Surface, Texture2D>(input);
            var face = new SpriteFontFace(manager.Ultraviolet, texture, new[] { CharacterRegion.Default }, positions, true);
            return new SpriteFont(manager.Ultraviolet, face);
        }

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
        