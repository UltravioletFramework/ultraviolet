using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Styles
{
	[TestClass]
	public class UvssCompilerTests : UltravioletTestFramework
	{
		[TestMethod]
		public void UvssCompiler_DefaultsToInvariantCulture()
		{
			UsingCulture("ru-RU", () =>
			{
				var tree = UvssParser.Parse("@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
				var document = UvssCompiler.Compile(tree);

				var keyframe = document?
					.Storyboards?.FirstOrDefault()?
					.Targets?.FirstOrDefault()?
					.Animations?.FirstOrDefault()?
					.Keyframes?.FirstOrDefault();

				TheResultingObject(keyframe)
					.ShouldNotBeNull()
					.ShouldSatisfyTheCondition(x => x.Value.Culture.Name == String.Empty);
			});
		}

		[TestMethod]
		public void UvssCompiler_ReadsCultureDirective()
		{
			UsingCulture("en-US", () =>
			{
				var tree = UvssParser.Parse(
					"$culture { ru-RU }\r\n" +
					"@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
				var document = UvssCompiler.Compile(tree);

				var keyframe = document?
					.Storyboards?.FirstOrDefault()?
					.Targets?.FirstOrDefault()?
					.Animations?.FirstOrDefault()?
					.Keyframes?.FirstOrDefault();

				TheResultingObject(keyframe)
					.ShouldNotBeNull()
					.ShouldSatisfyTheCondition(x => x.Value.Culture.Name == "ru-RU");
			});
		}

		[TestMethod]
		public void UvssCompiler_ReadsCultureDirective_WhenMultipleDirectivesExist()
		{
			UsingCulture("en-US", () =>
			{
				var tree = UvssParser.Parse(
					"$culture { ru-RU }\r\n" +
					"$culture { fr-FR }\r\n" +
					"@foo { target { animation Width { keyframe 0 { 100.0 } } } }");
				var document = UvssCompiler.Compile(tree);

				var keyframe = document?
					.Storyboards?.FirstOrDefault()?
					.Targets?.FirstOrDefault()?
					.Animations?.FirstOrDefault()?
					.Keyframes?.FirstOrDefault();

				TheResultingObject(keyframe)
					.ShouldNotBeNull()
					.ShouldSatisfyTheCondition(x => x.Value.Culture.Name == "fr-FR");
			});
		}
	}
}
