using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestClass]
    public class TextRendererTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Rendering")]
        public void TextRenderer_CanRenderColoredStrings()
        {
            var spriteBatch  = default(SpriteBatch);
            var spriteFont   = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch  = SpriteBatch.Create();
                    spriteFont   = content.Load<SpriteFont>("Fonts/SegoeUI");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, null, null, TextFlags.Standard);
                    textRenderer.Draw(spriteBatch, "Hello, |c:FFFF0000|world|c|! This is a |c:FF00FF00|colored|c| |c:FF0000FF|string|c|!", Vector2.Zero, Color.White, settings);

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanRenderColoredStrings.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void TextRenderer_CanRenderStyledStrings()
        {
            var spriteBatch  = default(SpriteBatch);
            var spriteFont   = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch  = SpriteBatch.Create();
                    spriteFont   = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, null, null, TextFlags.Standard);
                    textRenderer.Draw(spriteBatch, 
                        "This string is regular\n" +
                        "|b|This string is bold|b|\n" +
                        "|i|This string is italic|i|\n" +
                        "|b||i|This string is bold italic|i||b|", Vector2.Zero, Color.White, settings);

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanRenderStyledStrings.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void TextRenderer_CanAlignTextWithinAnArea()
        {
            var spriteBatch  = default(SpriteBatch);
            var spriteFont   = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch  = SpriteBatch.Create();
                    spriteFont   = content.Load<SpriteFont>("Fonts/SegoeUI");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width  = window.ClientSize.Width;
                    var height = window.ClientSize.Height;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settingsTopLeft = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignTop | TextFlags.AlignLeft);
                    textRenderer.Draw(spriteBatch, "Aligned top left", Vector2.Zero, Color.White, settingsTopLeft);

                    var settingsTopCenter = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignTop | TextFlags.AlignCenter);
                    textRenderer.Draw(spriteBatch, "Aligned top center", Vector2.Zero, Color.White, settingsTopCenter);

                    var settingsTopRight = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignTop | TextFlags.AlignRight);
                    textRenderer.Draw(spriteBatch, "Aligned top right", Vector2.Zero, Color.White, settingsTopRight);

                    var settingsMiddleLeft = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignLeft);
                    textRenderer.Draw(spriteBatch, "Aligned middle left", Vector2.Zero, Color.White, settingsMiddleLeft);

                    var settingsMiddleCenter = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                    textRenderer.Draw(spriteBatch, "Aligned middle center", Vector2.Zero, Color.White, settingsMiddleCenter);

                    var settingsMiddleRight = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignRight);
                    textRenderer.Draw(spriteBatch, "Aligned middle right", Vector2.Zero, Color.White, settingsMiddleRight);

                    var settingsBottomLeft = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignBottom | TextFlags.AlignLeft);
                    textRenderer.Draw(spriteBatch, "Aligned bottom left", Vector2.Zero, Color.White, settingsBottomLeft);

                    var settingsBottomCenter = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignBottom | TextFlags.AlignCenter);
                    textRenderer.Draw(spriteBatch, "Aligned bottom center", Vector2.Zero, Color.White, settingsBottomCenter);

                    var settingsBottomRight = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignBottom | TextFlags.AlignRight);
                    textRenderer.Draw(spriteBatch, "Aligned bottom right", Vector2.Zero, Color.White, settingsBottomRight);

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanAlignTextWithinAnArea.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void TextRenderer_CorrectlyAlignsKernedTextAcrossTokenBoundaries()
        {
            var spriteBatch  = default(SpriteBatch);
            var spriteFont   = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch  = SpriteBatch.Create();
                    spriteFont   = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    const string text =
                        "||c:AARRGGBB| - Changes the color of text.\n" + 
                        "|c:FFFF0000|red|c| |c:FFFF8000|orange|c| |c:FFFFFF00|yellow|c| |c:FF00FF00|green|c| |c:FF0000FF|blue|c| |c:FF6F00FF|indigo|c| |c:FFFF00FF|magenta|c|";
                    
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width  = window.ClientSize.Width;
                    var height = window.ClientSize.Height;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                    textRenderer.Draw(spriteBatch, text, Vector2.Zero, Color.White, settings);

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CorrectlyAlignsKernedTextAcrossTokenBoundaries.png");
        }
    }
}
