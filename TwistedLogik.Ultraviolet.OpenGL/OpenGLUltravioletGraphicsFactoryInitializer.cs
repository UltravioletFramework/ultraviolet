using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.OpenGL.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2.Platform;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Initializes factory methods for the OpenGL/SDL2 implementation of the graphics subsystem.
    /// </summary>
    public sealed class OpenGLUltravioletGraphicsFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The factory to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            // Core classes
            factory.SetFactoryMethod<CursorFactory>((uv, surface, hx, hv) => new OpenGLCursor(uv, surface, hx, hv));
            factory.SetFactoryMethod<GeometryStreamFactory>((uv) => new OpenGLGeometryStream(uv));
            factory.SetFactoryMethod<VertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, gl.GL_STATIC_DRAW));
            factory.SetFactoryMethod<IndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, gl.GL_STATIC_DRAW));
            factory.SetFactoryMethod<DynamicVertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, gl.GL_DYNAMIC_DRAW));
            factory.SetFactoryMethod<DynamicIndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, gl.GL_DYNAMIC_DRAW));
            factory.SetFactoryMethod<Surface2DFactory>((uv, width, height) => new OpenGLSurface2D(uv, width, height));
            factory.SetFactoryMethod<Surface2DFromSourceFactory>((uv, source) => new OpenGLSurface2D(uv, source));
            factory.SetFactoryMethod<Texture2DFactory>((uv, width, height) => new OpenGLTexture2D(uv, width, height));
            factory.SetFactoryMethod<RenderTarget2DFactory>((uv, width, height) => new OpenGLRenderTarget2D(uv, width, height));
            factory.SetFactoryMethod<RenderBuffer2DFactory>((uv, format, width, height) => new OpenGLRenderBuffer2D(uv, format, width, height));

            // Core effects
            factory.SetFactoryMethod<BasicEffectFactory>((uv) => new OpenGLBasicEffect(uv));
            factory.SetFactoryMethod<SpriteBatchEffectFactory>((uv) => new OpenGLSpriteBatchEffect(uv));

            // BlendState
            var blendStateOpaque = OpenGLBlendState.CreateOpaque(owner);
            var blendStateAlphaBlend = OpenGLBlendState.CreateAlphaBlend(owner);
            var blendStateAdditive = OpenGLBlendState.CreateAdditive(owner);
            var blendStateNonPremultiplied = OpenGLBlendState.CreateNonPremultiplied(owner);

            factory.SetFactoryMethod<BlendStateFactory>((uv) => new OpenGLBlendState(uv));
            factory.SetFactoryMethod<BlendStateFactory>("Opaque", (uv) => blendStateOpaque);
            factory.SetFactoryMethod<BlendStateFactory>("AlphaBlend", (uv) => blendStateAlphaBlend);
            factory.SetFactoryMethod<BlendStateFactory>("Additive", (uv) => blendStateAdditive);
            factory.SetFactoryMethod<BlendStateFactory>("NonPremultiplied", (uv) => blendStateNonPremultiplied);

            // DepthStencilState
            var depthStencilStateDefault = OpenGLDepthStencilState.CreateDefault(owner);
            var depthStencilStateDepthRead = OpenGLDepthStencilState.CreateDepthRead(owner);
            var depthStencilStateNone = OpenGLDepthStencilState.CreateNone(owner);

            factory.SetFactoryMethod<DepthStencilStateFactory>((uv) => new OpenGLDepthStencilState(uv));
            factory.SetFactoryMethod<DepthStencilStateFactory>("Default", (uv) => depthStencilStateDefault);
            factory.SetFactoryMethod<DepthStencilStateFactory>("DepthRead", (uv) => depthStencilStateDepthRead);
            factory.SetFactoryMethod<DepthStencilStateFactory>("None", (uv) => depthStencilStateNone);

            // RasterizerState
            var rasterizerStateCullClockwise = OpenGLRasterizerState.CreateCullClockwise(owner);
            var rasterizerStateCullCounterClockwise = OpenGLRasterizerState.CreateCullCounterClockwise(owner);
            var rasterizerStateCullNone = OpenGLRasterizerState.CreateCullNone(owner);

            factory.SetFactoryMethod<RasterizerStateFactory>((uv) => new OpenGLRasterizerState(uv));
            factory.SetFactoryMethod<RasterizerStateFactory>("CullClockwise", (uv) => rasterizerStateCullClockwise);
            factory.SetFactoryMethod<RasterizerStateFactory>("CullCounterClockwise", (uv) => rasterizerStateCullCounterClockwise);
            factory.SetFactoryMethod<RasterizerStateFactory>("CullNone", (uv) => rasterizerStateCullNone);

            // SamplerState
            var samplerStatePointClamp = OpenGLSamplerState.CreatePointClamp(owner);
            var samplerStatePointWrap = OpenGLSamplerState.CreatePointWrap(owner);
            var samplerStateLinearClamp = OpenGLSamplerState.CreateLinearClamp(owner);
            var samplerStateLinearWrap = OpenGLSamplerState.CreateLinearWrap(owner);
            var samplerStateAnisotropicClamp = OpenGLSamplerState.CreateAnisotropicClamp(owner);
            var samplerStateAnisotropicWrap = OpenGLSamplerState.CreateAnisotropicWrap(owner);

            factory.SetFactoryMethod<SamplerStateFactory>((uv) => new OpenGLSamplerState(uv));
            factory.SetFactoryMethod<SamplerStateFactory>("PointClamp", (uv) => samplerStatePointClamp);
            factory.SetFactoryMethod<SamplerStateFactory>("PointWrap", (uv) => samplerStatePointWrap);
            factory.SetFactoryMethod<SamplerStateFactory>("LinearClamp", (uv) => samplerStateLinearClamp);
            factory.SetFactoryMethod<SamplerStateFactory>("LinearWrap", (uv) => samplerStateLinearWrap);
            factory.SetFactoryMethod<SamplerStateFactory>("AnisotropicClamp", (uv) => samplerStateAnisotropicClamp);
            factory.SetFactoryMethod<SamplerStateFactory>("AnisotropicWrap", (uv) => samplerStateAnisotropicWrap);

            // Platform services
            var powerManagementService = new SDL2PowerManagementService();
            factory.SetFactoryMethod<PowerManagementServiceFactory>(() => powerManagementService);
        }
    }
}
