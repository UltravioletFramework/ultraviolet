using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using TwistedLogik.Nucleus;

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
                catch
                {
                    Console.WriteLine("The syntax of this command is:");
                    Console.WriteLine();
                    Console.WriteLine("UVFONT fontname [-fontsize:emsize]");
                    return;
                }

                var chars      = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
                var fontName   = parameters.FontName;
                var fontSize   = parameters.FontSize;

                var faces = new[] 
                {
                    new FontFaceInfo("Regular",    new Font(fontName, fontSize, FontStyle.Regular)),
                    new FontFaceInfo("Bold",       new Font(fontName, fontSize, FontStyle.Bold)),
                    new FontFaceInfo("Italic",     new Font(fontName, fontSize, FontStyle.Italic)),
                    new FontFaceInfo("BoldItalic", new Font(fontName, fontSize, FontStyle.Bold | FontStyle.Italic)),
                };

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

                var xml = GenerateXmlFontDefinition(parameters, faces, chars);
                xml.Save(GetFontDefinitionSafeName(parameters));

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

        private static Dictionary<String, String> ParseCommandLineArgs(IEnumerable<String> args)
        {
            var result = new Dictionary<String, String>();

            foreach (var arg in args)
            {
                if (!arg.StartsWith("-"))
                    return null;

                var ixSeparator = arg.IndexOf(":");
                if (ixSeparator < 0)
                    return null;

                var components = arg.Substring(1).Split(':');
                result[components[0]] = components[1];
            }

            return result;
        }

        private static IEnumerable<Bitmap> CreateGlyphImages(Font font, String chars)
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
            var x = 1;
            var y = 1;
            var width = 0;
            var height = 0;
            foreach (var glyph in glyphs)
            {
                if (x + glyph.Width >= size.Width)
                {
                    x = 1;
                    y = y + glyph.Height + 1;
                }
                if (y + glyph.Height > size.Height)
                {
                    return false;
                }

                width  = Math.Max(width, x + glyph.Width + 1);
                height = Math.Max(height, y + glyph.Height + 1);

                x = x + glyph.Width + 1;
            }
            size = new Size(width, height);
            return true;
        }

        private static Bitmap GenerateFaceTexture(Font font, IEnumerable<Bitmap> glyphs, Size textureSize)
        {
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
                        y = y + glyph.Height + 1;
                    }

                    gfx.SetClip(new Rectangle(x, y, glyph.Width, glyph.Height));
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

        private static XDocument GenerateXmlFontDefinition(FontGenerationParameters parameters, IEnumerable<FontFaceInfo> faces, String chars)
        {
            var x = 0;
            var y = 0;

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
                    new XElement("SpriteFont", faceDefinitions)
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
