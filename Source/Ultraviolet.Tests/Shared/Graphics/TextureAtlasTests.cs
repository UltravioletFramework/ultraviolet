using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public class TextureAtlasTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that texture atlases can be loaded and rendered correctly from XML files.")]
        public void TextureAtlas_LoadsAndRendersCorrectly_FromXml()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    atlas = content.Load<TextureAtlas>("Sprites/Textures/ExplosionAtlasXml");
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
        [Description("Ensures that texture atlases can be loaded and rendered correctly from JSON files.")]
        public void TextureAtlas_LoadsAndRendersCorrectly_FromJson()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    atlas = content.Load<TextureAtlas>("Sprites/Textures/ExplosionAtlasJson");
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

        [Test]
        [Category("Rendering")]
        [Description("Ensures that texture atlases can be loaded and rendered correctly when using content preprocessing.")]
        public void TextureAtlas_LoadsAndRendersCorrectly_FromPreprocessedAsset()
        {
            var sbatch = default(SpriteBatch);
            var atlas = default(TextureAtlas);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();

                    var atlasAssetPath = CreateMachineSpecificAssetCopy(content, "Sprites/Textures/ExplosionAtlasXml_Preprocessed.xml");
                    if (!content.Preprocess<TextureAtlas>(atlasAssetPath, false))
                        Assert.Fail("Failed to preprocess asset.");

                    atlas = content.Load<TextureAtlas>(atlasAssetPath + ".uvc");
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
                .ShouldMatch(@"Resources/Expected/Graphics/TextureAtlas_LoadsAndRendersCorrectly_FromPreprocessedAsset.png");
        }
    }
}
