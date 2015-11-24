using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestClass]
    public class TextLayoutCommandStreamTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextLayoutCommandStream class returns the correct value from GetLineBounds().")]
        public void TextLayoutCommandStream_CalculatesCorrectLineBounds()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);

            var blankTexture = default(Texture2D);

            var textParser = default(TextParser);
            var textParserResult = default(TextParserTokenStream);

            var textLayoutEngine = default(TextLayoutEngine);
            var textLayoutResult = default(TextLayoutCommandStream);

            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");

                    blankTexture = Texture2D.Create(1, 1);
                    blankTexture.SetData(new[] { Color.White });

                    textParserResult = new TextParserTokenStream();
                    textParser = new TextParser();
                    textParser.Parse("The |b|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\nThe |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", textParserResult);

                    textLayoutResult = new TextLayoutCommandStream();
                    textLayoutEngine = new TextLayoutEngine();

                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    spriteBatch.Begin();

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    textLayoutEngine.CalculateLayout(textParserResult, textLayoutResult,
                        new TextLayoutSettings(spriteFont, window.ClientSize.Width, window.ClientSize.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    textRenderer.Draw(spriteBatch, textLayoutResult, Vector2.Zero, Color.White);

                    textLayoutResult.AcquirePointers();
                    var line0Bounds = textRenderer.GetLineBounds(textLayoutResult, 0);
                    var line1Bounds = textRenderer.GetLineBounds(textLayoutResult, 1);
                    var line2Bounds = textRenderer.GetLineBounds(textLayoutResult, 2);
                    var line3Bounds = textRenderer.GetLineBounds(textLayoutResult, 3);
                    textLayoutResult.ReleasePointers();

                    spriteBatch.Draw(blankTexture, line0Bounds, Color.Red * 0.5f);
                    spriteBatch.Draw(blankTexture, line1Bounds, Color.Lime * 0.5f);
                    spriteBatch.Draw(blankTexture, line2Bounds, Color.Blue * 0.5f);
                    spriteBatch.Draw(blankTexture, line3Bounds, Color.Yellow * 0.5f);

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextLayoutCommandStream_CalculatesCorrectLineBounds.png");
        }
    }
}
