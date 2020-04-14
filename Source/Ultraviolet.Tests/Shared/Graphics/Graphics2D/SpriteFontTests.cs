using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D
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
    }
}
