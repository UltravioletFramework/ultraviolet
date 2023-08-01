using System;
using System.Collections.Generic;
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
    internal sealed class SpriteFontProcessor : ContentProcessor<SpriteFontDescription, SpriteFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SpriteFontDescription input, Boolean delete)
        {
            writer.Write(Int32.MaxValue);
            writer.Write(1);
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
            var fileVersion = 0;

            var characterRegionCount = reader.ReadInt32();
            if (characterRegionCount == Int32.MaxValue)
            {
                fileVersion = reader.ReadInt32();
                characterRegionCount = reader.ReadInt32();
            }

            var characterRegions = characterRegionCount > 0 ? new CharacterRegion[characterRegionCount] : null;
            for (int i = 0; i < characterRegionCount; i++)
            {
                characterRegions[i] = new CharacterRegion(
                    reader.ReadChar(),
                    reader.ReadChar());
            }

            var faceRegular = ImportPreprocessedFace(manager, metadata, reader, characterRegions, fileVersion);
            var faceBold = ImportPreprocessedFace(manager, metadata, reader, characterRegions, fileVersion);
            var faceItalic = ImportPreprocessedFace(manager, metadata, reader, characterRegions, fileVersion);
            var faceBoldItalic = ImportPreprocessedFace(manager, metadata, reader, characterRegions, fileVersion);

            return new SpriteFont(manager.Ultraviolet, faceRegular, faceBold, faceItalic, faceBoldItalic);
        }

        /// <inheritdoc/>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, SpriteFontDescription input)
        {
            var textures = (new[] { input.Faces?.Regular?.Texture, input.Faces?.Bold?.Texture, input.Faces?.Italic?.Texture, input.Faces?.BoldItalic?.Texture })
                .Where(x => x != null).Distinct()
                .Select(x => ResolveDependencyAssetPath(metadata, x))
                .ToDictionary(x => x, x => manager.Import<PlatformNativeSurface>(x, metadata.AssetDensity, metadata.IsLoadedFromSolution));

            foreach (var kvp in textures)
                metadata.AddAssetDependency(kvp.Key);

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
            writer.Write(textureName ?? String.Empty);

            var textureRegion = description.TextureRegion;
            writer.Write(textureRegion.HasValue);
            if (textureRegion.HasValue)
            {
                writer.Write(textureRegion.Value.X);
                writer.Write(textureRegion.Value.Y);
                writer.Write(textureRegion.Value.Width);
                writer.Write(textureRegion.Value.Height);
            }

            if (String.IsNullOrEmpty(textureName))
                throw new ContentLoadException(UltravioletStrings.InvalidSpriteFontTexture);

            var glyphs = default(IEnumerable<Rectangle>);
            using (var surface = manager.Import<PlatformNativeSurface>(textureName, metadata.AssetDensity))
                glyphs = SpriteFontHelper.IdentifyGlyphs(surface, textureRegion);

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

            writer.Write(description.Ascender);
            writer.Write(description.Descender);
        }

        /// <summary>
        /// Imports a font face from the specified preprocessed asset stream.
        /// </summary>
        private static SpriteFontFace ImportPreprocessedFace(ContentManager manager, 
            IContentProcessorMetadata metadata, BinaryReader reader, IEnumerable<CharacterRegion> characterRegions, Int32 fileVersion)
        {
            var faceExists = reader.ReadBoolean();
            if (!faceExists)
            {
                return null;
            }

            var texturePath = reader.ReadString();
            var texture = manager.Load<Texture2D>(texturePath, metadata.AssetDensity);
            var textureRegionSpecified = reader.ReadBoolean();
            if (textureRegionSpecified)
            {
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
            }

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

            var kerningPairs = new Dictionary<SpriteFontKerningPair, Int32>();
            var kerningDefaultAdjustment = reader.ReadInt32();
            var kerningCount = reader.ReadInt32();
            for (int j = 0; j < kerningCount; j++)
            {
                var pairFirstChar = reader.ReadChar();
                var pairSecondChar = reader.ReadChar();
                var offset = reader.ReadInt32();

                kerningPairs[new SpriteFontKerningPair(pairFirstChar, pairSecondChar)] = offset;
            }

            var kerning = new SpriteFontKerning() { DefaultAdjustment = kerningDefaultAdjustment };
            foreach (var kvp in kerningPairs)
                kerning.Set(kvp.Key, kvp.Value);

            var ascender = (fileVersion > 0) ? reader.ReadInt32() : 0;
            var descender = (fileVersion > 0) ? reader.ReadInt32() : 0;

            return new SpriteFontFace(manager.Ultraviolet, 
                texture, characterRegions, glyphPositions, kerning, ascender, descender, substitution);
        }

        /// <summary>
        /// Processes the definition for a single font face.
        /// </summary>
        private static SpriteFontFace ProcessFace(Dictionary<String, PlatformNativeSurface> textures, ContentManager manager,
            IContentProcessorMetadata metadata, SpriteFontFaceDescription description, String style, IEnumerable<CharacterRegion> characterRegions)
        {
            if (description == null)
                return null;

            var textureName = ResolveDependencyAssetPath(metadata, description.Texture);
            var textureRegion = description.TextureRegion;

            if (String.IsNullOrEmpty(textureName))
                throw new ContentLoadException(UltravioletStrings.InvalidSpriteFontTexture);

            var faceSurface = textures[textureName];
            var faceGlyphs = SpriteFontHelper.IdentifyGlyphs(faceSurface, textureRegion);

            var kerningDefaultAdjustment = description.Kernings?["default"] ?? 0;
            var kerningPairs = description.Kernings?.Where(x => !String.Equals(x.Key, "default", StringComparison.InvariantCulture))
                .ToDictionary(x => CreateKerningPair(x.Key), x => x.Value);

            var kerning = new SpriteFontKerning() { DefaultAdjustment = kerningDefaultAdjustment };
            foreach (var kvp in kerningPairs)
                kerning.Set(kvp.Key, kvp.Value);

            var ascender = description.Ascender;
            var descender = description.Descender;

            var faceTexture = manager.Load<Texture2D>(textureName, metadata.AssetDensity);
            var face = new SpriteFontFace(manager.Ultraviolet, 
                faceTexture, characterRegions, faceGlyphs, kerning, ascender, descender, description.Glyphs?.Substitution ?? '?');

            return face;
        }

        /// <summary>
        /// Creates a <see cref="SpriteFontKerningPair"/> from the specified string.
        /// </summary>
        private static SpriteFontKerningPair CreateKerningPair(String pair)
        {
            if (String.IsNullOrEmpty(pair) || pair.Length != 2)
                throw new ContentLoadException(UltravioletStrings.InvalidSpriteFontKerningPair);

            return new SpriteFontKerningPair(pair[0], pair[1]);
        }
    }
}
        