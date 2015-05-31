using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    [TestClass]
    public class PresentationFoundationTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Canvas_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<CanvasTestScreen>(content => new CanvasTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Canvas_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Grid_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<GridTestScreen>(content => new GridTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Grid_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Vertical_StackPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<VerticalStackPanelTestScreen>(content => new VerticalStackPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Vertical_StackPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Horizontal_StackPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<HorizontalStackPanelTestScreen>(content => new HorizontalStackPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Horizontal_StackPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Vertical_WrapPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<VerticalWrapPanelTestScreen>(content => new VerticalWrapPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Vertical_WrapPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Horizontal_WrapPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<HorizontalWrapPanelTestScreen>(content => new HorizontalWrapPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Horizontal_WrapPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_DockPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor<DockPanelTestScreen>(content => new DockPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_DockPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_CorrectlyLaysOutComplexScreen()
        {
            var result = RunPresentationTestFor<ComplexScreen>(content => new ComplexScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_CorrectlyLaysOutComplexScreen.png");
        }

        /// <summary>
        /// Runs a standard test by spinning up an Ultraviolet application and displaying the specified UPF screen.
        /// </summary>
        private Bitmap RunPresentationTestFor<T>(Func<ContentManager, T> ctor) where T : UIScreen
        {
            return GivenAnUltravioletApplication()
                .WithPresentationFoundationConfigured()
                .WithContent(content =>
                {
                    var contentManifestFiles = content.GetAssetFilePathsInDirectory("Manifests");
                    content.Ultraviolet.GetContent().Manifests.Load(contentManifestFiles);

                    var globalStyleSheet = content.Load<UvssDocument>(@"UI\DefaultUIStyles");
                    content.Ultraviolet.GetUI().GetPresentationFoundation().SetGlobalStyleSheet(globalStyleSheet);

                    var screen = ctor(content);
                    content.Ultraviolet.GetUI().GetScreens().Open(screen);
                })
                .SkipFrames(1).Render(uv =>
                {
                    using (var spriteBatch = SpriteBatch.Create())
                    {
                        uv.GetUI().GetScreens().Draw(new UltravioletTime(), spriteBatch);
                    }
                });
        }
    }
}
