using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Xml.Linq;

namespace UvFont
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var chars    = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            var fontname = "Garamond";
            var fontsize = 32f;

            using (var font             = new Font(fontname, fontsize))
            using (var fontSupersampled = new Font(fontname, fontsize * SupersampleFactor))
            {
                var glyphs      = CreateGlyphImages(fontSupersampled, chars);
                var textureSize = 0;
                var textureFits = CalculateTextureSize(glyphs, out textureSize);
                if (!textureFits)
                {
                    Console.WriteLine("won't fit");
                    return;
                }

                GenerateXmlFontDefinition(font, chars);
                GenerateOutputTexture(font, glyphs, textureSize);
            }
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

        private static String GetFontSafeName(Font font)
        {
            return font.FontFamily.Name.Replace(" ", String.Empty);
        }

        private static String GetFontDefinitionSafeName(Font font)
        {
            return String.Format("{0}.xml", GetFontSafeName(font));
        }

        private static String GetFontTextureSafeName(Font font)
        {
            return String.Format("{0}Texture.png", GetFontSafeName(font));
        }

        private static Boolean CalculateTextureSize(IEnumerable<Bitmap> glyphs, out Int32 size)
        {
            size = 64;
            while (size <= 4096)
            {
                if (WillGlyphsFitOnTexture(glyphs, size))
                {
                    return true;
                }
                size *= 2;
            }

            size = 0;
            return false;
        }

        private static Boolean WillGlyphsFitOnTexture(IEnumerable<Bitmap> glyphs, Int32 size)
        {
            var x = 1;
            var y = 1;
            foreach (var glyph in glyphs)
            {
                if (x + glyph.Width >= size)
                {
                    x = 1;
                    y = y + glyph.Height + 1;
                }
                if (y + glyph.Height > size)
                {
                    return false;
                }
                x = x + glyph.Width + 1;
            }
            return true;
        }

        private static void GenerateOutputTexture(Font font, IEnumerable<Bitmap> glyphs, Int32 textureSize)
        {
            using (var img = new Bitmap(textureSize, textureSize))
            {
                using (var gfx = Graphics.FromImage(img))
                {
                    gfx.Clear(Color.Magenta);

                    var x = 1;
                    var y = 1;

                    foreach (var glyph in glyphs)
                    {
                        if (x + glyph.Width >= textureSize)
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

                img.Save(GetFontTextureSafeName(font), ImageFormat.Png);
            }
        }

        private static void GenerateXmlFontDefinition(Font font, String chars)
        {
            using (var img = new Bitmap(1, 1))
            using (var gfx = Graphics.FromImage(img))
            {
                var kernings = 
                    from c1 in chars
                    from c2 in chars
                    let kerning = MeasureKerning(gfx, font, c1, c2)
                    where
                     c1 != c2
                    select new
                    {
                        KerningValue = kerning,
                        KerningXml   = new XElement("Kerning", new XAttribute("Pair", String.Format("{0}{1}", c1, c2)), kerning)
                    };

                var defaultAdjustment = (from k in kernings
                                         group k by k.KerningValue into g
                                         orderby g.Count() descending
                                         select g).First().Key;

                kernings = kernings.Where(x => x.KerningValue != defaultAdjustment);

                var xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SpriteFont",
                        new XElement("Face", new XAttribute("Style", "Regular"),
                            new XElement("Texture", GetFontTextureSafeName(font)),
                            new XElement("Kernings", new XAttribute("DefaultAdjustment", defaultAdjustment),
                                kernings.Select(x => x.KerningXml))
                        )
                    )
                );
                xml.Save(GetFontDefinitionSafeName(font));
            }
        }

        private static Int32 MeasureKerning(Graphics gfx, Font font, Char c1, Char c2)
        {
            var c1Size = gfx.MeasureString(c1.ToString(), font);
            var c2Size = gfx.MeasureString(c2.ToString(), font);
            var kernedSize = gfx.MeasureString(String.Format("{0}{1}", c1, c2), font);
            return (Int32)(kernedSize.Width - (c1Size.Width + c2Size.Width));
        }

        private const Single SupersampleFactor = 4.0f;
    }
}
