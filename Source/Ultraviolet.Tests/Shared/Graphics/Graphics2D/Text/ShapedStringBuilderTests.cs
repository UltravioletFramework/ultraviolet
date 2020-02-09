using System;
using System.Linq;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class ShapedStringBuilderTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_CanSpecifyInitialValueAndCapacityInConstructor()
        {
            var sstrb = default(ShapedStringBuilder);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstrb = new ShapedStringBuilder(textShaper.CreateShapedString(font), 128);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstrb.Capacity).ShouldBe(128);
            TheResultingValue(sstrb.Length).ShouldBe(13);
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_IsAppendedFromShaper_WhenSpecifyingSubstring()
        {
            var sstrb = new ShapedStringBuilder();

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        textShaper.AppendTo(sstrb, font.Regular, 7, 5);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstrb.Length).ShouldBe(5);
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_CanBeCleared()
        {
            var sstrb = new ShapedStringBuilder();

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstrb = new ShapedStringBuilder(textShaper.CreateShapedString(font), 128);
                        sstrb.Clear();
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstrb.Capacity).ShouldBe(128);
            TheResultingValue(sstrb.Length).ShouldBe(0);
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_CanBeManuallyExpanded()
        {
            var sstrb = new ShapedStringBuilder();

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstrb = new ShapedStringBuilder(textShaper.CreateShapedString(font));
                        sstrb.Capacity = 128;
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstrb.Capacity).ShouldBe(128);
            TheResultingValue(sstrb.Length).ShouldBe(13);
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_CannotBeManuallyShrunkSmallerThanLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var sstrb = new ShapedStringBuilder();
                sstrb.Append(new ShapedChar(), 8);
                sstrb.Capacity = 4;
            });
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_ExpandsWhenCapacityIsExceeded()
        {
            var sstrb = new ShapedStringBuilder();

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.Load<UltravioletFont>("Fonts/FiraSans");
                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        sstrb.Append(textShaper, font);
                        textShaper.Clear();

                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append(" Goodbye, world!");

                        sstrb.Append(textShaper, font);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(sstrb.Capacity).ShouldBe(36);
            TheResultingValue(sstrb.Length).ShouldBe(29);
        }

        [Test]
        [Category("Content")]
        public void ShapedStringBuilder_CanCreateShapedString()
        {
            var sstrb = new ShapedStringBuilder();
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

                        sstrb.Append(textShaper, font);
                        sstr = sstrb.ToShapedString(font, "en", TextScript.Latin, TextDirection.LeftToRight);
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
    }
}
