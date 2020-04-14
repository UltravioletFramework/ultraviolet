using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public class Texture2DTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that Texture2D loads without premultiplying its alpha when it is loaded with content metadata.")]
        public void Texture2D_LoadsNonPremultiplied_WhenGivenContentMetadata()
        {
            var sbatch = default(SpriteBatch);
            var texture = default(Texture2D);
            var textureNonPremultiplied = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();
                    texture = content.Load<Texture2D>("Textures/Face");
                    textureNonPremultiplied = content.Load<Texture2D>("Textures/FaceNonPremultiplied");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    sbatch.Draw(texture, new RectangleF(0, 0, 128, 128), Color.White);
                    sbatch.Draw(textureNonPremultiplied, new RectangleF(128, 0, 128, 128), Color.White);

                    sbatch.End();

                    sbatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

                    sbatch.Draw(texture, new RectangleF(0, 128, 128, 128), Color.White);
                    sbatch.Draw(textureNonPremultiplied, new RectangleF(128, 128, 128, 128), Color.White);

                    sbatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Texture2D_LoadsNonPremultiplied_WhenGivenContentMetadata.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprite fonts can be loaded and rendered correctly from preprocessed files.")]
        public void Texture2D_LoadsAndRendersCorrectly_FromPreprocessedAsset()
        {
            var sbatch = default(SpriteBatch);
            var texture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    sbatch = SpriteBatch.Create();

                    var textureAssetPath = CreateMachineSpecificAssetCopy(content, "Textures/Triangle_Preprocessed");
                    if (!content.Preprocess<Texture2D>(textureAssetPath))
                        Assert.Fail("Failed to preprocess asset.");

                    texture = content.Load<Texture2D>(textureAssetPath + ".uvc");
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.CornflowerBlue);

                    sbatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    sbatch.Draw(texture, new RectangleF(0, 0, 256, 256), Color.White);
                    sbatch.End();
                });
            
            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Texture2D_LoadsAndRendersCorrectly_FromPreprocessedAsset.png");
        }
    }
}
