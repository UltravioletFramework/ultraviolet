using System;
using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public partial class UltravioletGraphicsTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that the Graphics subsystem can render a single untextured triangle.")]
        public void UltravioletGraphics_CanRenderAColoredTriangle()
        {
            var effect = default(BasicEffect);
            var vertexBuffer = default(VertexBuffer);
            var geometryStream = default(GeometryStream);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    effect = BasicEffect.Create();

                    vertexBuffer = VertexBuffer.Create(VertexPositionColor.VertexDeclaration, 3);
                    vertexBuffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                        new VertexPositionColor(new Vector3(1, -1, 0), Color.Lime),
                        new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),
                    });

                    geometryStream = GeometryStream.Create();
                    geometryStream.Attach(vertexBuffer);
                })
                .Render(uv =>
                {
                    var gfx = uv.GetGraphics();
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var viewport = new Viewport(0, 0, window.Compositor.Width, window.Compositor.Height);
                    var aspectRatio = viewport.Width / (float)viewport.Height;

                    gfx.SetViewport(viewport);

                    effect.World = Matrix.Identity;
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
                    effect.VertexColorEnabled = true;

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        gfx.SetRasterizerState(RasterizerState.CullNone);
                        gfx.SetGeometryStream(geometryStream);
                        gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    }
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/UltravioletGraphics_CanRenderAColoredTriangle.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the Graphics subsystem can render a single textured triangle.")]
        public void UltravioletGraphics_CanRenderATexturedTriangle()
        {
            var effect = default(BasicEffect);
            var vertexBuffer = default(VertexBuffer);
            var geometryStream = default(GeometryStream);
            var texture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    effect = BasicEffect.Create();

                    vertexBuffer = VertexBuffer.Create(VertexPositionTexture.VertexDeclaration, 3);
                    vertexBuffer.SetData(new[]
                    {
                        new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1)),
                        new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
                        new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0))
                    });

                    geometryStream = GeometryStream.Create();
                    geometryStream.Attach(vertexBuffer);

                    texture = content.Load<Texture2D>(@"Textures\Triangle");
                })
                .Render(uv =>
                {
                    var gfx = uv.GetGraphics();
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    var viewport = new Viewport(0, 0, window.Compositor.Width, window.Compositor.Height);
                    var aspectRatio = viewport.Width / (float)viewport.Height;

                    gfx.SetViewport(viewport);

                    effect.World = Matrix.Identity;
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
                    effect.VertexColorEnabled = false;
                    effect.TextureEnabled = true;
                    effect.Texture = texture;

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        gfx.SetRasterizerState(RasterizerState.CullNone);
                        gfx.SetGeometryStream(geometryStream);
                        gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    }
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/UltravioletGraphics_CanRenderATexturedTriangle.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the Graphics subsystem can render several triangles using hardware instancing.")]
        public void UltravioletGraphics_CanRenderInstancedTriangles()
        {
            var effect = default(Effect);
            var vbuffer0 = default(VertexBuffer);
            var vbuffer1 = default(VertexBuffer);
            var ibuffer0 = default(IndexBuffer);
            var geomstream = default(GeometryStream);

            const Int32 InstancesX = 48;
            const Int32 InstancesY = 36;

            const Single TriangleWidth = 10f;
            const Single TriangleHeight = 10f;

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    effect = content.Load<Effect>("Effects\\InstancedRenderingTestEffect");

                    vbuffer0 = VertexBuffer.Create(VertexPosition.VertexDeclaration, 3);
                    vbuffer0.SetData(new[]
                    {
                        new VertexPosition(new Vector3(0, 0, 0)),
                        new VertexPosition(new Vector3(TriangleWidth, 0, 0)),
                        new VertexPosition(new Vector3(0, TriangleHeight, 0))
                    });

                    var instanceData = new CanRenderInstancedTrianglesData[InstancesX * InstancesY];
                    for (int y = 0; y < InstancesY; y++)
                    {
                        for (int x = 0; x < InstancesX; x++)
                        {
                            var transform =
                                Matrix.CreateTranslation(x * TriangleWidth, y * TriangleHeight, 0);

                            var color = new Color(
                                Math.Max(0, 255 - 5 * x),
                                Math.Max(0, 255 - 5 * y), 0);

                            instanceData[(y * InstancesX) + x] = new CanRenderInstancedTrianglesData(transform, color);
                        }
                    }

                    vbuffer1 = VertexBuffer.Create(CanRenderInstancedTrianglesData.VertexDeclaration, instanceData.Length);
                    vbuffer1.SetData(instanceData);

                    ibuffer0 = IndexBuffer.Create(IndexBufferElementType.Int16, 3);
                    ibuffer0.SetData(new Int16[] { 0, 1, 2 });

                    geomstream = GeometryStream.Create();
                    geomstream.Attach(vbuffer0);
                    geomstream.Attach(vbuffer1, 1);
                    geomstream.Attach(ibuffer0);
                })
                .Render(uv =>
                {
                    var gfx = uv.GetGraphics();
                    var viewport = gfx.GetViewport();

                    var matrixTransform = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
                    effect.Parameters["MatrixTransform"].SetValue(matrixTransform);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        gfx.SetRasterizerState(RasterizerState.CullNone);
                        gfx.SetGeometryStream(geomstream);
                        gfx.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 1, InstancesX * InstancesY);
                    }
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/UltravioletGraphics_CanRenderInstancedTriangles.png");
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that the Graphics subsystem correctly renders a scene when using a custom compositor.")]
        public void UltravioletGraphics_RendersFrameCorrectly_WithCustomCompositor()
        {
            var spriteBatch = default(SpriteBatch);
            var spriteTexture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithInitialization(uv =>
                {
                    var window = uv.GetPlatform().Windows.GetPrimary();
                    window.ChangeCompositor(new CustomCompositor(uv, window));
                })
                .WithContent(content =>
                {
                    spriteBatch = SpriteBatch.Create();
                    spriteTexture = Texture2D.Create(1, 1);
                    spriteTexture.SetData(new Color[] { Color.White });
                })
                .Render(uv =>
                {
                    var window = uv.GetPlatform().Windows.GetCurrent();

                    var width = window.Compositor.Width;
                    var height = window.Compositor.Height;

                    window.Compositor.BeginContext(CompositionContext.Scene);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(spriteTexture, new RectangleF(0, 0, width / 2, height), Color.White);
                    spriteBatch.End();

                    window.Compositor.BeginContext(CompositionContext.Interface);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(spriteTexture, new RectangleF(16, 16, 16, 16), null, Color.DarkGray,
                        Radians.FromDegrees(45), new Vector2(8, 8), SpriteEffects.OriginRelativeToDestination, 0f);
                    spriteBatch.End();
                    
                    window.Compositor.BeginContext(CompositionContext.Scene);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(spriteTexture, new RectangleF(width / 2, 0, width / 2, height), Color.DarkGray);
                    spriteBatch.End();

                    window.Compositor.BeginContext(CompositionContext.Interface);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(spriteTexture, new RectangleF(width - 16, height - 16, 16, 16), null, Color.White,
                        Radians.FromDegrees(45), new Vector2(8, 8), SpriteEffects.OriginRelativeToDestination, 0f);
                    spriteBatch.End();
                });

            TheResultingImage(result)
                .ShouldMatch(@"Resources/Expected/Graphics/UltravioletGraphics_RendersFrameCorrectly_WithCustomCompositor.png");
        }
    }
}