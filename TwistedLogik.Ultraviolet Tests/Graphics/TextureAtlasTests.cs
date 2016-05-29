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
        public void TextureAtlas_LoadsAndRendersCorrectly_FromXml()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    atlas = content.Load<TextureAtlas>("Sprites/Textures/ExplosionXml");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var cx = 0;
                    var cy = 0;

                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            sbatch.Draw(atlas, new Vector2(cx, cy), atlas[$"Explosion_{(y * 5) + x + 1:D2}"], Color.White);
                            cx = cx + 100;
                        }
                        cx = 0;
                        cy = cy + 100;
                    }

                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/TextureAtlas_LoadsAndRendersCorrectly_FromXml.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that texture atlases can be loaded and rendered correctly.")]
        public void TextureAtlas_LoadsAndRendersCorrectly_FromJson()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    atlas = content.Load<TextureAtlas>("Sprites/Textures/ExplosionJson");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var cx = 0;
                    var cy = 0;

                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            sbatch.Draw(atlas, new Vector2(cx, cy), atlas[$"Explosion_{(y * 5) + x + 1:D2}"], Color.White);
                            cx = cx + 100;
                        }
                        cx = 0;
                        cy = cy + 100;
                    }

                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/TextureAtlas_LoadsAndRendersCorrectly_FromJson.png");
        }
    }
}
