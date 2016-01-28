using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Styles
{
    [TestClass]
    public class UvssCompilerTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        public void UvssCompiler_DefaultsToInvariantCulture()
        {
            GivenAnUltravioletApplicationInServiceMode()
                .OnFrame(0, app =>
                {
                    UsingCulture("ru-RU", () =>
                    {
                        var tree = UvssParser.Parse("@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
                        var document = UvssCompiler.Compile(app.Ultraviolet, tree);

                        var keyframe = document?
                            .StoryboardDefinitions?.FirstOrDefault()?
                            .Targets?.FirstOrDefault()?
                            .Animations?.FirstOrDefault()?
                            .Keyframes?.FirstOrDefault();

                        TheResultingObject(keyframe)
                            .ShouldNotBeNull()
                            .ShouldSatisfyTheCondition(x => x.Value.Culture.Name == String.Empty);
                    });
                })
                .RunForOneFrame();
        }

        [TestMethod]
        public void UvssCompiler_ReadsCultureDirective()
        {
            GivenAnUltravioletApplicationInServiceMode()
               .OnFrame(0, app =>
               {
                   UsingCulture("en-US", () =>
                   {
                       var tree = UvssParser.Parse(
                           "$culture { ru-RU }\r\n" +
                           "@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
                       var document = UvssCompiler.Compile(app.Ultraviolet, tree);

                       var keyframe = document?
                           .StoryboardDefinitions?.FirstOrDefault()?
                           .Targets?.FirstOrDefault()?
                           .Animations?.FirstOrDefault()?
                           .Keyframes?.FirstOrDefault();

                       TheResultingObject(keyframe)
                           .ShouldNotBeNull()
                           .ShouldSatisfyTheCondition(x => x.Value.Culture.Name == "ru-RU");
                   });
               })
               .RunForOneFrame();
        }

        [TestMethod]
        public void UvssCompiler_ReadsCultureDirective_WhenMultipleDirectivesExist()
        {
            GivenAnUltravioletApplicationInServiceMode()
               .OnFrame(0, app =>
               {
                    UsingCulture("en-US", () =>
                    {
                        var tree = UvssParser.Parse(
                            "$culture { ru-RU }\r\n" +
                            "$culture { fr-FR }\r\n" +
                            "@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
                        var document = UvssCompiler.Compile(app.Ultraviolet, tree);

                        var keyframe = document?
                            .StoryboardDefinitions?.FirstOrDefault()?
                            .Targets?.FirstOrDefault()?
                            .Animations?.FirstOrDefault()?
                            .Keyframes?.FirstOrDefault();

                        TheResultingObject(keyframe)
                            .ShouldNotBeNull()
                            .ShouldSatisfyTheCondition(x => x.Value.Culture.Name == "fr-FR");
                    });
               })
               .RunForOneFrame();
        }
    }
}
