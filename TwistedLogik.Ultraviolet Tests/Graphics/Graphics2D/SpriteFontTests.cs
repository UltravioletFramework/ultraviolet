using NUnit.Framework;
using System.IO;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class SpriteFontTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprite fonts can be loaded and rendered correctly from XML files.")]
        public void SpriteFont_LoadsAndRendersCorrectly_FromXml()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Arial16Xml");
                })
                .Render(uv =>
                {
                    var cx = 0;
                    var cy = 0;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Regular.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Bold, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Bold.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Italic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Italic.LineSpacing;

                    spriteBatch.DrawString(spriteFont.BoldItalic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.BoldItalic.LineSpacing;

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteFont_LoadsAndRendersCorrectly_FromXml.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprite fonts can be loaded and rendered correctly from JSON files.")]
        public void SpriteFont_LoadsAndRendersCorrectly_FromJson()
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
                    var cx = 0;
                    var cy = 0;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Regular.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Bold, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Bold.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Italic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Italic.LineSpacing;

                    spriteBatch.DrawString(spriteFont.BoldItalic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.BoldItalic.LineSpacing;

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteFont_LoadsAndRendersCorrectly_FromJson.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprite fonts can be loaded and rendered correctly from preprocessed files.")]
        public void SpriteFont_LoadsAndRendersCorrectly_FromPreprocessedAsset()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();

                    var fontAssetPath = CreateMachineSpecificAssetCopy(content, "Fonts/Arial16Xml_Preprocessed");
                    if (!content.Preprocess<SpriteFont>(fontAssetPath))
                        Assert.Fail("Failed to preprocess asset.");

                    spriteFont = content.Load<SpriteFont>(fontAssetPath + ".uvc");
                })
                .Render(uv =>
                {
                    var cx = 0;
                    var cy = 0;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(spriteFont.Regular, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Regular.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Bold, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Bold.LineSpacing;

                    spriteBatch.DrawString(spriteFont.Italic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.Italic.LineSpacing;

                    spriteBatch.DrawString(spriteFont.BoldItalic, "Hello, world!", new Vector2(cx, cy), Color.White);
                    cy += spriteFont.BoldItalic.LineSpacing;

                    spriteBatch.End();
                });

            TheResultingImage(result)
              .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteFont_LoadsAndRendersCorrectly_FromPreprocessedAsset.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprite fonts can be used to draw East Asian characters.")]
        public void SpriteFont_CorrectlyRendersEastAsianCharacters()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/MSGothic16");
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
            
            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/SpriteFont_CorrectlyRendersEastAsianCharacters.png");
        }
    }
}
