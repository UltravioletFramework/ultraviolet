using System;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class TextShaperTests : UltravioletApplicationTestFramework
    {
        [Test]
        public void TextShaper_AppendsMultipleStrings()
        {
            var textLength = default(Int32);

            GivenAnUltravioletApplicationInServiceMode()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        textShaper.SetUnicodeProperties(TextDirection.LeftToRight, TextScript.Latin, "en");
                        textShaper.Append("Hello,");
                        textShaper.Append(" ");
                        textShaper.Append("world!");

                        textLength = textShaper.RawLength;
                    }
                })
                .RunForOneFrame();

            TheResultingValue(textLength).ShouldBe(13);
        }
    }
}
