using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public class Surface2DTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that Surface2D produces the correct texture data when the surface is flipped..")]
        public void Surface2D_RendersCorrectly_WhenFlipped()
        {
            var sbatch = default(SpriteBatch);
            var surface = default(Surface2D);
            var texture0 = default(Texture2D);
            var texture1 = default(Texture2D);
            var texture2 = default(Texture2D);
            var texture3 = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    surface = content.Load<Surface2D>("Surfaces/Test");

                    surface.ProcessAlpha(true, null);
                    TheResultingValue(surface.IsFlippedHorizontally).ShouldBe(false);
                    TheResultingValue(surface.IsFlippedVertically).ShouldBe(false);
                    texture0 = surface.CreateTexture();

                    surface.Flip(SurfaceFlipDirection.Horizontal);
                    TheResultingValue(surface.IsFlippedHorizontally).ShouldBe(true);
                    TheResultingValue(surface.IsFlippedVertically).ShouldBe(false);
                    texture1 = surface.CreateTexture();

                    surface.Flip(SurfaceFlipDirection.Vertical);
                    TheResultingValue(surface.IsFlippedHorizontally).ShouldBe(true);
                    TheResultingValue(surface.IsFlippedVertically).ShouldBe(true);
                    texture2 = surface.CreateTexture();

                    surface.Flip(SurfaceFlipDirection.Horizontal);
                    TheResultingValue(surface.IsFlippedHorizontally).ShouldBe(false);
                    TheResultingValue(surface.IsFlippedVertically).ShouldBe(true);
                    texture3 = surface.CreateTexture();
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

                    sbatch.Draw(texture0, new RectangleF(0, 0, 64, 64), Color.White);
                    sbatch.Draw(texture1, new RectangleF(64, 0, 64, 64), Color.White);
                    sbatch.Draw(texture2, new RectangleF(64, 64, 64, 64), Color.White);
                    sbatch.Draw(texture3, new RectangleF(0, 64, 64, 64), Color.White);

                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Surface2D_RendersCorrectly_WhenFlipped.png");
        }
    }
}
