using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class SpriteBatchTests : UltravioletApplicationTestFramework
    {
        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that the SpriteBatch class can render text using the DrawString() method.")]
        public void SpriteBatch_CanRenderSimpleStrings(FontKind fontKind)
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont  = default(UltravioletFont);

            var result = GivenAnUltravioletApplication()
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
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings.png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CanRenderSimpleStrings(FreeType2).png");
            }
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
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersEastAsianCharacters.png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteBatch_CorrectlyRendersEastAsianCharacters(FreeType2).png");
            }
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
    }
}
