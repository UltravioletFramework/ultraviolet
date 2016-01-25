using System;
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
		public void UvssCompiler_DefaultsToAmericanEnglishCulture()
		{
			UsingCulture("ru-RU", () =>
			{
				var tree = UvssParser.Parse(String.Empty);
				var document = UvssCompiler.Compile(tree);

				TheResultingString(document.Culture.Name)
					.ShouldBe("en-US");
			});
		}

		[TestMethod]
		public void UvssCompiler_ReadsCultureDirective()
		{
			UsingCulture("en-US", () =>
			{
				var tree = UvssParser.Parse(
					"$culture { ru-RU }\r\n" +
					"#foo { }");
				var document = UvssCompiler.Compile(tree);

				TheResultingString(document.Culture.Name)
					.ShouldBe("ru-RU");
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
				"#foo { }");
				var document = UvssCompiler.Compile(tree);

				TheResultingString(document.Culture.Name)
					.ShouldBe("fr-FR");
			});
		}
	}
}
