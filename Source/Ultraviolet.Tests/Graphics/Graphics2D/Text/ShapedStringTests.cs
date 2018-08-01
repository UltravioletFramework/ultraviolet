using System.Linq;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestFramework;

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
                        textShaper.SetLanguage("en");
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
                        textShaper.SetLanguage("en");
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
                        textShaper.SetLanguage("en");
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
                        textShaper.SetLanguage("ar");
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
                        textShaper.SetLanguage("en");
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
