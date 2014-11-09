﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
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
                    if (String.IsNullOrEmpty(e.Message))
                    {
                        Console.WriteLine("The syntax of this command is:");
                        Console.WriteLine();
                        Console.WriteLine("UVFONT fontname [-nobold] [-noitalic] [-fontsize:emsize] [-sourcetext:text] [-sourcefile:file] [-sub:char]");
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                    return;
                }

                var regions    = CreateCharacterRegions(parameters);
                var chars      = CreateCharacterList(regions);
                var fontName   = parameters.FontName;
                var fontSize   = parameters.FontSize;

                if (!regions.Where(x => x.Contains(parameters.SubstitutionCharacter)).Any())
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

                foreach (var face in faces)
                {
                    using (var fontSupersampled = new Font(fontName, fontSize * SupersampleFactor, face.Font.Style))
                    {
                        var glyphs      = CreateGlyphImages(fontSupersampled, chars);
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
                return CharacterRegion.CreateFromSourceText(parameters.SourceText);
            }
            if (parameters.SourceFile != null)
            {
                throw new NotImplementedException();
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

        private static IEnumerable<Bitmap> CreateGlyphImages(Font font, IEnumerable<Char> chars)
        {
            var glyphs = new List<Bitmap>();

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                foreach (var c in chars)
                {
                    var glyphSize               = gfx.MeasureString(c.ToString(), font);
                    var glyphWidth              = (Int32)(Math.Ceiling(glyphSize.Width) / SupersampleFactor);
                    var glyphHeight             = (Int32)(Math.Ceiling(glyphSize.Height) / SupersampleFactor);
                    var glyphSupersampledWidth  = (Int32)Math.Ceiling(glyphSize.Width);
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

        private static Object SanitizeCharacter(Char c)
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
                    new XElement("Start", SanitizeCharacter(region.Start)),
                    new XElement("End", SanitizeCharacter(region.End)))));
            }

            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var faceDefinitions = new List<XElement>();

                foreach (var face in faces)
                {
                    if (y + face.Texture.Height > MaxOutputSize)
                    {
                        x = x + face.Texture.Width;
                        y = 0;
                    }

                    var kernings = 
                        from c1 in chars
                        from c2 in chars
                        let kerning = MeasureKerning(gfx, face.Font, c1, c2)
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

        private static Int32 MeasureKerning(Graphics gfx, Font font, Char c1, Char c2)
        {
            var c1Size = gfx.MeasureString(c1.ToString(), font);
            var c2Size = gfx.MeasureString(c2.ToString(), font);
            var kernedSize = gfx.MeasureString(String.Format("{0}{1}", c1, c2), font);
            return (Int32)(kernedSize.Width - (c1Size.Width + c2Size.Width));
        }

        private const Int32 SupersampleFactor = 8;
        private const Int32 MaxOutputSize     = 4096;
    }
}
