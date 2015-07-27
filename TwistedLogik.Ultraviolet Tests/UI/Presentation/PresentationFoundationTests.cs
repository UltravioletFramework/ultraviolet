using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    [TestClass]
    [DeploymentItem(@"TwistedLogik.Ultraviolet.UI.Presentation.Compiler.dll")]
    public class PresentationFoundationTests : UltravioletApplicationTestFramework
    {
        [ClassInitialize]
        public static void Initialize(TestContext textContext)
        {
            var application = GivenAThrowawayUltravioletApplicationWithNoWindow()
               .WithPresentationFoundationConfigured()
               .WithInitialization(uv =>
               {
                   var upf = uv.GetUI().GetPresentationFoundation();
                   upf.CompileExpressions("Content");
               });

            application.RunForOneFrame();

            DestroyUltravioletApplication(application);
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Canvas_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new CanvasTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Canvas_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Grid_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new GridTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Grid_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Vertical_StackPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new VerticalStackPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Vertical_StackPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Horizontal_StackPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new HorizontalStackPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Horizontal_StackPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Vertical_WrapPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new VerticalWrapPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Vertical_WrapPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_Horizontal_WrapPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new HorizontalWrapPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_Horizontal_WrapPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_DockPanel_CorrectlyArrangesChildren()
        {
            var result = RunPresentationTestFor(content => new DockPanelTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_DockPanel_CorrectlyArrangesChildren.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_CorrectlyLaysOutComplexScreen()
        {
            var result = RunPresentationTestFor(content => new ComplexScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_CorrectlyLaysOutComplexScreen.png");
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void PresentationFoundation_CorrectlyDrawsElementsWithRenderTransforms()
        {
            var result = RunPresentationTestFor(content => new RenderTransformTestScreen(content));

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\UI\Presentation\PresentationFoundation_CorrectlyDrawsElementsWithRenderTransforms.png");
        }

        /// <summary>
        /// Runs a standard test by spinning up an Ultraviolet application and displaying the specified UPF screen.
        /// </summary>
        private Bitmap RunPresentationTestFor<T>(Func<ContentManager, T> ctor) where T : UIScreen
        {
            return GivenAnUltravioletApplication()
                .WithPresentationFoundationConfigured()
                .WithInitialization(uv =>
                {
                    var upf = uv.GetUI().GetPresentationFoundation();
                    upf.LoadCompiledExpressions();
                })
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
