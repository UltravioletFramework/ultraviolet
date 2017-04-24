using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Tooling;

namespace UvFont
{
    public class Program
    {
        public static void Main(String[] args)
        {
            try
            {
                FontGenerationParameters parameters;
                try { parameters = new FontGenerationParameters(args); }
                catch (InvalidCommandLineException e)
                {
                    if (String.IsNullOrEmpty(e.Error))
                    {
                        Console.WriteLine("Generates Ultraviolet-compatible SpriteFont definition files.");
                        Console.WriteLine();
                        Console.WriteLine("UVFONT fontname [-nobold] [-noitalic] [-fontsize:emsize] [-sub:char]\n" +
                                          "                [-supersample:value]\n" +
                                          "                [-overhang:value]\n" +
                                          "                [-pad-left:value]\n" +
                                          "                [-pad-right:value]\n" +
                                          "                [-sourcetext:text]\n" +
                                          "                [-sourcefile:file]\n" +
                                          "                [-sourceculture:culture]\n" +
                                          "                [-json]\n" +
                                          "\n" +
                                          "  fontname     Specifies the name of the font for which to generate a\n" +
                                          "               SpriteFont definition.\n" +
                                          "  -nobold      Disables generation of the bold and bold/italic font faces.\n" +
                                          "  -noitalic    Disables generation of the italic and bol/italic font faces.\n" +
                                          "  -fontsize    Specifies the point size of the font.\n" +
                                          "  -sub         Specifies the font's substitution character.\n" +
                                          "  -supersample Specifies the super sampling factor used when drawing\n" +
                                          "               the font's glyphs. Defaults to 2.\n" +
                                          "  -overhang    Specifies the overhang value for this font.\n" +
                                          "               Overhang adds additional space to the right side of every\n" +
                                          "               character, which is useful for flowing script fonts which\n" +
                                          "               don't fit properly under UvFont's default settings.\n" +
                                          "               Unlike padding, kerning accounts for overhang.\n" +
                                          "  -pad-left    Adds the specified number of pixels of padding to the\n" +
                                          "               left edge of every glyph.\n" +
                                          "  -pad-right   Adds the specified number of pixels of padding to the\n" +
                                          "               right edge of every glyph.\n" +
                                          "  -sourcetext  Specifies the source text.  The source text is used to determine\n" +
                                          "               which glyphs must be included in the font.\n" +
                                          "  -sourcefile: A comma-delimited list of files from which to generate the\n" +
                                          "               source text.  If the files are Ultraviolet localization databases,\n" +
                                          "               only the string variants matching the culture specified by\n" +
                                          "               the -sourceculture option will be read.\n" +
                                          "  -sourceculture\n" +
                                          "               When reading Ultraviolet localization databases for source text,\n" +
                                          "               this option specifies which culture should be read.  If not\n" +
                                          "               specified, UvFont will read the en-US culture.\n" +
                                          "  -json\n" +
                                          "               Specifies that the output should be in JSON format.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine(e.Error);
                    }
                    return;
                }

                var regions    = CreateCharacterRegions(parameters);
                var chars      = CreateCharacterList(regions);
                var fontName   = parameters.FontName;
                var fontSize   = parameters.FontSize;

                if (regions != null && !regions.Where(x => x.Contains(parameters.SubstitutionCharacter)).Any())
                {
                    Console.WriteLine("None of this font's character regions contain the substitution character ('{0}')", parameters.SubstitutionCharacter);
                    Console.WriteLine("Specify another substitution character with the -sub argument.");
                    return;
                }

                var faces = (new[] 
                {
                    new FontFaceInfo("Regular", new Font(fontName, fontSize, FontStyle.Regular)),
                    parameters.NoBold ? null : 
                        new FontFaceInfo("Bold", new Font(fontName, fontSize, FontStyle.Bold)),
                    parameters.NoItalic ? null : 
                        new FontFaceInfo("Italic", new Font(fontName, fontSize, FontStyle.Italic)),
                    parameters.NoBold || parameters.NoItalic ? null :
                        new FontFaceInfo("BoldItalic", new Font(fontName, fontSize, FontStyle.Bold | FontStyle.Italic)),
                }).Where(face => face != null).ToArray();

                if (faces.Select(x => x.Font).Where(x => !String.Equals(x.Name, fontName, StringComparison.CurrentCultureIgnoreCase)).Any())
                {
                    Console.WriteLine("No font named '{0}' was found on this system.", fontName);
                    return;
                }
                fontName = parameters.FontName = faces.First().Font.Name;

                Console.WriteLine("Generating {0}...", GetFontTexture(parameters));

                foreach (var face in faces)
                {
                    using (var fontSupersampled = new Font(fontName, fontSize * parameters.SuperSamplingFactor, face.Font.Style))
                    {
                        var glyphs      = CreateGlyphImages(parameters, fontSupersampled, chars);
                        var textureSize = Size.Empty;
                        var textureFits = CalculateTextureSize(glyphs, out textureSize);
                        if (!textureFits)
                        {
                            Console.WriteLine("The specified font won't fit within a 4096x4096 texture.");
                            return;
                        }

                        face.Texture = GenerateFaceTexture(face.Font, glyphs, textureSize);
                    }
                }

                var outputTextureSize = Size.Empty;
                if (!WillFaceTexturesFitOnOutput(faces, out outputTextureSize))
                {
                    Console.WriteLine("The specified font won't fit within a 4096x4096 texture.");
                    return;
                }

                using (var output = GenerateCombinedTexture(faces, outputTextureSize))
                {
                    output.Save(GetFontTexture(parameters), ImageFormat.Png);
                }

                Console.WriteLine("Generated {0}.", GetFontTexture(parameters));

                Console.WriteLine("Generating {0}...", GetFontFileName(parameters));

                if (parameters.OutputJson)
                {
                    var json = GenerateJsonFontDefinition(parameters, faces, regions, chars);
                    File.WriteAllText(GetFontFileName(parameters), json.ToString());
                }
                else
                {
                    var xml = GenerateXmlFontDefinition(parameters, faces, regions, chars);
                    using (var xmlWriter = new XmlTextWriter(GetFontFileName(parameters), Encoding.UTF8))
                    {
                        xmlWriter.Formatting = System.Xml.Formatting.Indented;
                        xml.Save(xmlWriter);
                    }
                }

                Console.WriteLine("Generated {0}.", GetFontFileName(parameters));
            }
            catch (ExternalException ex)
            {
                if (ex.ErrorCode == unchecked((Int32)0x80004005))
                {
                    Console.WriteLine("An error was raised by GDI+ during font generation.");
                    Console.WriteLine("Do you have permission to write to this folder?");
                    return;
                }
                throw;
            }
        }

        private static IEnumerable<Bitmap> CreateGlyphImages(FontGenerationParameters parameters, Font font, IEnumerable<Char> chars)
        {
            var glyphs = new List<Bitmap>();

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var layoutArea = new SizeF(Single.MaxValue, Single.MaxValue);

                foreach (var c in chars)
                {
                    var glyphIsWhiteSpace = Char.IsWhiteSpace(c);

                    var glyphSize = glyphIsWhiteSpace ?
                        gfx.MeasureString(c.ToString(), font) :
                        gfx.MeasureString(c.ToString(), font, layoutArea, StringFormat.GenericTypographic);

                    var sf1 = StringFormat.GenericDefault;
                    var sf2 = StringFormat.GenericTypographic;

                    var glyphPadding = glyphIsWhiteSpace ? 0 : parameters.PadLeft + parameters.PadRight;
                    var glyphWidth = (Int32)(Math.Ceiling(glyphSize.Width) / parameters.SuperSamplingFactor) + parameters.Overhang + glyphPadding;
                    var glyphHeight = (Int32)(Math.Ceiling(glyphSize.Height) / parameters.SuperSamplingFactor);
                    var glyphSupersampledWidth = (Int32)Math.Ceiling(glyphSize.Width) + (parameters.Overhang * parameters.SuperSamplingFactor) + (glyphPadding * parameters.SuperSamplingFactor);
                    var glyphSupersampledHeight = (Int32)Math.Ceiling(glyphSize.Height);

                    var glyphImg = new Bitmap(glyphWidth, glyphHeight);

                    using (var glyphSupersampledImg = new Bitmap(glyphSupersampledWidth, glyphSupersampledHeight))
                    using (var glyphSupersampledGfx = Graphics.FromImage(glyphSupersampledImg))
                    {
                        glyphSupersampledGfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        glyphSupersampledGfx.SmoothingMode = SmoothingMode.HighQuality;

                        glyphSupersampledGfx.Clear(Color.Transparent);
                        glyphSupersampledGfx.DrawString(c.ToString(), font, Brushes.White, 0f, 0f, StringFormat.GenericTypographic);

                        using (var glyphGfx = Graphics.FromImage(glyphImg))
                        {
                            var rect = new Rectangle(glyphIsWhiteSpace ? 0 : parameters.PadLeft, 0, glyphWidth, glyphHeight);

                            glyphGfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            glyphGfx.Clear(Color.Transparent);
                            glyphGfx.DrawImage(glyphSupersampledImg, rect);
                        }
                    }

                    glyphs.Add(glyphImg);
                }
            }

            return glyphs;
        }

