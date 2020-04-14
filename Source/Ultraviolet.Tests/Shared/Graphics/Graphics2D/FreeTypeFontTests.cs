using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class FreeTypeFontTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        [Description("Ensures that FreeType2 fonts measure ShapedString instances correctly.")]
        public void FreeTypeFont_MeasuresShapedStringsCorrectly()
        {
            var freetypeFont = default(FreeTypeFont);
            var size = Size2.Zero;

            GivenAnUltravioletApplication()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    freetypeFont = content.Load<FreeTypeFont>("Fonts/FiraSans");

                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        var str = textShaper.CreateShapedString(freetypeFont.Regular);
                        size = freetypeFont.Regular.MeasureShapedString(str);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(size.Width).ShouldBe(92);
            TheResultingValue(size.Height).ShouldBe(20);
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that FreeType2 fonts measure ShapedStringBuilder instances correctly")]
        public void FreeTypeFont_MeasuresShapedStringBuildersCorrectly()
        {
            var freetypeFont = default(FreeTypeFont);
            var size = Size2.Zero;

            GivenAnUltravioletApplication()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    freetypeFont = content.Load<FreeTypeFont>("Fonts/FiraSans");

                    using (var textShaper = new HarfBuzzTextShaper(content.Ultraviolet))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello, world!");

                        var str = new ShapedStringBuilder();
                        str.Append(textShaper, freetypeFont.Regular);

                        size = freetypeFont.Regular.MeasureShapedString(str);
                    }
                })
                .RunForOneFrame();

            TheResultingValue(size.Width).ShouldBe(92);
            TheResultingValue(size.Height).ShouldBe(20);
        }
    }
}
