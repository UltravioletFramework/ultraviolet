using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics;
using Ultraviolet.OpenGL.Graphics.Graphics2D;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Initializes factory methods for the OpenGL implementation of the graphics subsystem.
    /// </summary>
    public sealed class OpenGLUltravioletGraphicsFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            // Core classes
            factory.SetFactoryMethod<GeometryStreamFactory>((uv) => new OpenGLGeometryStream(uv));
            factory.SetFactoryMethod<VertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, gl.GL_STATIC_DRAW));
            factory.SetFactoryMethod<IndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, gl.GL_STATIC_DRAW));
            factory.SetFactoryMethod<DynamicVertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, gl.GL_DYNAMIC_DRAW));
            factory.SetFactoryMethod<DynamicIndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, gl.GL_DYNAMIC_DRAW));
            factory.SetFactoryMethod<Texture2DFromRawDataFactory>((uv, pixels, width, height, bytesPerPixel, srgb) => new OpenGLTexture2D(uv, pixels, width, height, bytesPerPixel, srgb));
            factory.SetFactoryMethod<Texture2DFactory>((uv, width, height, options) => new OpenGLTexture2D(uv, width, height, options));
            factory.SetFactoryMethod<Texture3DFromRawDataFactory>((uv, layers, width, height, bytesPerPixel, srgb) => new OpenGLTexture3D(uv, layers, width, height, bytesPerPixel, srgb));
            factory.SetFactoryMethod<Texture3DFactory>((uv, width, height, depth, options) => new OpenGLTexture3D(uv, width, height, depth, options));
            factory.SetFactoryMethod<RenderTarget2DFactory>((uv, width, height, usage) => new OpenGLRenderTarget2D(uv, width, height, usage));
            factory.SetFactoryMethod<RenderBuffer2DFactory>((uv, format, width, height, options) => new OpenGLRenderBuffer2D(uv, format, width, height, options));
            factory.SetFactoryMethod<DynamicTexture2DFactory>((uv, width, height, options, state, flushed) => new OpenGLDynamicTexture2D(uv, width, height, options, state, flushed));
            factory.SetFactoryMethod<DynamicTexture3DFactory>((uv, width, height, depth, options, state, flushed) => new OpenGLDynamicTexture3D(uv, width, height, depth, options, state, flushed));
            factory.SetFactoryMethod<SwapChainManagerFactory>((uv) => new OpenGLSwapChainManager(uv));

            // Core effects
            factory.SetFactoryMethod<BasicEffectFactory>((uv) => new OpenGLBasicEffect(uv));
            factory.SetFactoryMethod<SkinnedEffectFactory>((uv) => new OpenGLSkinnedEffect(uv));
            factory.SetFactoryMethod<SpriteBatchEffectFactory>((uv) => new OpenGLSpriteBatchEffect(uv));
            factory.SetFactoryMethod<BlurEffectFactory>((uv) => new OpenGLBlurEffect(uv));

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
        }
    }
}