        private static IEnumerable<CharacterRegion> CreateCharacterRegions(FontGenerationParameters parameters)
        {
            if (parameters.SourceText != null)
                return CreateCharacterRegionsFromSourceText(parameters);

            if (parameters.SourceFile != null)
                return CreateCharacterRegionsFromSourceFile(parameters);

            return null;
        }

        private static IEnumerable<CharacterRegion> CreateCharacterRegionsFromSourceText(FontGenerationParameters parameters)
        {
            return CharacterRegion.CreateFromSourceText(parameters.SourceText + parameters.SubstitutionCharacter.ToString());
        }

        private static IEnumerable<CharacterRegion> CreateCharacterRegionsFromSourceFile(FontGenerationParameters parameters)
        {
            var culture = parameters.SourceCulture ?? CultureInfo.CurrentCulture.ToString();
            var files = parameters.SourceFile.Split(',');
            var filesText = new StringBuilder();

            foreach (var file in files)
            {
                try
                {
                    var ext = Path.GetExtension(file)?.ToLowerInvariant();
                    if (ext == ".xml" || ext == ".json")
                    {
                        var db = new LocalizationDatabase();
                        db.LoadFromFile(file);

                        Console.Write("Reading source file '{0}'... ", Path.GetFileName(file));
                        var count = 0;

                        foreach (var lstring in db.EnumerateCultureStrings(culture))
                        {
                            foreach (var variant in lstring.Value)
                            {
                                filesText.Append(variant.Value);
                                count++;
                            }
                        }
                        Console.WriteLine("(found {0} string variants)", count);
                    }
                    else
                    {
                        Console.WriteLine("Reading source file '{0}'...", Path.GetFileName(file));
                        filesText.Append(File.ReadAllText(file));
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Unable to read file '{0}'.", file);
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Unable to read file '{0}'.", file);
                }
            }

            filesText.Append(parameters.SubstitutionCharacter);
            return CharacterRegion.CreateFromSourceText(filesText.ToString());
        }

        private static IEnumerable<Char> CreateCharacterList(IEnumerable<CharacterRegion> regions)
        {
            var chars = new List<Char>();
            
            regions = regions ?? new[] { CharacterRegion.Default };
            foreach (var region in regions)
            {
                for (Char c = region.Start; c <= region.End; c++)
                    chars.Add(c);
            }

            return chars;
        }

        private static String GetFontSafeName(FontGenerationParameters parameters)
        {
            return String.Format("{0}{1}", 
                parameters.FontName.Replace(" ", String.Empty),
                parameters.FontSize.ToString().Replace('.', '_'));
        }

        private static String GetFontFileName(FontGenerationParameters parameters)
        {
            var extension = parameters.OutputJson ? "json" : "xml";
            return $"{GetFontSafeName(parameters)}.{extension}";
        }

        private static String GetFontTexture(FontGenerationParameters parameters, Boolean extension = true)
        {
            return $"{GetFontSafeName(parameters)}Texture" + (extension ? ".png" : String.Empty);
        }

        private static Int32 MeasureKerning(FontGenerationParameters parameters, Graphics gfx, Font font, Char c1, Char c2)
        {
            if (Char.IsWhiteSpace(c1) || Char.IsWhiteSpace(c2))
                return 0;

            var layoutArea = new SizeF(Single.MaxValue, Single.MaxValue);
            var c1Size = gfx.MeasureString(c1.ToString(), font, layoutArea, StringFormat.GenericTypographic);
            var c2Size = gfx.MeasureString(c2.ToString(), font, layoutArea, StringFormat.GenericTypographic);
            var kernedSize = gfx.MeasureString($"{c1}{c2}", font, layoutArea, StringFormat.GenericTypographic);
            return (Int32)(kernedSize.Width - (c1Size.Width + c2Size.Width)) - parameters.Overhang;
        }

        private static Boolean CalculateTextureSize(IEnumerable<Bitmap> glyphs, out Size size)
        {
            size = new Size(64, 64);
            while (size.Width <= MaxOutputSize && size.Height < MaxOutputSize)
            {
                if (WillGlyphsFitOnTexture(glyphs, ref size))
                    return true;

                size = new Size(size.Width * 2, size.Height * 2);
            }

            size = Size.Empty;
            return false;
        }

        private static Boolean WillGlyphsFitOnTexture(IEnumerable<Bitmap> glyphs, ref Size size)
        {
            var lineHeight = glyphs.Max(g => g.Height);

            var x = 1;
            var y = 1;
            var width = 0;
            var height = 0;
            foreach (var glyph in glyphs)
            {
                if (x + glyph.Width >= size.Width)
                {
                    x = 1;
                    y = y + lineHeight + 1;
                }
                if (y + lineHeight > size.Height)
                {
                    return false;
                }

                width = Math.Max(width, x + glyph.Width + 1);
                height = Math.Max(height, y + lineHeight + 1);

                x = x + glyph.Width + 1;
            }
            size = new Size(width, height - 1);
            return true;
        }

        private static Boolean WillFaceTexturesFitOnOutput(IEnumerable<FontFaceInfo> faces, out Size size)
        {
            size = Size.Empty;

            var x = 0;
            var y = 0;
            foreach (var face in faces)
            {
                var texture = face.Texture;

                if (y + texture.Height > MaxOutputSize)
                {
                    x = x + texture.Width;
                    y = 0;
                }
                if (x + texture.Width > MaxOutputSize)
                {
                    size = Size.Empty;
                    return false;
                }
                if (x + texture.Width > size.Width)
                {
                    size.Width = x + texture.Width;
                }
                if (y + texture.Height > size.Height)
                {
                    size.Height = y + texture.Height;
                }
                y = y + texture.Height;
            }

            return true;
        }

        private static Bitmap GenerateFaceTexture(Font font, IEnumerable<Bitmap> glyphs, Size textureSize)
        {
            var img = new Bitmap(textureSize.Width, textureSize.Height);

            using (var gfx = Graphics.FromImage(img))
            {
                gfx.Clear(Color.Magenta);

                var lineHeight = glyphs.Max(g => g.Height);
                var x = 1;
                var y = 1;
                foreach (var glyph in glyphs)
                {
                    if (x + glyph.Width >= textureSize.Width)
                    {
                        x = 1;
                        y = y + lineHeight + 1;
                    }

                    gfx.SetClip(new Rectangle(x, y, glyph.Width, lineHeight));
                    gfx.Clear(Color.Transparent);
                    gfx.DrawImageUnscaled(glyph, new Point(x, y));

                    x = x + glyph.Width + 1;
                }
            }

            return img;
        }

        private static Bitmap GenerateCombinedTexture(IEnumerable<FontFaceInfo> faces, Size size)
        {
            var widthPowerOfTwo = MathUtil.FindNextPowerOfTwo(size.Width);
            var heightPowerOfTwo = MathUtil.FindNextPowerOfTwo(size.Height);
            var output = new Bitmap(widthPowerOfTwo, heightPowerOfTwo);

            var x = 0;
            var y = 0;
            using (var gfx = Graphics.FromImage(output))
            {
                gfx.Clear(Color.Magenta);

                foreach (var face in faces)
                {
                    var texture = face.Texture;

                    if (y + texture.Height > MaxOutputSize)
                    {
                        x = x + texture.Width;
                        y = 0;
                    }

                    gfx.SetClip(new Rectangle(x, y, texture.Width, texture.Height));
                    gfx.Clear(Color.Transparent);
                    gfx.DrawImageUnscaled(texture, new Point(x, y));

                    y = y + texture.Height;
                }
            }

            return output;
        }

        private static IDictionary<SpriteFontKerningPair, Int32> CalculateKerningsForFontFace(
            FontGenerationParameters parameters, Graphics gfx, Font font, IEnumerable<Char> chars)
        {
            var kernings =
                from c1 in chars
                from c2 in chars
                let kerningPair = new SpriteFontKerningPair(c1, c2)
                let kerningValue = MeasureKerning(parameters, gfx, font, c1, c2)
                select new KeyValuePair<SpriteFontKerningPair, Int32>(kerningPair, kerningValue);

            return kernings.ToDictionary(x => x.Key, x => x.Value);
        }

        private static XDocument GenerateXmlFontDefinition(FontGenerationParameters parameters, IEnumerable<FontFaceInfo> faces, 
            IEnumerable<CharacterRegion> characterRegions, IEnumerable<Char> chars)
        {
            var characterRegionsElement = default(XElement);
            if (characterRegions != null)
            {
                characterRegionsElement = new XElement("CharacterRegions", characterRegions.Select(region => 
                    new XElement("CharacterRegion",
                        new XElement("Start", region.Start),
                        new XElement("End", region.End)
                    )
                ));
            }

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var faceElements = new List<XElement>();

                var x = 0;
                var y = 0;

                foreach (var face in faces)
                {
                    Console.WriteLine("Calculating kerning for {0} face...", face.Name);

                    if (y + face.Texture.Height > MaxOutputSize)
                    {
                        x = x + face.Texture.Width;
                        y = 0;
                    }

                    var kerningData = CalculateKerningsForFontFace(parameters, gfx, face.Font, chars);
                    var kerningDefaultAdjustment =
                        (from kerning in kerningData
                         group kerning by kerning.Value into g
                         orderby g.Count() descending
                         select g).First().Key;

                    var kerningElements = kerningData.Where(data => data.Value != kerningDefaultAdjustment)
                        .Select(data => new XElement("Kerning", new XAttribute("Pair", data.Key), data.Value));

                    var glyphsElement = default(XElement);
                    if (parameters.SubstitutionCharacter != '?')
                    {
                        glyphsElement = new XElement("Glyphs",
                            new XElement("Substitution", parameters.SubstitutionCharacter));
                    }

                    var faceDefinition = new XElement("Face", new XAttribute("Style", face.Name),
                        new XElement("Texture", GetFontTexture(parameters, false)),
                        new XElement("TextureRegion", String.Format("{0} {1} {2} {3}", x, y, face.Texture.Width, face.Texture.Height)),
                        new XElement("Kernings", new XAttribute("DefaultAdjustment", kerningDefaultAdjustment), kerningElements),
                        glyphsElement
                    );
                    faceElements.Add(faceDefinition);

                    y = y + face.Texture.Height;
                }

                return new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SpriteFont", faceElements, characterRegionsElement)
                );
            }
        }

