using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics
{
    [TestClass]
    [DeploymentItem(@"Resources")]
    [DeploymentItem(@"Dependencies")]
    public class UltravioletGraphicsTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Rendering")]
        [DeploymentItem(@"Expected\Graphics\UltravioletGraphics_CanRenderAColoredTriangle.png")]
        public void UltravioletGraphics_CanRenderAColoredTriangle()
        {
            var effect         = default(BasicEffect);
            var vertexBuffer   = default(VertexBuffer);
            var geometryStream = default(GeometryStream);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    effect = BasicEffect.Create();

                    vertexBuffer = VertexBuffer.Create(VertexPositionColor.VertexDeclaration, 3);
                    vertexBuffer.SetData<VertexPositionColor>(new[]
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
                    var gfx         = uv.GetGraphics();
                    var window      = uv.GetPlatform().Windows.GetPrimary();
                    var aspectRatio = window.ClientSize.Width / (float)window.ClientSize.Height;

                    effect.World              = Matrix.Identity;
                    effect.View               = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
                    effect.Projection         = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
                    effect.VertexColorEnabled = true;

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        gfx.SetRasterizerState(RasterizerState.CullNone);
                        gfx.SetGeometryStream(geometryStream);
                        gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    }
                });

            TheResultingImage(result).ShouldMatch(@"Expected\Graphics\UltravioletGraphics_CanRenderAColoredTriangle.png");
        }
        
        [TestMethod]
        [TestCategory("Rendering")]
        [DeploymentItem(@"Expected\Graphics\UltravioletGraphics_CanRenderATexturedTriangle.png")]
        public void UltravioletGraphics_CanRenderATexturedTriangle()
        {
            var effect         = default(BasicEffect);
            var vertexBuffer   = default(VertexBuffer);
            var geometryStream = default(GeometryStream);
            var texture        = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    effect = BasicEffect.Create();

                    vertexBuffer = VertexBuffer.Create(VertexPositionTexture.VertexDeclaration, 3);
                    vertexBuffer.SetData<VertexPositionTexture>(new[]
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
                    var gfx         = uv.GetGraphics();
                    var window      = uv.GetPlatform().Windows.GetPrimary();
                    var aspectRatio = window.ClientSize.Width / (float)window.ClientSize.Height;

                    effect.World              = Matrix.Identity;
                    effect.View               = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
                    effect.Projection         = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);
                    effect.VertexColorEnabled = false;
                    effect.TextureEnabled     = true;
                    effect.Texture            = texture;

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        gfx.SetRasterizerState(RasterizerState.CullNone);
                        gfx.SetGeometryStream(geometryStream);
                        gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    }
                });

            TheResultingImage(result).ShouldMatch(@"Expected\Graphics\UltravioletGraphics_CanRenderATexturedTriangle.png");
        }
    }
}
