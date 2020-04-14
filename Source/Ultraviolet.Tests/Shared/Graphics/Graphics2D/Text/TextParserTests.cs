using System.Linq;
using NUnit.Framework;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestFixture]
    public class TextParserTests : UltravioletApplicationTestFramework
    {
        [Test]
        public void TextParser_ParseIncremental_CorrectlyHandlesAddedText()
        {
            var textParser = new TextParser();
            var textParserResult = new TextParserTokenStream();
            textParser.Parse(@"lorem ipsum |b|dolor|b| sit amet", textParserResult);

            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", null, "dolor", null, " ", "sit", " ", "amet");

            textParser.ParseIncremental(@"lorem ipsum hello world!! |b|dolor|b| sit amet", 12, 14, textParserResult);

            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", "hello", " ", "world!!", " ", null, "dolor", null, " ", "sit", " ", "amet");
        }

        [Test]
        public void TextParser_ParseIncremental_CorrectlyHandlesRemovedText()
        {
            var textParser = new TextParser();
            var textParserResult = new TextParserTokenStream();
            textParser.Parse(@"lorem ipsum |b|dolor|b| sit amet", textParserResult);

            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", null, "dolor", null, " ", "sit", " ", "amet");

            textParser.ParseIncremental(@"lorem ipsum amet", 12, 16, textParserResult);

            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", "amet");
        }

        [Test]
        public void TextParser_ParseIncremental_CorrectlyHandlesReplacedText()
        {
            var textParser = new TextParser();
            var textParserResult = new TextParserTokenStream();
            textParser.Parse(@"lorem ipsum |b|dolor|b| sit amet", textParserResult);

            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", null, "dolor", null, " ", "sit", " ", "amet");

            textParser.ParseIncremental(@"lorem ipsum |b|foo bar baz qux|b| sit amet", 15, 21, textParserResult);
            
            TheResultingCollection(textParserResult.Select(x => x.Text)).ShouldBeExactly("lorem", " ", "ipsum", " ", null, "foo", " ", "bar", " ", "baz", " ", "qux", null, " ", "sit", " ", "amet");
        }
    }
}
