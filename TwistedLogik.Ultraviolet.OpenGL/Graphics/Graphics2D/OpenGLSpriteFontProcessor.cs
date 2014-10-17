using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
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
    public sealed class OpenGLSpriteFontProcessor : ContentProcessor<XDocument, SpriteFont>
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
            ExportPreprocessedFace(manager, metadata, writer, input, "Regular");
            ExportPreprocessedFace(manager, metadata, writer, input, "Bold");
            ExportPreprocessedFace(manager, metadata, writer, input, "Italic");
            ExportPreprocessedFace(manager, metadata, writer, input, "BoldItalic");        
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
            var faceRegular    = ImportPreprocessedFace(manager, metadata, reader);
            var faceBold       = ImportPreprocessedFace(manager, metadata, reader);
            var faceItalic     = ImportPreprocessedFace(manager, metadata, reader);
            var faceBoldItalic = ImportPreprocessedFace(manager, metadata, reader);
            return new SpriteFont(manager.Ultraviolet, faceRegular, faceBold, faceItalic, faceBoldItalic);
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var faceRegular    = ProcessFace(manager, metadata, input, "Regular");
            var faceBold       = ProcessFace(manager, metadata, input, "Bold");
            var faceItalic     = ProcessFace(manager, metadata, input, "Italic");
            var faceBoldItalic = ProcessFace(manager, metadata, input, "BoldItalic");
            return new SpriteFont(manager.Ultraviolet, faceRegular, faceBold, faceItalic, faceBoldItalic);
        }

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }

        /// <summary>
        /// Reads the texture asset from the specified font face definition.
        /// </summary>
        /// <param name="metadata">The content processor's metadata.</param>
        /// <param name="input">The font face definition element.</param>
        /// <returns>The texture asset contained by the font face definition.</returns>
        private static String GetTextureName(IContentProcessorMetadata metadata, XElement input)
        {
            var textureName = input.ElementValueString("Texture");
            if (String.IsNullOrEmpty(textureName))
                throw new InvalidOperationException(OpenGLStrings.InvalidSpriteFontTexture);
            return ResolveDependencyAssetPath(metadata, textureName);
        }

        /// <summary>
        /// Gets the region of the specified sprite font's texture that contains the glyph data.
        /// </summary>
        /// <param name="input">The font face definition element.</param>
        /// <returns>The region of the specified sprite font's texture that contains the glyph data, or null to use the entire texture.</returns>
        private static Rectangle? GetTextureRegion(XElement input)
        {
            var textureRegionElement = input.Element("TextureRegion");
            if (textureRegionElement == null)
                return null;
            return Rectangle.Parse(textureRegionElement.Value);
        }

        /// <summary>
        /// Gets the font's first character.
        /// </summary>
        /// <param name="input">The font face definition element.</param>
        /// <returns>The font's first character.</returns>
        private static Char GetFirstChar(XElement input)
        {
            var glyphs = input.Element("Glyphs");
            if (glyphs == null)
                return ' ';
            var first = glyphs.Element("First");
            return (first == null) ? ' ' : Char.Parse(first.Value);
        }

        /// <summary>
        /// Gets the font's substitution character.
        /// </summary>
        /// <param name="input">The font face definition element.</param>
        /// <returns>The font's substitution character.</returns>
        private static Char GetSubstitutionChar(XElement input)
        {
            var glyphs = input.Element("Glyphs");
            if (glyphs == null)
                return '?';
            var substitution = glyphs.Element("Substitution");
            return (substitution == null) ? ' ' : Char.Parse(substitution.Value);
        }

        /// <summary>
        /// Reads the kerning information from the specified font face definition.
        /// </summary>
        /// <param name="input">The font face definition.</param>
        /// <param name="defaultAdjustment">The default kerning adjustment.</param>
        /// <returns>The kerning information contained by the font face definition.</returns>
        private static Dictionary<SpriteFontKerningPair, Int32> GetKerningInfo(XElement input, out Int32 defaultAdjustment)
        {
            defaultAdjustment = 0;

            var kerning = new Dictionary<SpriteFontKerningPair, Int32>();
            var kerningElement = input.Element("Kernings");
            if (kerningElement != null)
            {
                var kerningPairElements = kerningElement.Elements("Kerning");
                foreach (var kerningPairElement in kerningPairElements)
                {
                    var pair = kerningPairElement.AttributeValueString("Pair");
                    if (String.IsNullOrEmpty(pair) || pair.Length != 2)
                        throw new InvalidOperationException(OpenGLStrings.InvalidSpriteFontKerningPair);

                    var offset = 0;
                    if (!Int32.TryParse(kerningPairElement.Value, out offset))
                        throw new InvalidOperationException(OpenGLStrings.InvalidSpriteFontKerningPair);

                    kerning[new SpriteFontKerningPair(pair[0], pair[1])] = offset;
                }

                defaultAdjustment = kerningElement.AttributeValueInt32("DefaultAdjustment") ?? 0;
            }
            return kerning;
        }

        /// <summary>
        /// Exports a font face to the specified preprocessed asset stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        /// <param name="style">The style of the font face to export.</param>
        private static void ExportPreprocessedFace(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, String style)
        {
            var element = input.Root.Elements("Face").Where(x => x.AttributeValueString("Style") == style).SingleOrDefault();
            if (element == null)
            {
                writer.Write(false);
                return;
            }
            writer.Write(true);

            var textureName = GetTextureName(metadata, element);
            var textureRegion = GetTextureRegion(element);
            writer.Write(textureName);

            IEnumerable<Rectangle> glyphs;
            using (var surface = manager.Import<SDL_Surface>(textureName))
                glyphs = OpenGLSpriteFontHelper.IdentifyGlyphs(surface, textureRegion);

            var first = GetFirstChar(element);
            var substitution = GetSubstitutionChar(element);

            writer.Write(first);
            writer.Write(substitution);

            writer.Write(glyphs.Count());
            foreach (var glyph in glyphs)
            {
                writer.Write(glyph.X);
                writer.Write(glyph.Y);
                writer.Write(glyph.Width);
                writer.Write(glyph.Height);
            }

            var kerningDefaultAdjustment = 0;
            var kerning = GetKerningInfo(element, out kerningDefaultAdjustment);

            writer.Write(kerningDefaultAdjustment);
            writer.Write(kerning.Count);
            foreach (var kvp in kerning)
            {
                writer.Write(kvp.Key.FirstCharacter);
                writer.Write(kvp.Key.SecondCharacter);
                writer.Write(kvp.Value);
            }
        }

        /// <summary>
        /// Imports a font face from the specified preprocessed asset stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The font face that was imported, or null if no font face was imported.</returns>
        private static SpriteFontFace ImportPreprocessedFace(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var faceExists = reader.ReadBoolean();
            if (!faceExists)
            {
                return null;
            }

            var texture               = manager.Load<Texture2D>(reader.ReadString());
            var firstCharacter        = reader.ReadChar();
            var substitutionCharacter = reader.ReadChar();

            var glyphPositions = new List<Rectangle>();
            var glyphCount = reader.ReadInt32();
            for (int j = 0; j < glyphCount; j++)
            {
                var glyphX      = reader.ReadInt32();
                var glyphY      = reader.ReadInt32();
                var glyphWidth  = reader.ReadInt32();
                var glyphHeight = reader.ReadInt32();

                glyphPositions.Add(new Rectangle(glyphX, glyphY, glyphWidth, glyphHeight));
            }

            var face = new SpriteFontFace(manager.Ultraviolet, texture, glyphPositions, firstCharacter, substitutionCharacter);
            var kerning = new Dictionary<SpriteFontKerningPair, Int32>();
            var kerningDefaultAdjustment = reader.ReadInt32();
            var kerningCount = reader.ReadInt32();
            for (int j = 0; j < kerningCount; j++)
            {
                var pairFirstChar  = reader.ReadChar();
                var pairSecondChar = reader.ReadChar();
                var offset         = reader.ReadInt32();

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
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <param name="style">The style of the font face to process.</param>
        /// <returns>The processed font face, or null if the specified style does not exist in the font.</returns>
        private static SpriteFontFace ProcessFace(ContentManager manager, IContentProcessorMetadata metadata, XDocument input, String style)
        {
            var element = input.Root.Elements("Face").Where(x => x.AttributeValueString("Style") == style).SingleOrDefault();
            if (element != null)
            {
                var textureName   = GetTextureName(metadata, element);
                var texture       = manager.Load<Texture2D>(textureName);
                var textureRegion = GetTextureRegion(element);

                var glyphs = default(IEnumerable<Rectangle>);
                using (var surface = manager.Import<SDL_Surface>(textureName))
                {
                    glyphs = OpenGLSpriteFontHelper.IdentifyGlyphs(surface, textureRegion);
                }

                var face = new SpriteFontFace(manager.Ultraviolet, texture, glyphs);

                var kerningDefaultAdjustment = 0;
                var kerning = GetKerningInfo(element, out kerningDefaultAdjustment);

                face.Kerning.DefaultAdjustment = kerningDefaultAdjustment;
                foreach (var kvp in kerning)
                {
                    face.Kerning.Set(kvp.Key, kvp.Value);
                }

                return face;
            }
            return null;
        }
    }
}
        