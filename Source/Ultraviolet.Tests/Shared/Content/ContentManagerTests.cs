using NUnit.Framework;
using Ultraviolet.FreeType2;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Content
{
    [TestFixture]
    public class ContentManagerTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        public void ContentManager_ProcessesMetadataFiles_WhenAssetIsAStream()
        {
            var contentStream = typeof(ContentManagerTests).Assembly.GetManifestResourceStream("Ultraviolet.Tests.Resources.Content.Fonts.FiraSansEmbedded.uvmeta");

            GivenAnUltravioletApplicationWithNoWindow()
                .WithPlugin(new FreeTypeFontPlugin())
                .WithContent(content =>
                {
                    var font = content.LoadFromStream<UltravioletFont>(contentStream, "uvmeta");

                    TheResultingString(font.Regular.ToString())
                        .ShouldBe("Fira Sans Regular 32pt");
                })
                .RunForOneFrame();
        }
    }
}
