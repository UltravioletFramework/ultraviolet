using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Loads sprite font assets.
    /// </summary>
    [ContentProcessor]
    internal sealed class OpenGLSpriteFontProcessorFromXDocument : ContentProcessor<XDocument, SpriteFont>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete) =>
            implProcessor.ExportPreprocessed(manager, metadata, writer, CreateSpriteFontDescription(manager, metadata, input), delete);

        /// <inheritdoc/>
        public override SpriteFont ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            implProcessor.ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override SpriteFont Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input) =>
            implProcessor.Process(manager, metadata, CreateSpriteFontDescription(manager, metadata, input));

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => implProcessor.SupportsPreprocessing;

        /// <summary>
        /// Creates a <see cref="SpriteFontDescription"/> from the specified input file.
        /// </summary>
        private static SpriteFontDescription CreateSpriteFontDescription(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var spriteFontDesc = new SpriteFontDescription();

            var characterRegionElements = input.Root.Element("CharacterRegions")?.Elements("CharacterRegion")?.ToList();
            if (characterRegionElements != null)
            {
                var characterRegionsList = new CharacterRegionDescription[characterRegionElements.Count];
                spriteFontDesc.CharacterRegions = characterRegionsList;

                for (int i = 0; i < characterRegionElements.Count; i++)
                {
                    var characterRegionElement = characterRegionElements[i];
                    var startElement = characterRegionElement.Element("Start");
                    var startValue = (startElement == null) ? default(Char) : Char.Parse(startElement.Value);
                    var endElement = characterRegionElement.Element("End");
                    var endValue = (endElement == null) ? default(Char) : Char.Parse(endElement.Value);

                    var characterRegion = new CharacterRegionDescription();
                    characterRegion.Start = startValue;
                    characterRegion.End = endValue;

                    characterRegionsList[i] = characterRegion;
                }
            }

            spriteFontDesc.Regular = CreateSpriteFontFaceDescription(manager, metadata, 
                input.Root.Elements("Face").Where(x => String.Equals((String)x.Attribute("Style"), "Regular", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault());

            spriteFontDesc.Bold = CreateSpriteFontFaceDescription(manager, metadata,
                input.Root.Elements("Face").Where(x => String.Equals((String)x.Attribute("Style"), "Bold", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault());

            spriteFontDesc.Italic = CreateSpriteFontFaceDescription(manager, metadata,
                input.Root.Elements("Face").Where(x => String.Equals((String)x.Attribute("Style"), "Italic", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault());

            spriteFontDesc.BoldItalic = CreateSpriteFontFaceDescription(manager, metadata,
                input.Root.Elements("Face").Where(x => String.Equals((String)x.Attribute("Style"), "BoldItalic", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault());

            return spriteFontDesc;
        }

        /// <summary>
        /// Creates a <see cref="SpriteFontFaceDescription"/> from the specified input file.
        /// </summary>
        private static SpriteFontFaceDescription CreateSpriteFontFaceDescription(ContentManager manager, IContentProcessorMetadata metadata, XElement input)
        {
            if (input == null)
                return null;

            var spriteFontFaceDesc = new SpriteFontFaceDescription();

            var textureElement = input.Element("Texture");
            spriteFontFaceDesc.Texture = (String)textureElement;

            var textureRegionElement = input.Element("TextureRegion");
            spriteFontFaceDesc.TextureRegion = (textureRegionElement == null) ? (Rectangle?)null :
                Rectangle.Parse((String)textureRegionElement);

            var glyphsElement = input.Element("Glyphs");
            if (glyphsElement != null)
            {
                var glyphsDesc = new SpriteFontFaceGlyphDescription();
                spriteFontFaceDesc.Glyphs = glyphsDesc;

                var firstElement = glyphsElement.Element("First");
                glyphsDesc.First = (firstElement == null) ? (Char?)null :
                    Char.Parse((String)firstElement);

                var substitutionElement = glyphsElement.Element("Substitution");
                glyphsDesc.Substitution = (substitutionElement == null) ? (Char?)null :
                    Char.Parse((String)substitutionElement);
            }

            var kerningsElement = input.Element("Kernings");
            if (kerningsElement != null)
            {
                var kernings = new Dictionary<String, Int32>();
                spriteFontFaceDesc.Kernings = kernings;

                kernings["default"] = (Int32?)kerningsElement.Attribute("DefaultAdjustment") ?? 0;

                var kerningElements = kerningsElement.Elements("Kerning");
                foreach (var kerningElement in kerningElements)
                {
                    var kerningPair = (String)kerningElement.Attribute("Pair");
                    var kerningValue = (Int32)kerningElement;

                    kernings[kerningPair] = kerningValue;
                }
            }

            return spriteFontFaceDesc;
        }

        // The internal processor which converts SpriteFontDescription -> SpriteFont.
        private readonly OpenGLSpriteFontProcessor implProcessor = new OpenGLSpriteFontProcessor();
    }
}
        