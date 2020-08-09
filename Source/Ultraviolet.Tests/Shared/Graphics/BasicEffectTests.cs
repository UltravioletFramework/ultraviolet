using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ultraviolet.Graphics;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public partial class BasicEffectTests : UltravioletApplicationTestFramework
    {
        private static IEnumerable<TestCaseData> BasicEffectTestCases
        {
            get
            {
                yield return new TestCaseData(new BasicEffectTestParameters());
                yield return new TestCaseData(new BasicEffectTestParameters() { TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { TextureEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { LightingEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { LightingEnabled = true, TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { LightingEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { LightingEnabled = true, TextureEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { PreferPerPixelLighting = true, LightingEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { PreferPerPixelLighting = true, LightingEnabled = true, TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { PreferPerPixelLighting = true, LightingEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { PreferPerPixelLighting = true, LightingEnabled = true, TextureEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, TextureEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, LightingEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, LightingEnabled = true, TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, LightingEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, LightingEnabled = true, TextureEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, PreferPerPixelLighting = true, LightingEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, PreferPerPixelLighting = true, LightingEnabled = true, TextureEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, PreferPerPixelLighting = true, LightingEnabled = true, VertexColorEnabled = true });
                yield return new TestCaseData(new BasicEffectTestParameters() { FogEnabled = true, PreferPerPixelLighting = true, LightingEnabled = true, TextureEnabled = true, VertexColorEnabled = true });
            }
        }

        [Test, TestCaseSource(nameof(BasicEffectTestCases))]
        [Category("Rendering")]
        [Description("Ensures that #include directives in GLSL shader source are correctly processed.")]
        public void BasicEffect_RendersACubeCorrectly(BasicEffectTestParameters parameters)
        {
            void DrawGeometry(IUltravioletGraphics gfx, Effect eff, RasterizerState rasterizerState, DepthStencilState depthStencilState, Int32 count)
            {
                foreach (var pass in eff.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    gfx.SetRasterizerState(rasterizerState);
                    gfx.SetDepthStencilState(depthStencilState);
                    gfx.DrawPrimitives(PrimitiveType.TriangleList, 0, count);
                }
            }

            var effect = default(BasicEffect);
            var vbuffer = default(VertexBuffer);
            var gstream = default(GeometryStream);

            var rasterizerStateSolid = default(RasterizerState);
            var rasterizerStateWireframe = default(RasterizerState);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    rasterizerStateSolid = RasterizerState.Create();
                    rasterizerStateSolid.CullMode = CullMode.CullCounterClockwiseFace;
                    rasterizerStateSolid.FillMode = FillMode.Solid;

                    rasterizerStateWireframe = RasterizerState.Create();
                    rasterizerStateWireframe.CullMode = CullMode.CullCounterClockwiseFace;
                    rasterizerStateWireframe.FillMode = FillMode.Wireframe;

                    effect = BasicEffect.Create();
                    effect.FogEnabled = parameters.FogEnabled;
                    effect.TextureEnabled = parameters.TextureEnabled;
                    effect.Texture = content.Load<Texture2D>("Textures\\Cube");
                    effect.VertexColorEnabled = parameters.VertexColorEnabled;
                    effect.LightingEnabled = parameters.LightingEnabled;
                    effect.PreferPerPixelLighting = parameters.PreferPerPixelLighting;

                    if (effect.FogEnabled)
                    {
                        effect.FogColor = Color.Black;
                        effect.FogStart = 5f;
                        effect.FogEnd = 5.5f;
                    }

                    if (effect.LightingEnabled)
                        effect.EnableStandardLighting();

                    vbuffer = VertexBuffer.Create<BasicEffectTestVertex>(36);
                    vbuffer.SetData(new BasicEffectTestVertex[]
                    {
                        // Bottom face
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 0.75f), Color = Color.Red },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 0.75f), Color = Color.Red },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 1.00f), Color = Color.Red },

                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 1.00f), Color = Color.Red },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 1.00f), Color = Color.Red },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, -1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 0.75f), Color = Color.Red },

                        // Forward face
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.00f, 0.50f), Color = Color.Cyan },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.25f, 0.50f), Color = Color.Cyan },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.25f, 0.75f), Color = Color.Cyan },

                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.25f, 0.75f), Color = Color.Cyan },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.00f, 0.75f), Color = Color.Cyan },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 0f, -1f),
                            TextureCoordinate = new Vector2(0.00f, 0.50f), Color = Color.Cyan },

                        // Right face
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.25f, 0.50f), Color = Color.Magenta },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.50f, 0.50f), Color = Color.Magenta },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.50f, 0.75f), Color = Color.Magenta },

                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f, -1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.50f, 0.75f), Color = Color.Magenta },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.25f, 0.75f), Color = Color.Magenta },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.25f, 0.50f), Color = Color.Magenta },

                        // Backward face
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.50f, 0.50f), Color = Color.Yellow },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.75f, 0.50f), Color = Color.Yellow },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.75f, 0.75f), Color = Color.Yellow },

                        new BasicEffectTestVertex { Position = new Vector3( 1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.75f, 0.75f), Color = Color.Yellow },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.50f, 0.75f), Color = Color.Yellow },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 0f, 1f),
                            TextureCoordinate = new Vector2(0.50f, 0.50f), Color = Color.Yellow },

                        // Left face
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.75f, 0.50f), Color = Color.Blue },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(1.00f, 0.50f), Color = Color.Blue },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(1.00f, 0.75f), Color = Color.Blue },

                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f,  1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(1.00f, 0.75f), Color = Color.Blue },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 0f, -1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.75f, 0.75f), Color = Color.Blue },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(-1f, 0f, 0f),
                            TextureCoordinate = new Vector2(0.75f, 0.50f), Color = Color.Blue },

                        // Top face
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 0.25f), Color = Color.Lime },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f, -1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 0.25f), Color = Color.Lime },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 0.50f), Color = Color.Lime },

                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f, -1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.50f, 0.50f), Color = Color.Lime },
                        new BasicEffectTestVertex { Position = new Vector3( 1f, 2f,  1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 0.50f), Color = Color.Lime },
                        new BasicEffectTestVertex { Position = new Vector3(-1f, 2f,  1f), Normal = new Vector3(0, 1f, 0),
                            TextureCoordinate = new Vector2(0.25f, 0.25f), Color = Color.Lime },
                    });

                    gstream = GeometryStream.Create();
                    gstream.Attach(vbuffer);
                })
                .Render(uv =>
                {
                    var gfx = uv.GetGraphics();
                    var window = uv.GetPlatform().Windows.GetCurrent();
                    var aspectRatio = window.DrawableSize.Width / (Single)window.DrawableSize.Height;

                    effect.World = Matrix.CreateRotationY((float)(2.0 * Math.PI * 0.45f));
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 3, 6), new Vector3(0, 1f, 0), Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000f);

                    gfx.SetGeometryStream(gstream);
                    DrawGeometry(gfx, effect, rasterizerStateSolid, DepthStencilState.Default, vbuffer.VertexCount / 3);

                    effect.FogEnabled = false;
                    effect.TextureEnabled = false;
                    effect.VertexColorEnabled = false;
                    effect.LightingEnabled = false;
                    effect.PreferPerPixelLighting = false;
                    effect.DiffuseColor = Color.Black;
                    DrawGeometry(gfx, effect, rasterizerStateWireframe, DepthStencilState.None, vbuffer.VertexCount / 3);
                });

            TheResultingImage(result).WithinAbsoluteThreshold(32)
                .ShouldMatch($@"Resources/Expected/Graphics/BasicEffect_RendersACubeCorrectly({parameters}).png");
        }
    }
}
