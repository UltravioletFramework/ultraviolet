using System;
using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class TextShaperTests : UltravioletApplicationTestFramework
    {
        [Test]
        public void TextShaper_AppendsMultipleStrings()
        {
            var textLength = default(Int32);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        textShaper.SetLanguage("en");
                        textShaper.Append("Hello,");
                        textShaper.Append(" ");
                        textShaper.Append("world!");

                        textLength = textShaper.Length;
                    }
                })
                .RunForOneFrame();

            TheResultingValue(textLength).ShouldBe(13);
        }

        [Test]
        public void TextShaper_CorrectlyGuessesProperties_WithEnglishText()
        {
            var textLanguage = default(String);
            var textScript = default(TextScript);
            var textDirection = default(TextDirection);
            var textLength = default(Int32);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        textShaper.SetLanguage("en");
                        textShaper.Append("Hello, world!");
                        textShaper.GuessUnicodeProperties();

                        textLanguage = textShaper.GetLanguage();
                        textScript = textShaper.GetScript();
                        textDirection = textShaper.GetDirection();
                        textLength = textShaper.Length;
                    }
                })
                .RunForOneFrame();

            TheResultingString(textLanguage).ShouldBe("en");
            TheResultingValue(textScript).ShouldBe(TextScript.Latin);
            TheResultingValue(textDirection).ShouldBe(TextDirection.LeftToRight);
            TheResultingValue(textLength).ShouldBe(13);
        }

        [Test]
        public void TextShaper_CorrectlyGuessesProperties_WithArabicText()
        {
            var textLanguage = default(String);
            var textScript = default(TextScript);
            var textDirection = default(TextDirection);
            var textLength = default(Int32);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        textShaper.SetLanguage("ar");
                        textShaper.Append("مرحبا بالعالم");
                        textShaper.GuessUnicodeProperties();

                        textLanguage = textShaper.GetLanguage();
                        textScript = textShaper.GetScript();
                        textDirection = textShaper.GetDirection();
                        textLength = textShaper.Length;
                    }
                })
                .RunForOneFrame();

            TheResultingString(textLanguage).ShouldBe("ar");
            TheResultingValue(textScript).ShouldBe(TextScript.Arabic);
            TheResultingValue(textDirection).ShouldBe(TextDirection.RightToLeft);
            TheResultingValue(textLength).ShouldBe(13);
        }
        
        [Test]
        public void TextShaper_ResetsUnicodeProperties_WhenCleared()
        {
            var defaultLanguage = default(String);

            var textLanguage = default(String);
            var textScript = default(TextScript);
            var textDirection = default(TextDirection);
            var textLength = default(Int32);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        defaultLanguage = textShaper.GetLanguage();

                        textShaper.SetLanguage("ar");
                        textShaper.SetScript(TextScript.Devanagari);
                        textShaper.SetDirection(TextDirection.TopToBottom);
                        textShaper.Append("مرحبا بالعالم");
                        textShaper.GuessUnicodeProperties();
                        textShaper.Clear();

                        textLanguage = textShaper.GetLanguage();
                        textScript = textShaper.GetScript();
                        textDirection = textShaper.GetDirection();
                        textLength = textShaper.Length;
                    }
                })
                .RunForOneFrame();

            TheResultingString(textLanguage).ShouldBe(defaultLanguage);
            TheResultingValue(textScript).ShouldBe(TextScript.Invalid);
            TheResultingValue(textDirection).ShouldBe(TextDirection.Invalid);
            TheResultingValue(textLength).ShouldBe(0);
        }

        [Test]
        public void TextShaper_UserSpecifiedPropertiesArePreserved_WhenPropertiesAreGuessed()
        {
            var textLanguage = default(String);
            var textScript = default(TextScript);
            var textDirection = default(TextDirection);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    using (var textShaper = new HarfBuzzTextShaper(uv))
                    {
                        textShaper.SetLanguage("ar");
                        textShaper.SetScript(TextScript.Devanagari);
                        textShaper.SetDirection(TextDirection.TopToBottom);
                        textShaper.Append("مرحبا بالعالم");
                        textShaper.GuessUnicodeProperties();

                        textLanguage = textShaper.GetLanguage();
                        textScript = textShaper.GetScript();
                        textDirection = textShaper.GetDirection();
                    }
                })
                .RunForOneFrame();

            TheResultingString(textLanguage).ShouldBe("ar");
            TheResultingValue(textScript).ShouldBe(TextScript.Devanagari);
            TheResultingValue(textDirection).ShouldBe(TextDirection.TopToBottom);
        }
    }
}
