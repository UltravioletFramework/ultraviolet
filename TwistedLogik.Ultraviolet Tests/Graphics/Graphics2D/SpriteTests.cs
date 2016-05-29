using NUnit.Framework;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class SpriteTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprites can be loaded and rendered correctly from XML files.")]
        public void Sprite_LoadsAndRendersCorrectly_FromXml()
        {
            var spriteBatch = default(SpriteBatch);
            var sprite = default(Sprite);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    sprite = content.Load<Sprite>("Sprites/ExplosionXml");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var animation = sprite["Explosion"];
                    var cx = 50;
                    var cy = 50;

                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            var ix = (y * 5) + x;
                            if (ix >= animation.Frames.Count)
                                break;

                            var frame = animation.Frames[ix];
                            spriteBatch.DrawFrame(frame, new Rectangle(cx, cy, frame.Width, frame.Height), Color.White, 0f);
                            cx = cx + 100;
                        }
                        cx = 50;
                        cy = cy + 100;
                    }
                    spriteBatch.End();
                });
            
            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Sprite_LoadsAndRendersCorrectly_FromXml.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprites can be loaded and rendered correctly from XML files.")]
        public void Sprite_LoadsAndRendersCorrectly_FromPreprocessedAsset()
        {
            var spriteBatch = default(SpriteBatch);
            var sprite = default(Sprite);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();

                    if (!content.Preprocess<Sprite>("Sprites/ExplosionXml_Preprocessed"))
                        Assert.Fail("Failed to preprocess asset.");

                    sprite = content.Load<Sprite>("Sprites/ExplosionXml_Preprocessed.uvc");
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    var animation = sprite["Explosion"];
                    var cx = 50;
                    var cy = 50;

                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            var ix = (y * 5) + x;
                            if (ix >= animation.Frames.Count)
                                break;

                            var frame = animation.Frames[ix];
                            spriteBatch.DrawFrame(frame, new Rectangle(cx, cy, frame.Width, frame.Height), Color.White, 0f);
                            cx = cx + 100;
                        }
                        cx = 50;
                        cy = cy + 100;
                    }
                    spriteBatch.End();
                });
            
            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Sprite_LoadsAndRendersCorrectly_FromPreprocessedAsset.png");
        }
    }
}
