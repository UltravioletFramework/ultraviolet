using NUnit.Framework;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public class TextureAtlasTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that texture atlases can be loaded and rendered correctly.")]
        public void TextureAtlas_LoadsAndRendersCorrectly()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    atlas = content.Load<TextureAtlas>("Sprites/Textures/Explosion");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    var cx = 0;
                    var cy = 0;

                    sbatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_01"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_02"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_03"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_04"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_05"], Color.White);
                    cx = 0;
                    cy = cy + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_06"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_07"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_08"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_09"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_10"], Color.White);
                    cx = 0;
                    cy = cy + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_11"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_12"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_13"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_14"], Color.White);
                    cx = cx + 100;

                    sbatch.Draw(atlas, new Vector2(cx, cy), atlas["Explosion_15"], Color.White);
                    cx = 0;
                    cy = cy + 100;

                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/TextureAtlas_LoadsAndRendersCorrectly.png");
        }
    }
}
