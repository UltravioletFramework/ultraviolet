using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    [TestClass]
    public class PresentationFoundationTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_CorrectlyLaysOutComplexScreen()
        {
            var result = GivenAnUltravioletApplication()
                .WithPresentationFoundationConfigured()
                .WithContent(content =>
                {
                    var contentManifestFiles = content.GetAssetFilePathsInDirectory("Manifests");
                    content.Ultraviolet.GetContent().Manifests.Load(contentManifestFiles);

                    var screen = new ComplexScreen(content);
                    content.Ultraviolet.GetUI().GetScreens().Open(screen);
                })
                .SkipFrames(10).Render(uv => 
                {
                    using (var spriteBatch = SpriteBatch.Create())
                    {
                        uv.GetUI().GetScreens().Draw(new UltravioletTime(), spriteBatch);
                    }
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_CorrectlyLaysOutComplexScreen.png");
        }
    }
}
