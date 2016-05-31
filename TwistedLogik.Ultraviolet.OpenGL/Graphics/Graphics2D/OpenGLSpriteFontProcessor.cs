using System;
using System.Collections.Generic;
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
    internal sealed class OpenGLSpriteFontProcessor : ContentProcessor<SpriteFontDescription, SpriteFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SpriteFontDescription input, Boolean delete)
        {
            writer.Write(input.CharacterRegions?.Count() ?? 0);
            if (input.CharacterRegions != null)
            {
                foreach (var characterRegion in input.CharacterRegions)
                {
                    writer.Write(characterRegion.Start);
                    writer.Write(characterRegion.End);
                }
            }

            ExportPreprocessedFace(manager, metadata, writer, input.Faces?.Regular, "Regular", delete);
            ExportPreprocessedFace(manager, metadata, writer, input.Faces?.Bold, "Bold", delete);
            ExportPreprocessedFace(manager, metadata, writer, input.Faces?.Italic, "Italic", delete);
            ExportPreprocessedFace(manager, metadata, writer, input.Faces?.BoldItalic, "BoldItalic", delete);
        }

        /// <inheritdoc/>
        public override SpriteFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var characterRegionCount = reader.ReadInt32();
            var characterRegions = characterRegionCount > 0 ? new CharacterRegion[characterRegionCount] : null;
            for (int i = 0; i < characterRegionCount; i++)
            {
                characterRegions[i] = new CharacterRegion(
                    reader.ReadChar(),
                    reader.ReadChar());
            }

            var faceRegular = ImportPreprocessedFace(manager, metadata, reader, characterRegions);
            var faceBold = ImportPreprocessedFace(manager, metadata, reader, characterRegions);
            var faceItalic = ImportPreprocessedFace(manager, metadata, reader, characterRegions);
            var faceBoldItalic = ImportPreprocessedFace(manager, metadata, reader, characterRegions);

            return new SpriteFont(manager.Ultraviolet, faceRegular, faceBold, faceItalic, faceBoldItalic);
        }

        /// <inheritdoc/>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, SpriteFontDescription input)
        {
            var textures = (new[] { input.Faces?.Regular?.Texture, input.Faces?.Bold?.Texture, input.Faces?.Italic?.Texture, input.Faces?.BoldItalic?.Texture })
                .Where(x => x != null).Distinct()
                .Select(x => ResolveDependencyAssetPath(metadata, x))
                .ToDictionary(x => x, x => manager.Import<SDL_Surface>(x));

            try
            {
                var characterRegions = input.CharacterRegions?.Select(x => new CharacterRegion(x.Start, x.End)).ToList();

                var faceRegular = ProcessFace(textures, manager, metadata, input.Faces?.Regular, "Regular", characterRegions);
                var faceBold = ProcessFace(textures, manager, metadata, input.Faces?.Bold, "Bold", characterRegions);
                var faceItalic = ProcessFace(textures, manager, metadata, input.Faces?.Italic, "Italic", characterRegions);
                var faceBoldItalic = ProcessFace(textures, manager, metadata, input.Faces?.BoldItalic, "BoldItalic", characterRegions);

                return new SpriteFont(manager.Ultraviolet, faceRegular, faceBold, faceItalic, faceBoldItalic);
            }
            finally
            {
                foreach (var texture in textures)
                {
                    texture.Value.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;
        
        /// <summary>
        /// Exports a font face to the specified preprocessed asset stream.
        /// </summary>
        private static void ExportPreprocessedFace(ContentManager manager, 
            IContentProcessorMetadata metadata, BinaryWriter writer, SpriteFontFaceDescription description, String style, Boolean delete)
        {
            if (description == null)
            {
                writer.Write(false);
                return;
            }
            writer.Write(true);

            var textureName = ResolveDependencyAssetPath(metadata, description.Texture);
            var textureRegion = description.TextureRegion;

            if (String.IsNullOrEmpty(textureName))
                throw new ContentLoadException(OpenGLStrings.InvalidSpriteFontTexture);

            var glyphs = default(IEnumerable<Rectangle>);
            using (var surface = manager.Import<SDL_Surface>(textureName))
                glyphs = OpenGLSpriteFontHelper.IdentifyGlyphs(surface, textureRegion);

            var substitution = description.Glyphs?.Substitution ?? '?';
            
            writer.Write(substitution);

            writer.Write(glyphs.Count());
            foreach (var glyph in glyphs)
            {
                writer.Write(glyph.X);
                writer.Write(glyph.Y);
                writer.Write(glyph.Width);
                writer.Write(glyph.Height);
            }

            var kerningDefaultAdjustment = description.Kernings?["default"] ?? 0;
            var kerning = description.Kernings?.Where(x => !String.Equals(x.Key, "default", StringComparison.InvariantCulture))
                .ToDictionary(x => CreateKerningPair(x.Key), x => x.Value);

            writer.Write(kerningDefaultAdjustment);
            writer.Write(kerning.Count);
            foreach (var kvp in kerning)
            {
                writer.Write(kvp.Key.FirstCharacter);
                writer.Write(kvp.Key.SecondCharacter);
                writer.Write(kvp.Value);
            }

            manager.Preprocess<Texture2D>(textureName, delete);
        }

        /// <summary>
        /// Imports a font face from the specified preprocessed asset stream.
        /// </summary>
        private static SpriteFontFace ImportPreprocessedFace(ContentManager manager, 
            IContentProcessorMetadata metadata, BinaryReader reader, IEnumerable<CharacterRegion> characterRegions)
        {
            var faceExists = reader.ReadBoolean();
            if (!faceExists)
            {
                return null;
            }

            var texture = manager.Load<Texture2D>(reader.ReadString());
            var substitution = reader.ReadChar();

            var glyphPositions = new List<Rectangle>();
            var glyphCount = reader.ReadInt32();
            for (int j = 0; j < glyphCount; j++)
            {
                var glyphX = reader.ReadInt32();
                var glyphY = reader.ReadInt32();
                var glyphWidth = reader.ReadInt32();
                var glyphHeight = reader.ReadInt32();

                glyphPositions.Add(new Rectangle(glyphX, glyphY, glyphWidth, glyphHeight));
            }

            var face = new SpriteFontFace(manager.Ultraviolet, texture, characterRegions, glyphPositions, substitution);
            var kerning = new Dictionary<SpriteFontKerningPair, Int32>();
            var kerningDefaultAdjustment = reader.ReadInt32();
            var kerningCount = reader.ReadInt32();
            for (int j = 0; j < kerningCount; j++)
            {
                var pairFirstChar = reader.ReadChar();
                var pairSecondChar = reader.ReadChar();
                var offset = reader.ReadInt32();

                kerning[new SpriteFontKerningPair(pairFirstChar, pairSecondChar)] = offset;
            }

            face.Kerning.DefaultAdjustment = kerningDefaultAdjustment;
            foreach (var kvp in kerning)
            {
                face.Kerning.Set(kvp.Key, kvp.Value);
            }

            return face;
        }

        /// <summary>
        /// Processes the definition for a single font face.
        /// </summary>
        private static SpriteFontFace ProcessFace(Dictionary<String, SDL_Surface> textures, ContentManager manager,
            IContentProcessorMetadata metadata, SpriteFontFaceDescription description, String style, IEnumerable<CharacterRegion> characterRegions)
        {
            if (description == null)
                return null;

            var textureName = ResolveDependencyAssetPath(metadata, description.Texture);
            var textureRegion = description.TextureRegion;

            if (String.IsNullOrEmpty(textureName))
                throw new ContentLoadException(OpenGLStrings.InvalidSpriteFontTexture);

            var faceSurface = textures[textureName];
            var faceGlyphs = OpenGLSpriteFontHelper.IdentifyGlyphs(faceSurface, textureRegion);

            var faceTexture = manager.Load<Texture2D>(textureName);
            var face = new SpriteFontFace(manager.Ultraviolet, faceTexture, characterRegions, faceGlyphs);

            var kerningDefaultAdjustment = description.Kernings?["default"] ?? 0;
            var kerning = description.Kernings?.Where(x => !String.Equals(x.Key, "default", StringComparison.InvariantCulture))
                .ToDictionary(x => CreateKerningPair(x.Key), x => x.Value);

            face.Kerning.DefaultAdjustment = kerningDefaultAdjustment;

            foreach (var kvp in kerning)
                face.Kerning.Set(kvp.Key, kvp.Value);

            return face;
        }

        /// <summary>
        /// Creates a <see cref="SpriteFontKerningPair"/> from the specified string.
        /// </summary>
        private static SpriteFontKerningPair CreateKerningPair(String pair)
        {
            if (String.IsNullOrEmpty(pair) || pair.Length != 2)
                throw new ContentLoadException(OpenGLStrings.InvalidSpriteFontKerningPair);

            return new SpriteFontKerningPair(pair[0], pair[1]);
        }
    }
}
        