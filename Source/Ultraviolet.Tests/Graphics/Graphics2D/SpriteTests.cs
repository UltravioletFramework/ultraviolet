using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics.Graphics2D
{
    [TestFixture]
    public class SpriteTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprites can be fully created from program code.")]
        public void Sprite_CanBeConstructedProgrammatically()
        {
            var spriteBatch = default(SpriteBatch);
            var sprite = default(Sprite);
            var texture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    texture = content.Load<Texture2D>("Textures/HexagonsTexture");
                    
                    var anim1 = new SpriteAnimation("anim1", SpriteAnimationRepeat.Loop);
                    anim1.Frames.Add(new SpriteFrame(texture, 0, 0, 120, 140, 0));
                    anim1.Controller.PlayAnimation(anim1);

                    var anim2 = new SpriteAnimation("anim2", SpriteAnimationRepeat.Loop);
                    anim2.Frames.Add(new SpriteFrame(texture, 120, 0, 120, 140, 0));
                    anim2.Controller.PlayAnimation(anim2);

                    spriteBatch = SpriteBatch.Create();
                    sprite = new Sprite(new[] { anim1, anim2 });
                })
                .Render(uv =>
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawSprite(sprite[0].Controller, new Vector2(0, 0));
                    spriteBatch.DrawSprite(sprite[1].Controller, new Vector2(120, 0));

                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Sprite_CanBeConstructedProgrammatically.png");
        }

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
        [Description("Ensures that sprites can be loaded and rendered correctly from JSON files.")]
        public void Sprite_LoadsAndRendersCorrectly_FromJson()
        {
            var spriteBatch = default(SpriteBatch);
            var sprite = default(Sprite);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    sprite = content.Load<Sprite>("Sprites/ExplosionJson");
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
                .ShouldMatch(@"Resources/Expected/Graphics/Graphics2D/Sprite_LoadsAndRendersCorrectly_FromJson.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that sprites can be loaded and rendered correctly from preprocessed files.")]
        public void Sprite_LoadsAndRendersCorrectly_FromPreprocessedAsset()
        {
            var spriteBatch = default(SpriteBatch);
            var sprite = default(Sprite);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();

                    var spriteAssetPath = CreateMachineSpecificAssetCopy(content, "Sprites/ExplosionXml_Preprocessed.sprite");
                    if (!content.Preprocess<Sprite>(spriteAssetPath))
                        Assert.Fail("Failed to preprocess asset.");

                    sprite = content.Load<Sprite>(spriteAssetPath + ".uvc");
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
