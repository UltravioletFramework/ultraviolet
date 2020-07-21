using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class SpriteBatchTests : UltravioletApplicationTestFramework
    {
        [Test]
        [TestCase(FontKind.SpriteFont, ColorEncoding.Linear)]
        [TestCase(FontKind.FreeType2, ColorEncoding.Linear)]
        [TestCase(FontKind.FreeType2, ColorEncoding.Srgb)]
        [Category("Rendering")]
        [Description("Ensures that the SpriteBatch class can render text using the DrawString() method.")]
        public void SpriteBatch_CanRenderSimpleStrings(FontKind fontKind, ColorEncoding encoding)
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont  = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    if (encoding == ColorEncoding.Srgb)
                    {
                        config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                        config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                        config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                    }
                })
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont  = content.Load<UltravioletFont>(fontKind == FontKind.SpriteFont ?
                        "Fonts/SegoeUI" : "Fonts/FiraSans");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", Vector2.Zero, Color.White);
                    spriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings(SpriteFont,Linear).png");
            }
            else
            {
                if (encoding == ColorEncoding.Linear)
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings(FreeType2,Linear).png");
                }
                else
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings(FreeType2,Srgb).png");
                }
            }
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the SpriteBatch class can render right-to-left text using the DrawString() method.")]
        public void SpriteBatch_CanRenderSimpleStrings_RightToLeft()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                    config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                    config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                })
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<UltravioletFont>("Fonts/FiraGO-Regular");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", new Vector2(16f, 16f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.DrawTextReversed, 0f);
                    spriteBatch.DrawString(spriteFont.Regular, "שלום עולם", new Vector2(16f, 48f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.DrawTextReversed, 0f);
                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings_RightToLeft.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the SpriteBatch class can render stroked FreeType2 text using the DrawString() method.")]
        public void SpriteBatch_CanRenderStrokedSimpleStrings()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                    config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                    config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                })
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<UltravioletFont>("Fonts/FiraSansStroked");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", new Vector2(32, 32), Color.White);
                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderStrokedSimpleStrings.png");
        }

        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch can be used to draw East Asian characters.")]
        public void SpriteBatch_CorrectlyRendersEastAsianCharacters(FontKind fontKind)
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<UltravioletFont>(fontKind == FontKind.SpriteFont ?
                        "Fonts/MSGothic16" : "Fonts/NotoSansCJKjp-Regular");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    // From the Japanese Wikipedia article "日本"
                    spriteBatch.DrawString(spriteFont.Regular,
                        "日本国（にっぽんこく、にほんこく）、または日本\n" +
                        "（にっぽん、にほん）は、東アジアに位置する日本\n" +
                        "列島（北海道・本州・四国・九州の主要四島および\n" +
                        "それに付随する島々）及び、南西諸島・小笠原諸島\n" +
                        "などの諸島嶼から成る島国である。日本語が事実上\n" +
                        "の公用語として使用されている。首都は事実上東京\n" +
                        "都とされている。", Vector2.Zero, Color.White);

                    spriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersEastAsianCharacters(SpriteFont).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersEastAsianCharacters(FreeType2).png");
            }
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch can be used to draw shaped Arabic script.")]
        public void SpriteBatch_CorrectlyRendersShapedArabicScript()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(UltravioletFont);
            var sstr = default(ShapedString);

            var result = GivenAnUltravioletApplication()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<UltravioletFont>("Fonts/FiraGO-Regular");

                    using (var textShaper = TextShaper.Create())
                    {
                        textShaper.SetUnicodeProperties(TextDirection.RightToLeft, TextScript.Arabic, "ar");
                        textShaper.Append("مرحبا بالعالم");

                        sstr = textShaper.CreateShapedString(spriteFont.Regular);
                    }
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawShapedString(spriteFont.Regular, sstr, Vector2.Zero, Color.White);
                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersShapedArabicScript.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch can be used to draw stroked East Asian characters using FreeType2.")]
        public void SpriteBatch_CorrectlyRendersStrokedEastAsianCharacters()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<UltravioletFont>("Fonts/NotoSansCJKjpStroked");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    // From the Japanese Wikipedia article "日本"
                    spriteBatch.DrawString(spriteFont.Regular,
                        "日本国（にっぽんこく、にほんこく）、または日本\n" +
                        "（にっぽん、にほん）は、東アジアに位置する日本\n" +
                        "列島（北海道・本州・四国・九州の主要四島および\n" +
                        "それに付随する島々）及び、南西諸島・小笠原諸島\n" +
                        "などの諸島嶼から成る島国である。日本語が事実上\n" +
                        "の公用語として使用されている。首都は事実上東京\n" +
                        "都とされている。", new Vector2(4, 4), Color.White);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersStrokedEastAsianCharacters.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch correctly renders a sprite font's default substitution glyph.")]
        public void SpriteBatch_RendersDefaultSubstitutionGlyphForSpriteFont()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Arial16Json");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(spriteFont.Regular, 
                        "plus ça change, plus c'est la même chose", Vector2.Zero, Color.White);

                    spriteBatch.End();
                });
            
            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_RendersDefaultSubstitutionGlyphForSpriteFont.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch correctly renders a sprite font's non-default substitution glyph.")]
        public void SpriteBatch_RendersSpecifiedSubstitutionGlyphForSpriteFont()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Arial16JsonSubstitution");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(spriteFont.Regular,
                        "plus ça change, plus c'est la même chose", Vector2.Zero, Color.White);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_RendersSpecifiedSubstitutionGlyphForSpriteFont.png");
        }

        [Test]
        [TestCase(ColorEncoding.Linear)]
        [TestCase(ColorEncoding.Srgb)]
        [Category("Rendering")]
        [Description("Ensures that SpriteBatch performs color blending correctly.")]
        public void SpriteBatch_CanBlendColorsCorrectly(ColorEncoding encoding)
        {
            var spriteBatch = default(SpriteBatch);
            var blendBg = default(Texture2D);
            var blendFg = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    if (encoding == ColorEncoding.Srgb)
                    {
                        config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                        config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                        config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                    }
                })
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    blendBg = content.Load<Texture2D>("Textures/ColorBlendBg");
                    blendFg = content.Load<Texture2D>("Textures/ColorBlendFg");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(blendBg, Vector2.Zero, Color.White);
                    spriteBatch.Draw(blendFg, Vector2.Zero, Color.White);
                    spriteBatch.End();
                });

            if (encoding == ColorEncoding.Linear)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanBlendColorsCorrectly(Linear).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanBlendColorsCorrectly(Srgb).png");
            }
        }
    }
}
