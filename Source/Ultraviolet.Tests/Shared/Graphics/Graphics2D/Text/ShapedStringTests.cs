using System.Linq;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class ShapedStringTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        public void ShapedString_ReflectsUnicodePropertiesOfTextShaper_WhenCreated()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstr = textShaper.CreateShapedString(font.Regular);
                    }
                })
                .RunForOneFrame();

            TheResultingString(sstr.Language).ShouldBe("en");
            TheResultingValue(sstr.Script).ShouldBe(TextScript.Latin);
            TheResultingValue(sstr.Direction).ShouldBe(TextDirection.LeftToRight);
            TheResultingValue(sstr.Length).ShouldBe(13);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_IsCreatedCorrectlyFromShaper_WhenSpecifyingSubstring()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstr = textShaper.CreateShapedString(font.Regular, 7, 5);
                    }
                })
                .RunForOneFrame();

            TheResultingString(sstr.Language).ShouldBe("en");
            TheResultingValue(sstr.Script).ShouldBe(TextScript.Latin);
            TheResultingValue(sstr.Direction).ShouldBe(TextDirection.LeftToRight);
            TheResultingValue(sstr.Length).ShouldBe(5);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_ProvidesCorrectShapingData_InEnglish()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstr = textShaper.CreateShapedString(font.Regular);
                    }
                })
                .RunForOneFrame();

            var chars = new ShapedChar[sstr.Length];
            sstr.CopyTo(0, chars, 0, sstr.Length);

            var glyphIndices = chars.Select(x => x.GlyphIndex).ToArray();
            TheResultingCollection(glyphIndices)
                .ShouldBeExactly(111, 412, 514, 514, 555, 2122, 3, 696, 555, 609, 514, 393, 2125);

            var sourceIndices = chars.Select(x => x.SourceIndex).ToArray();
            TheResultingCollection(sourceIndices)
                .ShouldBeExactly(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            var advances = chars.Select(x => x.Advance).ToArray();
            TheResultingCollection(advances)
                .ShouldBeExactly(11, 9, 5, 5, 9, 4, 4, 11, 9, 6, 5, 10, 4);

            var offsets_x = chars.Select(x => x.OffsetX).ToArray();
            TheResultingCollection(offsets_x)
                .ShouldContainTheSpecifiedNumberOfItems(13)
                .ShouldContainItemsSatisfyingTheCondition(x => x == 0);

            var offsets_y = chars.Select(x => x.OffsetY).ToArray();
            TheResultingCollection(offsets_y)
                .ShouldContainTheSpecifiedNumberOfItems(13)
                .ShouldContainItemsSatisfyingTheCondition(x => x == 0);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_ProvidesCorrectShapingData_InArabic()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraGO-Regular");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.RightToLeft, TextScript.Arabic, "ar");
                        textShaper.Append("مرحبا بالعالم");

                        sstr = textShaper.CreateShapedString(font.Regular);
                    }
                })
                .RunForOneFrame();

            var chars = new ShapedChar[sstr.Length];
            sstr.CopyTo(0, chars, 0, sstr.Length);

            var glyphIndices = chars.Select(x => x.GlyphIndex).ToArray();
            TheResultingCollection(glyphIndices)
                .ShouldBeExactly(2531, 2513, 2150, 2392, 2513, 2150, 2173, 3, 2150, 2172, 2243, 2288, 2533);

            var sourceIndices = chars.Select(x => x.SourceIndex).ToArray();
            TheResultingCollection(sourceIndices)
                .ShouldBeExactly(12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0);

            var advances = chars.Select(x => x.Advance).ToArray();
            TheResultingCollection(advances)
                .ShouldBeExactly(15, 5, 5, 13, 5, 5, 6, 6, 5, 7, 15, 9, 13);

            var offsets_x = chars.Select(x => x.OffsetX).ToArray();
            TheResultingCollection(offsets_x)
                .ShouldContainTheSpecifiedNumberOfItems(13)
                .ShouldContainItemsSatisfyingTheCondition(x => x == 0);

            var offsets_y = chars.Select(x => x.OffsetY).ToArray();
            TheResultingCollection(offsets_y)
                .ShouldContainTheSpecifiedNumberOfItems(13)
                .ShouldContainItemsSatisfyingTheCondition(x => x == 0);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_ProvidesCorrectSourceIndices()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/NotoColorEmoji");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello ");
                        textShaper.Append("👨");
                        textShaper.Append("\u200D");
                        textShaper.Append("👩");
                        textShaper.Append("\u200D");
                        textShaper.Append("👧");
                        textShaper.Append("\u200D");
                        textShaper.Append("👦");
                        textShaper.Append("!");

                        sstr = textShaper.CreateShapedString(font.Regular);
                    }
                })
                .RunForOneFrame();

            var chars = new ShapedChar[sstr.Length];
            sstr.CopyTo(0, chars, 0, sstr.Length);

            var sourceIndices = chars.Select(x => x.SourceIndex).ToArray();
            TheResultingCollection(sourceIndices)
                .ShouldBeExactly(0, 1, 2, 3, 4, 5, 6, 17);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_ProvidesCorrectSourceIndices_WhenGivenAnExplicitOffset()
        {
            var sstr1 = default(ShapedString);
            var sstr2 = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/NotoColorEmoji");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstr1 = textShaper.CreateShapedString(font.Regular);

                        textShaper.Clear();
                        textShaper.Append(" Goodbye, world!");

                        sstr2 = textShaper.CreateShapedString(font.Regular, "Hello, world!".Length);
                    }
                })
                .RunForOneFrame();

            var chars1 = new ShapedChar[sstr1.Length];
            sstr1.CopyTo(0, chars1, 0, sstr1.Length);

            var sourceIndices1 = chars1.Select(x => x.SourceIndex).ToArray();
            TheResultingCollection(sourceIndices1)
                .ShouldBeExactly(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            var chars2 = new ShapedChar[sstr2.Length];
            sstr2.CopyTo(0, chars2, 0, sstr2.Length);

            var sourceIndices2 = chars2.Select(x => x.SourceIndex).ToArray();
            TheResultingCollection(sourceIndices2)
                .ShouldBeExactly(13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28);
        }

        [Test]
        [Category("Content")]
        public void ShapedString_CombinesLigatures()
        {
            var sstr = default(ShapedString);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/NotoColorEmoji");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("👨");
                        textShaper.Append("\u200D");
                        textShaper.Append("👩");
                        textShaper.Append("\u200D");
                        textShaper.Append("👧");
                        textShaper.Append("\u200D");
                        textShaper.Append("👦");

                        sstr = textShaper.CreateShapedString(font.Regular);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstr.Length).ShouldBe(1);
            TheResultingValue(sstr[0].GlyphIndex).ShouldBe(1687);
        }
    }
}
