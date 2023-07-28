using System;
using System.Linq;
using NUnit.Framework;
using Ultraviolet.Presentation.Styles;
using Ultraviolet.Presentation.Uvss;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Presentation.Tests.Styles
{
    [TestFixture]
    public class UvssCompilerTests : UltravioletApplicationTestFramework
    {
        [Test]
        public void UvssCompiler_DefaultsToInvariantCulture()
        {
            GivenAnUltravioletApplicationInServiceMode()
                .OnFrameStart(0, app =>
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

        [Test]
        public void UvssCompiler_ReadsCultureDirective()
        {
            GivenAnUltravioletApplicationInServiceMode()
               .OnFrameStart(0, app =>
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

        [Test]
        public void UvssCompiler_ReadsCultureDirective_WhenMultipleDirectivesExist()
        {
            GivenAnUltravioletApplicationInServiceMode()
               .OnFrameStart(0, app =>
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
