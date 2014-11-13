using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.UvFont
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
                                          "                [-overhang:value]\n" +
                                          "                [-sourcetext:text]\n" +
                                          "                [-sourcefile:file]\n" +
                                          "                [-sourceculture:culture]\n" +
                                          "\n" +
                                          "  fontname     Specifies the name of the font for which to generate a\n" +
                                          "               SpriteFont definition.\n" +
                                          "  -nobold      Disables generation of the bold and bold/italic font faces.\n" +
                                          "  -noitalic    Disables generation of the italic and bol/italic font faces.\n" +
                                          "  -fontsize    Specifies the point size of the font.\n" +
                                          "  -sub         Specifies the font's substitution character.\n" +
                                          "  -overhang    Specifies the overhang value for this font.\n" +
                                          "               Overhang adds additional space to the right side of every\n" +
                                          "               character, which is useful for flowing script fonts which\n" +
                                          "               don't fit properly under UvFont's default settings.\n" +
                                          "  -sourcetext  Specifies the source text.  The source text is used to determine\n" +
                                          "               which glyphs must be included in the font.\n" +
                                          "  -sourcefile: A comma-delimited list of files from which to generate the\n" +
                                          "               source text.  If the files are Nucleus localization databases,\n" +
                                          "               only the string variants matching the culture specified by\n" +
                                          "               the -sourceculture option will be read.\n" +
                                          "  -sourceculture\n" +
                                          "               When reading Nucleus localization databases for source text,\n" +
                                          "               this option specifies which culture should be read.  If not\n" +
                                          "               specified, UvFont will read the en-US culture.");
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

                Console.WriteLine("Generating {0}...", GetFontTextureSafeName(parameters));

                foreach (var face in faces)
                {
                    using (var fontSupersampled = new Font(fontName, fontSize * SupersampleFactor, face.Font.Style))
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

                using (var output = CombineFaceTextures(faces, outputTextureSize))
                {
                    output.Save(GetFontTextureSafeName(parameters), ImageFormat.Png);
                }

                Console.WriteLine("Generated {0}.", GetFontTextureSafeName(parameters));

                Console.WriteLine("Generating {0}...", GetFontDefinitionSafeName(parameters));

                var xml = GenerateXmlFontDefinition(parameters, faces, regions, chars);
                using (var xmlWriter = new XmlTextWriter(GetFontDefinitionSafeName(parameters), Encoding.UTF8))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xml.Save(xmlWriter);
                }

                Console.WriteLine("Generated {0}.", GetFontDefinitionSafeName(parameters));
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

        private static IEnumerable<CharacterRegion> CreateCharacterRegions(FontGenerationParameters parameters)
        {
            if (parameters.SourceText != null)
            {
                return CharacterRegion.CreateFromSourceText(parameters.SourceText + parameters.SubstitutionCharacter.ToString());
            }
            if (parameters.SourceFile != null)
            {
                var culture   = parameters.SourceCulture ?? "en-US";
                var files     = parameters.SourceFile.Split(',');
                var filesText = new StringBuilder();
                
                foreach (var file in files)
                {
                    try
                    {
                        var ext = Path.GetExtension(file);
                        if (ext == ".xml")
                        {
                            var xml = XDocument.Load(file);
                            if (xml.Root.Name.LocalName == "LocalizedStrings")
                            {
                                var variants = xml.Root.Descendants(culture).SelectMany(x => x.Elements("Variant"));
                                Console.WriteLine("Reading source file '{0}'... (found {1} string variants)", Path.GetFileName(file), variants.Count());

                                foreach (var variant in variants)
                                {
                                    filesText.Append(variant.Value);
                                }
                                continue;
                            }
                        }

                        Console.WriteLine("Reading source file '{0}'...", Path.GetFileName(file));
                        filesText.Append(File.ReadAllText(file));
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
            return null;
        }

        private static IEnumerable<Char> CreateCharacterList(IEnumerable<CharacterRegion> regions)
        {
            var chars = new List<Char>();
            var regionsToUse = (regions == null) ? new[] { CharacterRegion.Default } : regions;
            foreach (var region in regionsToUse)
            {
                for (Char c = region.Start; c <= region.End; c++)
                {
                    chars.Add(c);
                }
            }
            return chars;
        }

        private static IEnumerable<Bitmap> CreateGlyphImages(FontGenerationParameters parameters, Font font, IEnumerable<Char> chars)
        {
            var glyphs = new List<Bitmap>();

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                foreach (var c in chars)
                {
                    var glyphSize               = gfx.MeasureString(c.ToString(), font);
                    var glyphWidth              = (Int32)(Math.Ceiling(glyphSize.Width) / SupersampleFactor) + parameters.Overhang;
                    var glyphHeight             = (Int32)(Math.Ceiling(glyphSize.Height) / SupersampleFactor);
                    var glyphSupersampledWidth  = (Int32)Math.Ceiling(glyphSize.Width) + (parameters.Overhang * SupersampleFactor);
                    var glyphSupersampledHeight = (Int32)Math.Ceiling(glyphSize.Height);

                    var glyphImg = new Bitmap(glyphWidth, glyphHeight);

                    using (var glyphSupersampledImg = new Bitmap(glyphSupersampledWidth, glyphSupersampledHeight))
                    using (var glyphSupersampledGfx = Graphics.FromImage(glyphSupersampledImg))
                    {
                        glyphSupersampledGfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        glyphSupersampledGfx.SmoothingMode = SmoothingMode.HighQuality;

                        glyphSupersampledGfx.Clear(Color.Transparent);
                        glyphSupersampledGfx.DrawString(c.ToString(), font, Brushes.White, 0f, 0f);

                        using (var glyphGfx = Graphics.FromImage(glyphImg))
                        {
                            glyphGfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            glyphGfx.Clear(Color.Transparent);
                            glyphGfx.DrawImage(glyphSupersampledImg, new Rectangle(0, 0, glyphWidth, glyphHeight));
                        }
                    }

                    glyphs.Add(glyphImg);
                }
            }

            return glyphs;
        }

        private static String GetFontSafeName(FontGenerationParameters parameters)
        {
            return String.Format("{0}{1}", 
                parameters.FontName.Replace(" ", String.Empty),
                parameters.FontSize.ToString().Replace('.', '_'));
        }

        private static String GetFontDefinitionSafeName(FontGenerationParameters parameters)
        {
            return String.Format("{0}.xml", GetFontSafeName(parameters));
        }

        private static String GetFontTextureSafeName(FontGenerationParameters parameters)
        {
            return String.Format("{0}Texture.png", GetFontSafeName(parameters));
        }

        private static Boolean CalculateTextureSize(IEnumerable<Bitmap> glyphs, out Size size)
        {
            size = new Size(64, 64);
            while (size.Width <= MaxOutputSize && size.Height < MaxOutputSize)
            {
                if (WillGlyphsFitOnTexture(glyphs, ref size))
                {
                    return true;
                }
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

                width  = Math.Max(width, x + glyph.Width + 1);
                height = Math.Max(height, y + lineHeight + 1);

                x = x + glyph.Width + 1;
            }
            size = new Size(width, height - 1);
            return true;
        }

        private static Bitmap GenerateFaceTexture(Font font, IEnumerable<Bitmap> glyphs, Size textureSize)
        {
            var lineHeight = glyphs.Max(g => g.Height);

            var img = new Bitmap(textureSize.Width, textureSize.Height);

            using (var gfx = Graphics.FromImage(img))
            {
                gfx.Clear(Color.Magenta);

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

        private static Bitmap CombineFaceTextures(IEnumerable<FontFaceInfo> faces, Size size)
        {
            var widthPowerOfTwo  = MathUtil.FindNextPowerOfTwo(size.Width);
            var heightPowerOfTwo = MathUtil.FindNextPowerOfTwo(size.Height);
            var output           = new Bitmap(widthPowerOfTwo, heightPowerOfTwo);

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

        private static Object SanitizeCharacterForXml(Char c)
        {
            if (Char.IsWhiteSpace(c))
            {
                return new XCData(c.ToString());
            }
            return c;
        }

        private static XDocument GenerateXmlFontDefinition(FontGenerationParameters parameters, IEnumerable<FontFaceInfo> faces, 
            IEnumerable<CharacterRegion> characterRegions, IEnumerable<Char> chars)
        {
            var x = 0;
            var y = 0;

            var characterRegionsElement = default(XElement);
            if (characterRegions != null)
            {
                characterRegionsElement = new XElement("CharacterRegions", characterRegions.Select(region => new XElement("CharacterRegion",
                    new XElement("Start", SanitizeCharacterForXml(region.Start)),
                    new XElement("End", SanitizeCharacterForXml(region.End)))));
            }

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var faceDefinitions = new List<XElement>();

                foreach (var face in faces)
                {
                    Console.WriteLine("Calculating kerning for {0} face...", face.Name);

                    if (y + face.Texture.Height > MaxOutputSize)
                    {
                        x = x + face.Texture.Width;
                        y = 0;
                    }

                    var kernings = 
                        from c1 in chars
                        from c2 in chars
                        let kerning = MeasureKerning(parameters, gfx, face.Font, c1, c2)
                        where
                         c1 != c2
                        select new
                        {
                            KerningValue = kerning,
                            KerningXml   = new XElement("Kerning", 
                                new XAttribute("Pair", String.Format("{0}{1}", c1, c2)), kerning)
                        };

                    var defaultAdjustment = (from k in kernings
                                             group k by k.KerningValue into g
                                             orderby g.Count() descending
                                             select g).First().Key;

                    kernings = kernings.Where(k => k.KerningValue != defaultAdjustment);

                    var faceDefinition = 
                        new XElement("Face", new XAttribute("Style", face.Name),
                            new XElement("Texture", GetFontTextureSafeName(parameters)),
                            new XElement("TextureRegion", String.Format("{0} {1} {2} {3}", x, y, face.Texture.Width, face.Texture.Height)),
                            new XElement("Kernings", new XAttribute("DefaultAdjustment", defaultAdjustment),
                                kernings.Select(k => k.KerningXml))
                        );
                    faceDefinitions.Add(faceDefinition);

                    y = y + face.Texture.Height;
                }

                return new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SpriteFont", characterRegionsElement, faceDefinitions)
                );
            }
        }

        private static Int32 MeasureKerning(FontGenerationParameters parameters, Graphics gfx, Font font, Char c1, Char c2)
        {
            var c1Size = gfx.MeasureString(c1.ToString(), font);
            var c2Size = gfx.MeasureString(c2.ToString(), font);
            var kernedSize = gfx.MeasureString(String.Format("{0}{1}", c1, c2), font);
            return (Int32)(kernedSize.Width - (c1Size.Width + c2Size.Width)) - parameters.Overhang;
        }

        private const Int32 SupersampleFactor = 8;
        private const Int32 MaxOutputSize     = 4096;
    }
}
