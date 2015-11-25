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
        [Description("Ensures that the TextRenderer class returns the correct value from GetLineBounds().")]
        public void TextRenderer_CalculatesCorrectLineBounds()
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
                    textParser.Parse("The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\nThe |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", textParserResult);

                    textLayoutResult = new TextLayoutCommandStream();
                    textLayoutEngine = new TextLayoutEngine();
                    
                    var icons = content.Load<Sprite>("Sprites/InterfaceIcons");
                    textLayoutEngine.RegisterIcon("test", icons["test"]);

                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

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

            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CalculatesCorrectLineBounds.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetLineBounds() when layout commands are disabled.")]
        public void TextRenderer_CalculatesCorrectLineBounds_WhenCommandsAreDisabled()
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
                    textParser.Parse("The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\nThe |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", 
                        textParserResult, TextParserOptions.IgnoreCommandCodes);

                    textLayoutResult = new TextLayoutCommandStream();
                    textLayoutEngine = new TextLayoutEngine();

                    var icons = content.Load<Sprite>("Sprites/InterfaceIcons");
                    textLayoutEngine.RegisterIcon("test", icons["test"]);

                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

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
            
            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CalculatesCorrectLineBounds_WhenCommandsAreDisabled.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetGlyphBounds().")]
        public void TextRenderer_CalculatesCorrectGlyphBounds()
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
                    textParser.Parse("The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\nThe |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", textParserResult);

                    textLayoutResult = new TextLayoutCommandStream();
                    textLayoutEngine = new TextLayoutEngine();

                    var icons = content.Load<Sprite>("Sprites/InterfaceIcons");
                    textLayoutEngine.RegisterIcon("test", icons["test"]);

                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin();

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    textLayoutEngine.CalculateLayout(textParserResult, textLayoutResult,
                        new TextLayoutSettings(spriteFont, window.ClientSize.Width, window.ClientSize.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    textRenderer.Draw(spriteBatch, textLayoutResult, Vector2.Zero, Color.White);

                    textLayoutResult.AcquirePointers();
                    var glyph0Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 0);
                    var glyph4Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 4);
                    var glyph14Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 14);
                    var glyph26Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 26);
                    var glyph51Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 51);
                    var glyph52Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 52);
                    var glyphLastBounds = textRenderer.GetGlyphBounds(textLayoutResult, textLayoutResult.TotalLength - 1);
                    textLayoutResult.ReleasePointers();

                    spriteBatch.Draw(blankTexture, glyph0Bounds, Color.Red * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph4Bounds, Color.Cyan * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph14Bounds, Color.Lime * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph26Bounds, Color.Blue * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph51Bounds, Color.Yellow * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph52Bounds, Color.Purple * 0.5f);
                    spriteBatch.Draw(blankTexture, glyphLastBounds, Color.White * 0.5f);

                    spriteBatch.End();
                });
            
            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CalculatesCorrectGlyphBounds.png");
        }
        
        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetGlyphBounds() when layout commands are disabled.")]
        public void TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled()
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
                    textParser.Parse("The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\nThe |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", 
                        textParserResult, TextParserOptions.IgnoreCommandCodes);

                    textLayoutResult = new TextLayoutCommandStream();
                    textLayoutEngine = new TextLayoutEngine();

                    var icons = content.Load<Sprite>("Sprites/InterfaceIcons");
                    textLayoutEngine.RegisterIcon("test", icons["test"]);

                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin();

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    textLayoutEngine.CalculateLayout(textParserResult, textLayoutResult,
                        new TextLayoutSettings(spriteFont, window.ClientSize.Width, window.ClientSize.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    textRenderer.Draw(spriteBatch, textLayoutResult, Vector2.Zero, Color.White);

                    textLayoutResult.AcquirePointers();
                    var glyph0Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 0);
                    var glyph4Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 4);
                    var glyph14Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 14);
                    var glyph26Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 26);
                    var glyph51Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 51);
                    var glyph52Bounds = textRenderer.GetGlyphBounds(textLayoutResult, 52);
                    var glyphLastBounds = textRenderer.GetGlyphBounds(textLayoutResult, textLayoutResult.TotalLength - 1);
                    textLayoutResult.ReleasePointers();

                    spriteBatch.Draw(blankTexture, glyph0Bounds, Color.Red * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph4Bounds, Color.Cyan * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph14Bounds, Color.Lime * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph26Bounds, Color.Blue * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph51Bounds, Color.Yellow * 0.5f);
                    spriteBatch.Draw(blankTexture, glyph52Bounds, Color.Purple * 0.5f);
                    spriteBatch.Draw(blankTexture, glyphLastBounds, Color.White * 0.5f);

                    spriteBatch.End();
                });
            
            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class correctly parses and renders color tags.")]
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

            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanRenderColoredStrings.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class correctly parses and renders styling tags.")]
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

            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanRenderStyledStrings.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class correctly aligns text in accordance with the TextFlags values specified in TextLayoutSettings.")]
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

            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CanAlignTextWithinAnArea.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class takes font kerning into account even when crossing the boundaries between layout tokens.")]
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

            TheResultingImage(result)
                .WithinThreshold(0).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CorrectlyAlignsKernedTextAcrossTokenBoundaries.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct bounding box from its Draw() method.")]
        public void TextRenderer_CorrectlyCalculatesBoundingBoxOfFormattedText()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);
            var blankTexture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                    blankTexture = Texture2D.Create(1, 1);
                    blankTexture.SetData(new[] { Color.White });
                })
                .Render(uv =>
                {
                    const string text =
                        "Lorem ipsum dolor sit amet,\n" +
                        "|b|consectetur adipiscing elit.|b|\n" +
                        "\n" +
                        "|i|Pellentesque egestas luctus sapien|i|\n" +
                        "|b||i|in malesuada.|i||b|\n" + 
                        "\n";
                    
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.ClientSize.Width;
                    var height = window.ClientSize.Height;

                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                    var bounds = textRenderer.Draw(spriteBatch, text, Vector2.Zero, Color.White, settings);

                    spriteBatch.Draw(blankTexture, bounds, Color.Red * 0.5f);

                    spriteBatch.End();
                });
            
            TheResultingImage(result).WithinThreshold(0)
                .ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextRenderer_CorrectlyCalculatesBoundingBoxOfFormattedText.png");
        }
    }
}
