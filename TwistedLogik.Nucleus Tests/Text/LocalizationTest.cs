using System;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Tests.Text
{
    [TestFixture]
    public class LocalizationTest : NucleusTestFramework
    {
        [Test]
        public void Localization_CorrectlyTranslatesEnglish_FromXml()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("en-US", () =>
            {
                var enUS_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(enUS_sword_sing).ShouldBe("sword");

                var enUS_sword_plur = Localization.Get("SWORD").GetPluralVariant(5);
                TheResultingString(enUS_sword_plur).ShouldBe("swords");

                var enUS_glowing = Localization.Get("GLOWING");
                TheResultingString(enUS_glowing).ShouldBe("glowing");
            });
        }

        [Test]
        public void Localization_CorrectlyTranslatesFrench_FromXml()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("fr-FR", () =>
            {
                var frFR_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(frFR_sword_sing)
                    .ShouldBe("epée")
                    .ShouldHaveProperty("singular")
                    .ShouldHaveProperty("feminine")
                    .ShouldHaveProperty("vowel")
                    .ShouldNotHaveProperty("masculine");

                var frFR_sword_plur = Localization.Get("SWORD").GetPluralVariant(5);
                TheResultingString(frFR_sword_plur)
                    .ShouldBe("epées");

                var frFR_glowing_sing_masc = Localization.Get("GLOWING").GetVariant("sing_masculine");
                TheResultingString(frFR_glowing_sing_masc)
                    .ShouldBe("rougeoyant");

                var frFR_glowing_plur_masc = Localization.Get("GLOWING").GetVariant("plur_masculine");
                TheResultingString(frFR_glowing_plur_masc)
                    .ShouldBe("rougeoyants");

                var frFR_glowing_sing_feminine = Localization.Get("GLOWING").GetVariant("sing_feminine");
                TheResultingString(frFR_glowing_sing_feminine)
                    .ShouldBe("rougeoyante");

                var frFR_glowing_plur_feminine = Localization.Get("GLOWING").GetVariant("plur_feminine");
                TheResultingString(frFR_glowing_plur_feminine)
                    .ShouldBe("rougeoyantes");
            });
        }

        [Test]
        public void Localization_CorrectlySpecifiesEnglishProperties_FromXml()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("en-US", () =>
            {
                var enUS_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(enUS_sword_sing)
                    .ShouldBe("sword")
                    .ShouldHaveProperty("singular")
                    .ShouldNotHaveProperty("plural");
            });
        }

        [Test]
        public void Localization_CorrectlySpecifiesFrenchProperties_FromXml()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("fr-FR", () =>
            {
                var frFR_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(frFR_sword_sing)
                    .ShouldHaveProperty("singular")
                    .ShouldHaveProperty("feminine")
                    .ShouldHaveProperty("vowel")
                    .ShouldNotHaveProperty("plural")
                    .ShouldNotHaveProperty("masculine");
            });
        }

        [Test]
        public void Localization_CorrectlyTranslatesEnglish_FromJson()
        {
            LoadTestLocalizationDatabaseFromJson();

            UsingCulture("en-US", () =>
            {
                var enUS_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(enUS_sword_sing).ShouldBe("sword");

                var enUS_sword_plur = Localization.Get("SWORD").GetPluralVariant(5);
                TheResultingString(enUS_sword_plur).ShouldBe("swords");

                var enUS_glowing = Localization.Get("GLOWING");
                TheResultingString(enUS_glowing).ShouldBe("glowing");
            });
        }

        [Test]
        public void Localization_CorrectlyTranslatesFrench_FromJson()
        {
            LoadTestLocalizationDatabaseFromJson();

            UsingCulture("fr-FR", () =>
            {
                var frFR_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(frFR_sword_sing)
                    .ShouldBe("epée")
                    .ShouldHaveProperty("singular")
                    .ShouldHaveProperty("feminine")
                    .ShouldHaveProperty("vowel")
                    .ShouldNotHaveProperty("masculine");

                var frFR_sword_plur = Localization.Get("SWORD").GetPluralVariant(5);
                TheResultingString(frFR_sword_plur)
                    .ShouldBe("epées");

                var frFR_glowing_sing_masc = Localization.Get("GLOWING").GetVariant("sing_masculine");
                TheResultingString(frFR_glowing_sing_masc)
                    .ShouldBe("rougeoyant");

                var frFR_glowing_plur_masc = Localization.Get("GLOWING").GetVariant("plur_masculine");
                TheResultingString(frFR_glowing_plur_masc)
                    .ShouldBe("rougeoyants");

                var frFR_glowing_sing_feminine = Localization.Get("GLOWING").GetVariant("sing_feminine");
                TheResultingString(frFR_glowing_sing_feminine)
                    .ShouldBe("rougeoyante");

                var frFR_glowing_plur_feminine = Localization.Get("GLOWING").GetVariant("plur_feminine");
                TheResultingString(frFR_glowing_plur_feminine)
                    .ShouldBe("rougeoyantes");
            });
        }

        [Test]
        public void Localization_CorrectlySpecifiesEnglishProperties_FromJson()
        {
            LoadTestLocalizationDatabaseFromJson();

            UsingCulture("en-US", () =>
            {
                var enUS_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(enUS_sword_sing)
                    .ShouldBe("sword")
                    .ShouldHaveProperty("singular")
                    .ShouldNotHaveProperty("plural");
            });
        }

        [Test]
        public void Localization_CorrectlySpecifiesFrenchProperties_FromJson()
        {
            LoadTestLocalizationDatabaseFromJson();

            UsingCulture("fr-FR", () =>
            {
                var frFR_sword_sing = Localization.Get("SWORD").GetPluralVariant(1);
                TheResultingString(frFR_sword_sing)
                    .ShouldHaveProperty("singular")
                    .ShouldHaveProperty("feminine")
                    .ShouldHaveProperty("vowel")
                    .ShouldNotHaveProperty("plural")
                    .ShouldNotHaveProperty("masculine");
            });
        }

        [Test]
        public void Localization_LocalizedTextFormatting()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("fr-FR", () =>
            {
                var formatter = new StringFormatter();
                var buffer = new StringBuilder();

                formatter.Reset();
                formatter.AddArgument(Localization.Get("SWORD").GetPluralVariant(1));
                formatter.AddArgument(Localization.Get("GLOWING"));
                formatter.Format("test: {?:match:0:def_art}{0} {1:match:0:adj}", buffer);

                TheResultingString(buffer)
                    .ShouldBe("test: l'epée rougeoyante");

                formatter.Reset();
                formatter.AddArgument(Localization.Get("SWORD").GetPluralVariant(2));
                formatter.AddArgument(Localization.Get("GLOWING"));
                formatter.Format("test: {?:match:0:def_art}{0} {1:match:0:adj}", buffer);

                TheResultingString(buffer)
                    .ShouldBe("test: les epées rougeoyantes");
            });
        }

        [Test]
        public void StringResource_SerializesToJson()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("fr-FR", () =>
            {
                var resource = new StringResource("LOCALIZED_RESOURCE");
                var json = JsonConvert.SerializeObject(resource);

                TheResultingString(json).ShouldBe($"\"{resource.Key}\"");
            });
        }

        [Test]
        public void StringResource_DeserializesFromJson()
        {
            LoadTestLocalizationDatabaseFromXml();

            UsingCulture("fr-FR", () =>
            {
                const String json = "\"LOCALIZED_RESOURCE\"";

                var resource = JsonConvert.DeserializeObject<StringResource>(json);

                TheResultingString(resource.Value).ShouldBe("C'est un test");
            });
        }

        private void LoadTestLocalizationDatabaseFromXml()
        {
            Localization.Strings.Unload();

            using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream("TwistedLogik.Nucleus.Tests.Resources.LocalizedStrings.xml"))
                Localization.Strings.LoadFromXmlStream(manifestResourceStream);
        }

        private void LoadTestLocalizationDatabaseFromJson()
        {
            Localization.Strings.Unload();

            using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream("TwistedLogik.Nucleus.Tests.Resources.LocalizedStrings.json"))
                Localization.Strings.LoadFromJsonStream(manifestResourceStream);
        }
    }
}