        private static JObject GenerateJsonFontDefinition(FontGenerationParameters parameters, IEnumerable<FontFaceInfo> faces,
            IEnumerable<CharacterRegion> characterRegions, IEnumerable<Char> chars)
        {
            var characterRegionsProperty = default(JProperty);
            if (characterRegions != null)
            {
                characterRegionsProperty = new JProperty("characterRegions", new JArray(
                    characterRegions.Select(region => new JObject(
                        new JProperty("start", region.Start.ToString()),
                        new JProperty("end", region.End.ToString())
                    ))
                ));
            }

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var faceProperties = new List<JProperty>();

                var x = 0;
                var y = 0;

                foreach (var face in faces)
                {
                    Console.WriteLine("Calculating kerning for {0} face...", face.Name);

                    if (y + face.Texture.Height > MaxOutputSize)
                    {
                        x = x + face.Texture.Width;
                        y = 0;
                    }

                    var kerningData = CalculateKerningsForFontFace(parameters, gfx, face.Font, chars);
                    var kerningDefaultAdjustment =
                        (from kerning in kerningData
                         group kerning by kerning.Value into g
                         orderby g.Count() descending
                         select g).First().Key;
                    
                    var kerningProperties = kerningData.Where(data => data.Value != kerningDefaultAdjustment)
                        .Select(data => new JProperty(data.Key.ToString(), data.Value));

                    kerningProperties = Enumerable.Union(new[] { new JProperty("default", kerningDefaultAdjustment)  }, kerningProperties);

                    var glyphsProperty = default(JProperty);
                    if (parameters.SubstitutionCharacter != '?')
                    {
                        glyphsProperty = new JProperty("glyphs", new JObject(
                            new JProperty("substitution", parameters.SubstitutionCharacter.ToString())));
                    }

                    var faceName =
                        face.Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) +
                        face.Name.Substring(1);

                    var faceDefinition = new JProperty(faceName, 
                        new JObject(new[]
                        {
                            new JProperty("texture", GetFontTexture(parameters, false)),
                            new JProperty("textureRegion", new JObject(
                                new JProperty("x", x),
                                new JProperty("y", y),
                                new JProperty("width", face.Texture.Width),
                                new JProperty("height", face.Texture.Height)
                            )),
                            glyphsProperty,
                            new JProperty("kernings", new JObject(kerningProperties))
                        }
                        .Where(property => property != null))
                    );
                    faceProperties.Add(faceDefinition);

                    y = y + face.Texture.Height;
                }

                var facesProperty = faceProperties.Any() ? 
                    new JProperty("faces", new JObject(faceProperties)) : null;

                return new JObject(new[] { facesProperty, characterRegionsProperty }.Where(p => p != null));
            }
        }

        private const Int32 MaxOutputSize = 4096;
    }
}
