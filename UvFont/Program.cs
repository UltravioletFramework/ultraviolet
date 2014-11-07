using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace UvFont
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var chars    = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            var fontname = "Segoe UI";
            var fontsize = 24f;

            using (var font = new Font(fontname, fontsize * SupersampleFactor))
            {
                var glyphs      = CreateGlyphImages(font, chars);
                var textureSize = 0;
                var textureFits = CalculateTextureSize(glyphs, out textureSize);
                if (!textureFits)
                {
                    Console.WriteLine("won't fit");
                    return;
                }

                using (var imgOutput = CreateOutputTexture(glyphs, textureSize))
                {
                    imgOutput.Save("output.png", ImageFormat.Png);
                }
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

        private static Bitmap CreateOutputTexture(IEnumerable<Bitmap> glyphs, Int32 textureSize)
        {
            var img = new Bitmap(textureSize, textureSize);

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

            return img;
        }

        private const Single SupersampleFactor = 4.0f;
    }
}
