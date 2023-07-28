using System;
using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public class DynamicTextureAtlasTests : UltravioletApplicationTestFramework
    {
        [Test, Category("Rendering")]
        [Description("Ensures that dynamic texture atlases are constructed correctly.")]
        public void DynamicTextureAtlas_IsConstructedCorrectly()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(DynamicTextureAtlas);
            var surface = default(Surface2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();

                    atlas = DynamicTextureAtlas.Create(512, 512, 8);

                    surface = content.Load<Surface2D>("Surfaces/bear");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/frog");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/pig");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/panda");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/dog");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/walrus");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    sbatch.Draw(atlas, Vector2.Zero, Color.White);
                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/DynamicTextureAtlas_IsConstructedCorrectly.png");
        }

        [Test, Category("Rendering")]
        [Description("Ensures that dynamic texture atlases correctly fail to reserve space when they are full.")]
        public void DynamicTextureAtlas_FailsToReserveSpace_WhenAtlasIsFull()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(DynamicTextureAtlas);
            var surface = default(Surface2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();

                    atlas = DynamicTextureAtlas.Create(512, 256, 8);

                    surface = content.Load<Surface2D>("Surfaces/bear");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/frog");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/pig");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(true);

                    surface = content.Load<Surface2D>("Surfaces/panda");
                    TheResultingValue(BlitSurfaceToDynamicTextureAtlas(atlas, surface)).ShouldBe(false);
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    sbatch.Draw(atlas, Vector2.Zero, Color.White);
                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/DynamicTextureAtlas_FailsToReserveSpace_WhenAtlasIsFull.png");
        }

        /// <summary>
        /// Attempts to reserve a position on a texture atlas for the specified surface, and if the reservation
        /// is successful, blits the surface to the atlas.
        /// </summary>
        private static Boolean BlitSurfaceToDynamicTextureAtlas(DynamicTextureAtlas atlas, Surface2D surface)
        {
            if (!atlas.TryReserveCell(surface.Width, surface.Height, out var reservation))
                return false;

            surface.ProcessAlpha(true, Color.Magenta);
            surface.Blit(atlas.Surface, new Point2(reservation.X, reservation.Y), atlas.IsFlipped ?
                SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None);

            return true;
        }
    }
}
