using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;
using Ultraviolet.TestFramework.Graphics.Graphics2D.Text;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class TextRendererTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensure that the TextRenderer class does not render text beyond the vertical extent of its layout area.")]
        public void TextRenderer_CutsOffTextThatExceedsVerticalLayoutSpace()
        {
            var content = new TextRendererTestContent(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum commodo pretium risus sollicitudin ultricies. " +
                "Ut quis arcu suscipit, rutrum libero nec, convallis sem. Morbi rhoncus urna nec turpis bibendum eleifend. In vestibulum " +
                "congue feugiat. In pulvinar enim quis diam malesuada, a vestibulum urna bibendum.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height / 2;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignLeft, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CutsOffTextThatExceedsVerticalLayoutSpace.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class renders substrings of formatted text correctly.")]
        public void TextRenderer_CorrectlyRendersSubstrings()
        {
            var content = new TextRendererTestContent("Welcome to |b||c:FFFF0000|Corneria|c||b|");
            content.FontPath = "Fonts/SegoeUI";

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, null, null, TextFlags.Standard));

                    content.SpriteBatch.Begin();

                    var positionX = 0f;
                    var positionY = 0f;

                    positionY = 0f;

                    for (int i = 0; i < content.Text.Length; i++)
                    {
                        content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, new Vector2(positionX, positionY), Color.White, 0, i);
                        positionY += content.TextLayoutResult.ActualHeight;
                    }

                    positionX = uv.GetPlatform().Windows.GetPrimary().Compositor.Width / 3;
                    positionY = 0f;

                    for (int i = 0; i < content.Text.Length; i++)
                    {
                        content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, new Vector2(positionX, positionY), Color.White, i, Int32.MaxValue);
                        positionY += content.TextLayoutResult.ActualHeight;
                    }

                    positionX = 2 * uv.GetPlatform().Windows.GetPrimary().Compositor.Width / 3;
                    positionY = 0f;

                    for (int i = 0; i < content.Text.Length; i++)
                    {
                        content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, new Vector2(positionX, positionY), Color.White, i, 10);
                        positionY += content.TextLayoutResult.ActualHeight;
                    }

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersSubstrings.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text layout engine will not break on a non-breaking space if a breaking space is available on the line.")]
        public void TextRenderer_DoesNotBreakOnNonBreakingSpace_WhenBreakingSpaceIsAvailable()
        {
            var content = new TextRendererTestContent(
                "Distance to Moon: 238,900\u00A0mi\n" +
                "\n" +
                "Distance to Mars: 140\u00A0million\u00A0miles");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignLeft, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_DoesNotBreakOnNonBreakingSpace_WhenBreakingSpaceIsAvailable.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text layout engine WILL break on a non-breaking space if a there is no breaking space available on the line.")]
        public void TextRenderer_BreaksOnNonBreakingSpace_WhenNoBreakingSpaceIsAvailable()
        {
            var content = new TextRendererTestContent("Every\u00A0single\u00A0space\u00A0on\u00A0this\u00A0line\u00A0is\u00A0non-breaking!");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignLeft, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalSourceLength).ShouldBe(content.Text.Length);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksOnNonBreakingSpace_WhenNoBreakingSpaceIsAvailable.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text layout engine correctly splits very long words across multiple lines.")]
        public void TextRenderer_BreaksVeryLongWordsIntoMultipleLines()
        {
            var content = new TextRendererTestContent("Welcome to Llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch! Please enjoy your stay.\n\n" +
                "This message brought to you by the Llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch tourist board.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.Standard));

                    content.SpriteBatch.Begin();

                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalSourceLength).ShouldBe(content.Text.Length);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksVeryLongWordsIntoMultipleLines.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text layout engine correctly splits very long words across multiple lines when hyphenation is enabled.")]
        public void TextRenderer_BreaksVeryLongWordsIntoMultipleLines_WithHyphens()
        {
            var content = new TextRendererTestContent("Welcome to Llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch! Please enjoy your stay.\n\n" +
                "This message brought to you by the Llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch tourist board.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.Standard, TextLayoutOptions.Hyphenate));

                    content.SpriteBatch.Begin();

                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalSourceLength).ShouldBe(content.Text.Length);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksVeryLongWordsIntoMultipleLines_WithHyphens.png");
        }

        [Test]
        [Category("Rendering")]
        [Description(
            "Ensures that the text layout engine breaks lines at the last breaking space prior to the token which exceeded the layout area, and " +
            "that drawing the result with left alignment produces the correct image.")]
        public void TextRenderer_BreaksAtLastBreakingSpace_WithLeftAlignment()
        {
            var content = new TextRendererTestContent("Hello! This is a |icon:test|test|b|test|b||i|test|i||b||i|test|i||b| of the line breaking algorithm.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignLeft, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalGlyphLength).ShouldBe("Hello! This is a Xtesttesttesttest of the line breaking algorithm.".Length);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(0))
                .ShouldHavePosition(0, 0)
                .ShouldHaveSize(132, 22)
                .ShouldHaveLengthInCommands(2)
                .ShouldHaveLengthInGlyphs(17);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(1))
                .ShouldHavePosition(0, 22)
                .ShouldHaveSize(236, 22)
                .ShouldHaveLengthInCommands(15)
                .ShouldHaveLengthInGlyphs(30);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(2))
                .ShouldHavePosition(0, 44)
                .ShouldHaveSize(166, 22)
                .ShouldHaveLengthInCommands(1)
                .ShouldHaveLengthInGlyphs(19);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksAtLastBreakingSpace_WithLeftAlignment.png");
        }

        [Test]
        [Category("Rendering")]
        [Description(
            "Ensures that the text layout engine breaks lines at the last breaking space prior to the token which exceeded the layout area, and " +
            "that drawing the result with right alignment produces the correct image.")]
        public void TextRenderer_BreaksAtLastBreakingSpace_WithRightAlignment()
        {
            var content = new TextRendererTestContent("Hello! This is a |icon:test|test|b|test|b||i|test|i||b||i|test|i||b| of the line breaking algorithm.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignRight, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalGlyphLength).ShouldBe("Hello! This is a Xtesttesttesttest of the line breaking algorithm.".Length);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(0))
                .ShouldHavePosition(108, 0)
                .ShouldHaveSize(132, 22)
                .ShouldHaveLengthInCommands(2)
                .ShouldHaveLengthInGlyphs(17);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(1))
                .ShouldHavePosition(4, 22)
                .ShouldHaveSize(236, 22)
                .ShouldHaveLengthInCommands(15)
                .ShouldHaveLengthInGlyphs(30);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(2))
                .ShouldHavePosition(74, 44)
                .ShouldHaveSize(166, 22)
                .ShouldHaveLengthInCommands(1)
                .ShouldHaveLengthInGlyphs(19);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksAtLastBreakingSpace_WithRightAlignment.png");
        }

        [Test]
        [Category("Rendering")]
        [Description(
            "Ensures that the text layout engine breaks lines at the last breaking space prior to the token which exceeded the layout area, and " +
            "that drawing the result with center alignment produces the correct image.")]
        public void TextRenderer_BreaksAtLastBreakingSpace_WithCenterAlignment()
        {
            var content = new TextRendererTestContent("Hello! This is a |icon:test|test|b|test|b||i|test|i||b||i|test|i||b| of the line breaking algorithm.");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width / 2;
                    var height = window.Compositor.Height;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, width, height, TextFlags.AlignCenter, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.SpriteBatch.Draw(content.BlankTexture,
                        new RectangleF(0, 0, width, height), Color.Red * 0.5f);

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.End();
                });

            TheResultingValue(content.TextLayoutResult.TotalGlyphLength).ShouldBe("Hello! This is a Xtesttesttesttest of the line breaking algorithm.".Length);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(0))
                .ShouldHavePosition(54, 0)
                .ShouldHaveSize(132, 22)
                .ShouldHaveLengthInCommands(2)
                .ShouldHaveLengthInGlyphs(17);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(1))
                .ShouldHavePosition(2, 22)
                .ShouldHaveSize(236, 22)
                .ShouldHaveLengthInCommands(15)
                .ShouldHaveLengthInGlyphs(30);

            TheResultingValue(content.TextLayoutResult.GetLineInfo(2))
                .ShouldHavePosition(37, 44)
                .ShouldHaveSize(166, 22)
                .ShouldHaveLengthInCommands(1)
                .ShouldHaveLengthInGlyphs(19);

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_BreaksAtLastBreakingSpace_WithCenterAlignment.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the GetLineAtPosition() method on TextRenderer returns the correct result for positions inside of text.")]
        public void TextRenderer_GetCorrectLineAtPosition_ForPositionInsideText()
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignLeft | TextFlags.AlignTop));

                    content.TextLayoutResult.AcquirePointers();
                    var lines = new[]
                    {
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 9, 8),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 56, 33),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 112, 55),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 181, 77),
                    };
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    var colors = new[] { Color.Red, Color.Lime, Color.Blue, Color.Yellow };
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line.HasValue)
                        {
                            var bounds = new Rectangle(0, line.Value * content.Font.Regular.LineSpacing,
                                window.DrawableSize.Width, content.Font.Regular.LineSpacing);
                            content.SpriteBatch.Draw(content.BlankTexture, bounds, colors[i] * 0.5f);
                        }
                    }

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetCorrectLineAtPosition_ForPositionInsideText.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the GetLineAtPosition() method on TextRenderer returns the correct result for positions outside of text.")]
        public void TextRenderer_GetCorrectLineAtPosition_ForPositionOutsideText()
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignLeft | TextFlags.AlignTop));

                    content.TextLayoutResult.AcquirePointers();
                    var lines = new[]
                    {
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 8),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 33),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 55),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 77),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 200),
                    };
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    var colors = new[] { Color.Red, Color.Lime, Color.Blue, Color.Yellow, Color.Purple };
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line.HasValue)
                        {
                            var bounds = new Rectangle(0, line.Value * content.Font.Regular.LineSpacing,
                                window.DrawableSize.Width, content.Font.Regular.LineSpacing);
                            content.SpriteBatch.Draw(content.BlankTexture, bounds, colors[i] * 0.5f);
                        }
                    }

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetCorrectLineAtPosition_ForPositionOutsideText.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the GetLineAtPosition() method on TextRenderer returns the correct result for positions outside of text when the 'stretch' parameter is set to true.")]
        public void TextRenderer_GetCorrectLineAtPosition_ForPositionOutsideText_Stretch()
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignLeft | TextFlags.AlignTop));

                    content.TextLayoutResult.AcquirePointers();
                    var lines = new[]
                    {
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 8, true),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 33, true),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 55, true),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 77, true),
                        content.TextRenderer.GetLineAtPosition(content.TextLayoutResult, 350, 200, true),
                    };
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    var colors = new[] { Color.Red, Color.Lime, Color.Blue, Color.Yellow, Color.Purple };
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line.HasValue)
                        {
                            var bounds = new Rectangle(0, line.Value * content.Font.Regular.LineSpacing,
                                window.DrawableSize.Width, content.Font.Regular.LineSpacing);
                            content.SpriteBatch.Draw(content.BlankTexture, bounds, colors[i] * 0.5f);
                        }
                    }

                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetCorrectLineAtPosition_ForPositionOutsideText_Stretch.png");
        }

        [Test]
        [TestCase(false, TestName = "TextRenderer_GetsCorrectGlyphAtPosition_ForPositionInsideGlyph")]
        [TestCase(true, TestName = "TextRenderer_GetsCorrectGlyphAtPosition_ForPositionInsideGlyph(Shaped)")]
        [Category("Rendering")]
        [Description("Ensures that the GetGlyphAtPosition() method on TextRenderer returns the correct result for positions inside of a glyph.")]
        public void TextRenderer_GetsCorrectGlyphAtPosition_ForPositionInsideGlyph(Boolean shaped)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var glyphPositions = shaped ?
                new[] { new Point2(7, 6), new Point2(37, 13), new Point2(73, 35), new Point2(46, 69), new Point2(122, 91) } :
                new[] { new Point2(7, 6), new Point2(49, 10), new Point2(88, 33), new Point2(55, 55), new Point2(146, 77) };

            var glyphIndices = new Int32?[glyphPositions.Length];
            var glyphLines = new Int32?[glyphPositions.Length];

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    if (shaped)
                    {
                        config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                        config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                        config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                    }
                })
                .WithPlugin(shaped ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, shaped ? FontKind.FreeType2 : FontKind.SpriteFont))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var options = shaped ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignLeft | TextFlags.AlignTop, options));

                    content.TextLayoutResult.AcquirePointers();

                    for (int i = 0; i < glyphPositions.Length; i++)
                        glyphIndices[i] = content.TextRenderer.GetGlyphAtPosition(content.TextLayoutResult, glyphPositions[i], out glyphLines[i]);

                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    var colors = new[] { Color.Red, Color.Lime, Color.Blue, Color.Yellow, Color.Purple };
                    for (int i = 0; i < glyphIndices.Length; i++)
                    {
                        var glyph = glyphIndices[i];
                        if (glyph.HasValue)
                        {
                            var bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, glyph.Value);
                            content.SpriteBatch.Draw(content.BlankTexture, bounds, colors[i] * 0.5f);
                        }
                    }

                    content.SpriteBatch.End();
                });

            if (shaped)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetsCorrectGlyphAtPosition_ForPositionInsideGlyph(Shaped).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetsCorrectGlyphAtPosition_ForPositionInsideGlyph.png");
            }

            TheResultingCollection(glyphIndices).ShouldBeExactly(0, 4, 37, 51, 82);
            TheResultingCollection(glyphLines).ShouldBeExactly(0, 0, 1, 2, 3);
        }

        [Test]
        [TestCase(false, TestName = "TextRenderer_GetsCorrectGlyphAtPosition_ForPositionOutsideGlyph")]
        [TestCase(true, TestName = "TextRenderer_GetsCorrectGlyphAtPosition_ForPositionOutsideGlyph(Shaped)")]
        [Category("Rendering")]
        [Description("Ensures that the GetGlyphAtPosition() method on TextRenderer returns the correct result for positions outside of a glyph.")]
        public void TextRenderer_GetsCorrectGlyphAtPosition_ForPositionOutsideGlyph(Boolean shaped)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var glyphPositions = shaped ?
                new[] { new Point2(169, 32), new Point2(178, 122), new Point2(472, 8), new Point2(114, 203), new Point2(391, 217) } :
                new[] { new Point2(169, 32), new Point2(178, 122), new Point2(472, 8), new Point2(114, 203), new Point2(391, 217) };

            var glyphIndices = new Int32?[glyphPositions.Length];
            var glyphLines = new Int32?[glyphPositions.Length];

            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    if (shaped)
                    {
                        config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                        config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                        config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                    }
                })
                .WithPlugin(shaped ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, shaped ? FontKind.FreeType2 : FontKind.SpriteFont))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var options = shaped ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignLeft | TextFlags.AlignTop, options));

                    content.TextLayoutResult.AcquirePointers();

                    for (int i = 0; i < glyphPositions.Length; i++)
                        glyphIndices[i] = content.TextRenderer.GetGlyphAtPosition(content.TextLayoutResult, glyphPositions[i], out glyphLines[i]);

                    content.TextLayoutResult.ReleasePointers();

                    TheResultingCollection(glyphIndices).ShouldBeExactly(null, null, null, null, null);
                    TheResultingCollection(glyphLines).ShouldBeExactly(1, null, 0, null, null);

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    var colors = new[] { Color.Red, Color.Lime, Color.Blue, Color.Yellow, Color.Purple };
                    for (int i = 0; i < glyphIndices.Length; i++)
                    {
                        var glyph = glyphIndices[i];
                        if (glyph.HasValue)
                        {
                            var bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, glyph.Value);
                            content.SpriteBatch.Draw(content.BlankTexture, bounds, colors[i] * 0.5f);
                        }
                    }

                    content.SpriteBatch.End();
                });

            if (shaped)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetsCorrectGlyphAtPosition_ForPositionOutsideGlyph(Shaped).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_GetsCorrectGlyphAtPosition_ForPositionOutsideGlyph.png");
            }
        }

        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetLineBounds().")]
        public void TextRenderer_CalculatesCorrectLineBounds(FontKind fontKind)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    content.TextLayoutResult.AcquirePointers();
                    var line0Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 0);
                    var line1Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 1);
                    var line2Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 2);
                    var line3Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 3);
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.Draw(content.BlankTexture, line0Bounds, Color.Red * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line1Bounds, Color.Lime * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line2Bounds, Color.Blue * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line3Bounds, Color.Yellow * 0.5f);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectLineBounds(SpriteFont).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectLineBounds(FreeType2).png");
            }
        }

        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetLineBounds() when layout commands are disabled.")]
        public void TextRenderer_CalculatesCorrectLineBounds_WhenCommandsAreDisabled(FontKind fontKind)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", TextParserOptions.IgnoreCommandCodes);

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    content.TextLayoutResult.AcquirePointers();
                    var line0Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 0);
                    var line1Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 1);
                    var line2Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 2);
                    var line3Bounds = content.TextRenderer.GetLineBounds(content.TextLayoutResult, 3);
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.Draw(content.BlankTexture, line0Bounds, Color.Red * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line1Bounds, Color.Lime * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line2Bounds, Color.Blue * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, line3Bounds, Color.Yellow * 0.5f);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectLineBounds_WhenCommandsAreDisabled(SpriteFont).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectLineBounds_WhenCommandsAreDisabled(FreeType2).png");
            }
        }

        [Test]
        [TestCase(FontKind.SpriteFont, false, TestName = "TextRenderer_CalculatesCorrectGlyphBounds(SpriteFont)")]
        [TestCase(FontKind.FreeType2, false, TestName = "TextRenderer_CalculatesCorrectGlyphBounds(FreeType2)")]
        [TestCase(FontKind.FreeType2, true, TestName = "TextRenderer_CalculatesCorrectGlyphBounds(FreeType2,Shaped)")]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetGlyphBounds().")]
        public void TextRenderer_CalculatesCorrectGlyphBounds(FontKind fontKind, Boolean shaped)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|");

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var options = shaped ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle, options));

                    content.TextLayoutResult.AcquirePointers();
                    var glyph0Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 0);
                    var glyph4Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 4);
                    var glyph14Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 14);
                    var glyph26Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 26);
                    var glyph27Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 27);
                    var glyph53Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 53);
                    var glyph54Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 54);
                    var glyphLastBounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, content.TextLayoutResult.TotalGlyphLength - 1);
                    content.TextLayoutResult.ReleasePointers();

                    // Glyph 26 is a line break and therefore invisible, so check it manually
                    if (fontKind == FontKind.SpriteFont)
                    {
                        TheResultingValue(glyph26Bounds.X).ShouldBe(369);
                        TheResultingValue(glyph26Bounds.Y).ShouldBe(136);
                        TheResultingValue(glyph26Bounds.Width).ShouldBe(0);
                        TheResultingValue(glyph26Bounds.Height).ShouldBe(22);
                    }
                    else
                    {
                        TheResultingValue(glyph26Bounds.X).ShouldBe(345);
                        TheResultingValue(glyph26Bounds.Y).ShouldBe(130);
                        TheResultingValue(glyph26Bounds.Width).ShouldBe(0);
                        TheResultingValue(glyph26Bounds.Height).ShouldBe(25);
                    }

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.Draw(content.BlankTexture, glyph0Bounds, Color.Red * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph4Bounds, Color.Cyan * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph14Bounds, Color.Lime * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph26Bounds, Color.Red * 0.5f); // should be invisible
                    content.SpriteBatch.Draw(content.BlankTexture, glyph27Bounds, Color.Blue * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph53Bounds, Color.Yellow * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph54Bounds, Color.Purple * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyphLastBounds, Color.White * 0.5f);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds(SpriteFont).png");
            }
            else
            {
                if (shaped)
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds(FreeType2,Shaped).png");
                }
                else
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds(FreeType2).png");
                }
            }
        }

        [Test]
        [TestCase(FontKind.SpriteFont, false, TestName = "TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(SpriteFont)")]
        [TestCase(FontKind.FreeType2, false, TestName = "TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(FreeType2)")]
        [TestCase(FontKind.FreeType2, true, TestName = "TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(FreeType2,Shaped)")]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class returns the correct value from GetGlyphBounds() when layout commands are disabled.")]
        public void TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(FontKind fontKind, Boolean shaped)
        {
            var content = new TextRendererTestContent(
                "The |b||icon:test|quick brown fox|b| jumps\nover the |c:ffff0000|lazy dog.|c|\n" +
                "The |i|quick|i| brown |i|fox|i|\njumps over the |b||i|lazy dog|i||b|", TextParserOptions.IgnoreCommandCodes);

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var options = shaped ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle, options));

                    content.TextLayoutResult.AcquirePointers();
                    var glyph0Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 0);
                    var glyph4Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 4);
                    var glyph14Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 14);
                    var glyph26Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 26);
                    var glyph52Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 52);
                    var glyph53Bounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, 53);
                    var glyphLastBounds = content.TextRenderer.GetGlyphBounds(content.TextLayoutResult, content.TextLayoutResult.TotalGlyphLength - 1);
                    content.TextLayoutResult.ReleasePointers();

                    content.SpriteBatch.Begin();

                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);

                    content.SpriteBatch.Draw(content.BlankTexture, glyph0Bounds, Color.Red * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph4Bounds, Color.Cyan * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph14Bounds, Color.Lime * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph26Bounds, Color.Blue * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph52Bounds, Color.Yellow * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyph53Bounds, Color.Purple * 0.5f);
                    content.SpriteBatch.Draw(content.BlankTexture, glyphLastBounds, Color.White * 0.5f);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(SpriteFont).png");
            }
            else
            {
                if (shaped)
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(FreeType2,Shaped).png");
                }
                else
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CalculatesCorrectGlyphBounds_WhenCommandsAreDisabled(FreeType2).png");
                }
            }
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly parses and renders color tags.")]
        public void TextRenderer_CanRenderColoredStrings()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/SegoeUI");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, null, null, TextFlags.Standard);
                    textRenderer.Draw(spriteBatch, "Hello, |c:FFFF0000|world|c|! This is a |c:FF00FF00|colored|c| |c:FF0000FF|string|c|!", Vector2.Zero, Color.White, settings);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CanRenderColoredStrings.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly parses and renders styling tags.")]
        public void TextRenderer_CanRenderStyledStrings()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");
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

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CanRenderStyledStrings.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly aligns text in accordance with the TextFlags values specified in TextLayoutSettings.")]
        public void TextRenderer_CanAlignTextWithinAnArea()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/SegoeUI");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width;
                    var height = window.Compositor.Height;

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

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CanAlignTextWithinAnArea.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class takes font kerning into account even when crossing the boundaries between layout tokens.")]
        public void TextRenderer_CorrectlyAlignsKernedTextAcrossTokenBoundaries()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                })
                .Render(uv =>
                {
                    const string text =
                        "||c:AARRGGBB| - Changes the color of text.\n" +
                        "|c:FFFF0000|red|c| |c:FFFF8000|orange|c| |c:FFFFFF00|yellow|c| |c:FF00FF00|green|c| |c:FF0000FF|blue|c| |c:FF6F00FF|indigo|c| |c:FFFF00FF|magenta|c|";

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var width = window.Compositor.Width;
                    var height = window.Compositor.Height;

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                    textRenderer.Draw(spriteBatch, text, Vector2.Zero, Color.White, settings);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyAlignsKernedTextAcrossTokenBoundaries.png");
        }

        [Test]
        [Category("Rendering")]
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
                    blankTexture = Texture2D.CreateTexture(1, 1);
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
                    var width = window.Compositor.Width;
                    var height = window.Compositor.Height;

                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                    var bounds = textRenderer.Draw(spriteBatch, text, Vector2.Zero, Color.White, settings);

                    spriteBatch.Draw(blankTexture, bounds, Color.Red * 0.5f);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyCalculatesBoundingBoxOfFormattedText.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly renders links.")]
        public void TextRenderer_CorrectlyRendersLinks()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);
            var textStream = default(TextLayoutCommandStream);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                    textStream = new TextLayoutCommandStream();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var window = uv.GetPlatform().Windows.GetCurrent();
                    var width = window.DrawableSize.Width;
                    var height = window.DrawableSize.Height;

                    textRenderer.LinkStateEvaluator = (target) => String.Equals(target, "visited", StringComparison.InvariantCulture);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
                    textRenderer.CalculateLayout(
                        "Links can |link:unvisited|unvisited|link| if you've never clicked them.\n" +
                        "Links can be |link:visited|visisted|link| if you've already clicked them.\n" +
                        "Links can be |link:active|active even if they\ncross multiple lines|link| if the cursor is clicking them.", textStream, settings);
                    
                    textRenderer.UpdateCursor(textStream, new Point2(236, 191));
                    textRenderer.ActivateLinkAtCursor(textStream);

                    textRenderer.Draw(spriteBatch, textStream, Vector2.Zero, Color.White);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersLinks.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly renders links when using a custom colorizer.")]
        public void TextRenderer_CorrectlyRendersLinks_WithColorizer()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteFont = default(SpriteFont);
            var textRenderer = default(TextRenderer);
            var textStream = default(TextLayoutCommandStream);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteFont = content.Load<SpriteFont>("Fonts/Garamond");
                    textRenderer = new TextRenderer();
                    textStream = new TextLayoutCommandStream();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var window = uv.GetPlatform().Windows.GetCurrent();
                    var width = window.DrawableSize.Width;
                    var height = window.DrawableSize.Height;

                    textRenderer.LinkColorizer = (target, visited, hovering, active, currentColor) =>
                    {
                        if (active)
                        {
                            return Color.Magenta;
                        }
                        else
                        {
                            if (visited)
                            {
                                return Color.Yellow;
                            }
                            else
                            {
                                return Color.Lime;
                            }
                        }
                    };
                    textRenderer.LinkStateEvaluator = (target) => String.Equals(target, "visited", StringComparison.InvariantCulture);

                    var settings = new TextLayoutSettings(spriteFont, width, height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
                    textRenderer.CalculateLayout(
                        "Links can |link:unvisited|unvisited|link| if you've never clicked them.\n" +
                        "Links can be |link:visited|visisted|link| if you've already clicked them.\n" +
                        "Links can be |link:active|active even if they\ncross multiple lines|link| if the cursor is clicking them.", textStream, settings);

                    textRenderer.UpdateCursor(textStream, new Point2(236, 191));
                    textRenderer.ActivateLinkAtCursor(textStream);

                    textRenderer.Draw(spriteBatch, textStream, Vector2.Zero, Color.White);

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersLinks_WithColorizer.png");
        }

        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly renders text which contains surrogate pairs.")]
        public void TextRenderer_CorrectlyRendersSurrogatePairs(FontKind fontKind)
        {
            var content = new TextRendererTestContent(
                "Lorem ipsum 𪘁 dolor sit amet, consectetur adipiscing elit.", TextParserOptions.IgnoreCommandCodes);

            if (fontKind == FontKind.SpriteFont)
                content.FontPath = "Fonts/SegoeUI12";
            else
                content.FontPath = "Fonts/NotoSansCJKjp-Regular";

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, 256, 256, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    content.SpriteBatch.Begin();

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var offset = new Vector2(
                        (Int32)((window.Compositor.Width - 256f) / 2.0f),
                        (Int32)((window.Compositor.Height - 256f) / 2.0f));
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, offset, Color.White);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersSurrogatePairs(SpriteFont).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersSurrogatePairs(FreeType2).png");
            }
        }

        [Test]
        [TestCase(FontKind.SpriteFont)]
        [TestCase(FontKind.FreeType2)]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly renders text which contains surrogate pairs.")]
        public void TextRenderer_CorrectlyRendersIncompleteSurrogatePairs(FontKind fontKind)
        {
            var content = new TextRendererTestContent(
                "Lorem ipsum \uD800 dolor \uDC00 sit amet, consectetur adipiscing elit.", TextParserOptions.IgnoreCommandCodes);

            if (fontKind == FontKind.SpriteFont)
                content.FontPath = "Fonts/SegoeUI12";

            var result = GivenAnUltravioletApplication()
                .WithPlugin(fontKind == FontKind.FreeType2 ? new FreeTypeFontPlugin() : null)
                .WithContent(manager => content.Load(manager, fontKind))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, 256, 256, TextFlags.AlignCenter | TextFlags.AlignMiddle));

                    content.SpriteBatch.Begin();

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var offset = new Vector2(
                        (Int32)((window.Compositor.Width - 256f) / 2.0f),
                        (Int32)((window.Compositor.Height - 256f) / 2.0f));
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, offset, Color.White);

                    content.SpriteBatch.End();
                });

            if (fontKind == FontKind.SpriteFont)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersIncompleteSurrogatePairs(SpriteFont).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersIncompleteSurrogatePairs(FreeType2).png");
            }
        }

        [Test]
        [TestCase(ColorEncoding.Linear)]
        [TestCase(ColorEncoding.Srgb)]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly renders text which makes use of fallback fonts.")]
        public void TextRenderer_CorrectlyRendersFallbackFonts(ColorEncoding encoding)
        {
            var emojiFont = default(UltravioletFont);
            var content = new TextRendererTestContent(
                "😀Lorem ipsum😀 dolor sit 😀🤣😀 amet 🤣 😀", TextParserOptions.IgnoreCommandCodes);

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
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(manager =>
                {
                    content.LoadFreeType(manager);
                    emojiFont = manager.Load<UltravioletFont>("Fonts/NotoColorEmoji");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.Black);

                    content.TextLayoutEngine.RegisterFont("emoji", emojiFont);
                    content.TextLayoutEngine.RegisterFallbackFont("emoji", 126976, 129519, "emoji");

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width, window.Compositor.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle, TextLayoutOptions.None));

                    content.SpriteBatch.Begin();
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);                
                    content.SpriteBatch.End();
                });

            if (encoding == ColorEncoding.Linear)
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersFallbackFonts(Linear).png");
            }
            else
            {
                TheResultingImage(result)
                    .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyRendersFallbackFonts(Srgb).png");
            }
        }

        [Test]
        [TestCase(ColorEncoding.Linear, false, TestName = "TextRenderer_CorrectlyPerformsRightToLeftLayout(Linear)")]
        [TestCase(ColorEncoding.Linear, true, TestName = "TextRenderer_CorrectlyPerformsRightToLeftLayout(Linear,Shaped)")]
        [TestCase(ColorEncoding.Srgb, false, TestName = "TextRenderer_CorrectlyPerformsRightToLeftLayout(Srgb)")]
        [TestCase(ColorEncoding.Srgb, true, TestName = "TextRenderer_CorrectlyPerformsRightToLeftLayout(Srgb,Shaped)")]
        [Category("Rendering")]
        [Description("Ensures that the TextRenderer class correctly lays out right-to-left text.")]
        public void TextRenderer_CorrectlyPerformsRightToLeftLayout(ColorEncoding encoding, Boolean shaped)
        {
            var content = new TextRendererTestContent(
                "Hello, |c:ffff0000|world|c|! This is a |c:ff00ff00|test of RTL text|c| rendering in the |c:ff0000ff|layout engine|c|! Supercalifragilisticexpialidocious", TextParserOptions.None);

            var sourceStringsActual = new List<String>();
            var sourceStringsExpected = new List<String> {
                "Hello, ", "world", "!",
                "This is a ", "test of",
                "RTL text", String.Empty,
                "rendering in the",
                "layout engine", "!",
                "Supercalifragili",
                "sticexpialidoci",
                "ous"  };

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
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(manager =>
                {
                    content.LoadFreeType(manager);
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.Black);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var options = TextLayoutOptions.Hyphenate | (shaped ? TextLayoutOptions.Shape : TextLayoutOptions.None);
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width / 4, window.Compositor.Height, 
                            TextFlags.AlignTop | TextFlags.AlignRight, options, TextDirection.RightToLeft, TextScript.Latin));

                    content.SpriteBatch.Begin();
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);
                    content.SpriteBatch.End();

                    content.TextLayoutResult.AcquirePointers();
                    content.TextLayoutResult.SeekBeginning();
                    unsafe
                    {
                        for (int i = 0; i < content.TextLayoutResult.Count; i++)
                        {
                            var cmdType = *(TextLayoutCommandType*)content.TextLayoutResult.Data;
                            if (cmdType == TextLayoutCommandType.Text)
                            {
                                var cmd = (TextLayoutTextCommand*)content.TextLayoutResult.Data;
                                var cmdText = content.Text.Substring(cmd->SourceOffset, cmd->SourceLength);
                                sourceStringsActual.Add(cmdText);
                            }
                            content.TextLayoutResult.SeekNextCommand();
                        }
                    }
                    content.TextLayoutResult.ReleasePointers();

                    TheResultingCollection(sourceStringsActual)
                        .ShouldBeExactly(sourceStringsExpected);
                });

            if (encoding == ColorEncoding.Linear)
            {
                if (shaped)
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyPerformsRightToLeftLayout(Linear,Shaped).png");
                }
                else
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyPerformsRightToLeftLayout(Linear).png");
                }
            }
            else
            {
                if (shaped)
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyPerformsRightToLeftLayout(Srgb,Shaped).png");
                }
                else
                {
                    TheResultingImage(result)
                        .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_CorrectlyPerformsRightToLeftLayout(Srgb).png");
                }
            }
        }
        
        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text renderer assigns correct source indices to shaped text characters.")]
        public void TextRenderer_AssignsCorrectSourceIndicesToShapedText()
        {
            var content = new TextRendererTestContent(
                "Hello, world! |c:ffff0000|This|c| is a test of source indices!");
            var recombined = String.Empty;
            
            var result = GivenAnUltravioletApplication()
                .WithConfiguration(config =>
                {
                    config.GraphicsConfiguration.SrgbBuffersEnabled = true;
                    config.GraphicsConfiguration.SrgbDefaultForTexture2D = true;
                    config.GraphicsConfiguration.SrgbDefaultForRenderBuffer2D = true;
                })
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(manager => content.Load(manager, FontKind.FreeType2))
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var window = uv.GetPlatform().Windows.GetPrimary();
                    content.TextLayoutEngine.CalculateLayout(content.TextParserResult, content.TextLayoutResult,
                        new TextLayoutSettings(content.Font, window.Compositor.Width / 4, window.Compositor.Height, 
                            TextFlags.AlignCenter | TextFlags.AlignMiddle, TextLayoutOptions.Shape));

                    var sourceIndices = new List<Int32>();
                    unsafe
                    {
                        var source = content.TextLayoutResult.GetSourceShapedStringBuilder(0);

                        content.TextLayoutResult.AcquirePointers();
                        content.TextLayoutResult.Seek(0);
                        for (int i = 0; i < content.TextLayoutResult.Count; i++)
                        {
                            var cmd = *(TextLayoutCommandType*)content.TextLayoutResult.Data;
                            if (cmd == TextLayoutCommandType.Text)
                            {
                                var textCmd = (TextLayoutTextCommand*)content.TextLayoutResult.Data;
                                var shapedOffset = textCmd->ShapedOffset;
                                var shapedLength = textCmd->ShapedLength;
                                var text = source.CreateShapedStringSegmentFromSubstring(shapedOffset, shapedLength);
                                for (int j = 0; j < text.Length; j++)
                                {
                                    var sourceIndex = text[j].SourceIndex;
                                    sourceIndices.Add(sourceIndex);
                                }
                            }
                            content.TextLayoutResult.SeekNextCommand();
                        }
                        content.TextLayoutResult.ReleasePointers();
                    }
                    recombined = String.Join(String.Empty, sourceIndices.Select(x => content.Text[x]));

                    content.SpriteBatch.Begin();
                    content.TextRenderer.Draw(content.SpriteBatch, content.TextLayoutResult, Vector2.Zero, Color.White);
                    content.SpriteBatch.End();
                });

            TheResultingString(recombined).ShouldBe(
                "Hello, world!" +
                "This is a test of" + 
                "source indices!");

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_AssignsCorrectSourceIndicesToShapedText.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the text renderer can optionally ignore certain layout components, such as color changes or font changes.")]
        public void TextRenderer_IgnoresSpecifiedLayoutComponents()
        {
            var content = new TextRendererTestContent("Hello, |c:ffff0000|world|c|! This is |b|bold|b|, |i|italic|i|, |font:alt|stylized|font| text!");

            var result = GivenAnUltravioletApplication()
                .WithContent(content.Load)
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var settings1 = new TextLayoutSettings(content.Font, null, null, TextFlags.Standard, TextLayoutOptions.None, null);
                    var settings2 = new TextLayoutSettings(content.Font, null, null, TextFlags.Standard, TextLayoutOptions.IgnoreColorChanges, null);
                    var settings3 = new TextLayoutSettings(content.Font, null, null, TextFlags.Standard, TextLayoutOptions.IgnoreFontFaceChanges, null);
                    var settings4 = new TextLayoutSettings(content.Font, null, null, TextFlags.Standard, TextLayoutOptions.IgnoreFontStyleChanges, null);
                    var settings5 = new TextLayoutSettings(content.Font, null, null, TextFlags.Standard, TextLayoutOptions.IgnoreStyleChanges, null);

                    var cy = 0;

                    content.SpriteBatch.Begin();
                    cy += (Int32)content.TextRenderer.Draw(content.SpriteBatch, content.Text, new Vector2(0, cy), Color.White, settings1).Height;
                    cy += (Int32)content.TextRenderer.Draw(content.SpriteBatch, content.Text, new Vector2(0, cy), Color.White, settings2).Height;
                    cy += (Int32)content.TextRenderer.Draw(content.SpriteBatch, content.Text, new Vector2(0, cy), Color.White, settings3).Height;
                    cy += (Int32)content.TextRenderer.Draw(content.SpriteBatch, content.Text, new Vector2(0, cy), Color.White, settings4).Height;
                    cy += (Int32)content.TextRenderer.Draw(content.SpriteBatch, content.Text, new Vector2(0, cy), Color.White, settings5).Height;
                    content.SpriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Text/TextRenderer_IgnoresSpecifiedLayoutComponents.png");
        }

        protected static LineInfoResult TheResultingValue(LineInfo obj)
        {
            return new LineInfoResult(obj);
        }
    }
}
