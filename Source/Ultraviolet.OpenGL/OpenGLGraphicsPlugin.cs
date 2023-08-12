using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics.Graphics2D;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents an Ultraviolet plugin which registers OpenGL as the graphics subsystem implementation.
    /// </summary>
    public class OpenGLGraphicsPlugin : UltravioletPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLGraphicsPlugin"/> class.
        /// </summary>
        /// <param name="configuration">The graphics configuration settings.</param>
        public OpenGLGraphicsPlugin(OpenGLGraphicsConfiguration configuration = null)
        {
            this.configuration = configuration ?? OpenGLGraphicsConfiguration.Default;
        }

        /// <inheritdoc/>
        public override void Configure(UltravioletContext uv, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UltravioletGraphicsFactory>((uv, configuration) => new OpenGLUltravioletGraphics(uv, configuration));

            {
                // Core classes
                factory.SetFactoryMethod<GeometryStreamFactory>((uv) => new OpenGLGeometryStream(uv));
                factory.SetFactoryMethod<VertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, GL.GL_STATIC_DRAW));
                factory.SetFactoryMethod<IndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, GL.GL_STATIC_DRAW));
                factory.SetFactoryMethod<DynamicVertexBufferFactory>((uv, vdecl, vcount) => new OpenGLVertexBuffer(uv, vdecl, vcount, GL.GL_DYNAMIC_DRAW));
                factory.SetFactoryMethod<DynamicIndexBufferFactory>((uv, itype, icount) => new OpenGLIndexBuffer(uv, itype, icount, GL.GL_DYNAMIC_DRAW));
                factory.SetFactoryMethod<Texture2DFromRawDataFactory>((uv, pixels, width, height, format, srgb) => new OpenGLTexture2D(uv, pixels, width, height, format, srgb));
                factory.SetFactoryMethod<Texture2DFactory>((uv, width, height, options) => new OpenGLTexture2D(uv, width, height, options));
                factory.SetFactoryMethod<Texture3DFromRawDataFactory>((uv, layers, width, height, format, srgb) => new OpenGLTexture3D(uv, layers, width, height, format, srgb));
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
                var blendStateOpaque = OpenGLBlendState.CreateOpaque(uv);
                var blendStateAlphaBlend = OpenGLBlendState.CreateAlphaBlend(uv);
                var blendStateAdditive = OpenGLBlendState.CreateAdditive(uv);
                var blendStateNonPremultiplied = OpenGLBlendState.CreateNonPremultiplied(uv);

                factory.SetFactoryMethod<BlendStateFactory>((uv) => new OpenGLBlendState(uv));
                factory.SetFactoryMethod<BlendStateFactory>("Opaque", (uv) => blendStateOpaque);
                factory.SetFactoryMethod<BlendStateFactory>("AlphaBlend", (uv) => blendStateAlphaBlend);
                factory.SetFactoryMethod<BlendStateFactory>("Additive", (uv) => blendStateAdditive);
                factory.SetFactoryMethod<BlendStateFactory>("NonPremultiplied", (uv) => blendStateNonPremultiplied);

                // DepthStencilState
                var depthStencilStateDefault = OpenGLDepthStencilState.CreateDefault(uv);
                var depthStencilStateDepthRead = OpenGLDepthStencilState.CreateDepthRead(uv);
                var depthStencilStateNone = OpenGLDepthStencilState.CreateNone(uv);

                factory.SetFactoryMethod<DepthStencilStateFactory>((uv) => new OpenGLDepthStencilState(uv));
                factory.SetFactoryMethod<DepthStencilStateFactory>("Default", (uv) => depthStencilStateDefault);
                factory.SetFactoryMethod<DepthStencilStateFactory>("DepthRead", (uv) => depthStencilStateDepthRead);
                factory.SetFactoryMethod<DepthStencilStateFactory>("None", (uv) => depthStencilStateNone);

                // RasterizerState
                var rasterizerStateCullClockwise = OpenGLRasterizerState.CreateCullClockwise(uv);
                var rasterizerStateCullCounterClockwise = OpenGLRasterizerState.CreateCullCounterClockwise(uv);
                var rasterizerStateCullNone = OpenGLRasterizerState.CreateCullNone(uv);

                factory.SetFactoryMethod<RasterizerStateFactory>((uv) => new OpenGLRasterizerState(uv));
                factory.SetFactoryMethod<RasterizerStateFactory>("CullClockwise", (uv) => rasterizerStateCullClockwise);
                factory.SetFactoryMethod<RasterizerStateFactory>("CullCounterClockwise", (uv) => rasterizerStateCullCounterClockwise);
                factory.SetFactoryMethod<RasterizerStateFactory>("CullNone", (uv) => rasterizerStateCullNone);

                // SamplerState
                var samplerStatePointClamp = OpenGLSamplerState.CreatePointClamp(uv);
                var samplerStatePointWrap = OpenGLSamplerState.CreatePointWrap(uv);
                var samplerStateLinearClamp = OpenGLSamplerState.CreateLinearClamp(uv);
                var samplerStateLinearWrap = OpenGLSamplerState.CreateLinearWrap(uv);
                var samplerStateAnisotropicClamp = OpenGLSamplerState.CreateAnisotropicClamp(uv);
                var samplerStateAnisotropicWrap = OpenGLSamplerState.CreateAnisotropicWrap(uv);

                factory.SetFactoryMethod<SamplerStateFactory>((uv) => new OpenGLSamplerState(uv));
                factory.SetFactoryMethod<SamplerStateFactory>("PointClamp", (uv) => samplerStatePointClamp);
                factory.SetFactoryMethod<SamplerStateFactory>("PointWrap", (uv) => samplerStatePointWrap);
                factory.SetFactoryMethod<SamplerStateFactory>("LinearClamp", (uv) => samplerStateLinearClamp);
                factory.SetFactoryMethod<SamplerStateFactory>("LinearWrap", (uv) => samplerStateLinearWrap);
                factory.SetFactoryMethod<SamplerStateFactory>("AnisotropicClamp", (uv) => samplerStateAnisotropicClamp);
                factory.SetFactoryMethod<SamplerStateFactory>("AnisotropicWrap", (uv) => samplerStateAnisotropicWrap);
            }

            base.Configure(uv, factory);
        }

        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            var content = uv.GetContent();
            {
                content.Importers.RegisterImporter<OpenGLFragmentShaderImporter>(OpenGLFragmentShaderImporter.SupportedExtensions);

                content.Importers.RegisterImporter<OpenGLVertexShaderImporter>(OpenGLVertexShaderImporter.SupportedExtensions);

                content.Processors.RegisterProcessor<OpenGLEffectImplementationProcessorFromJObject>();
                content.Processors.RegisterProcessor<OpenGLEffectImplementationProcessorFromShaderSource>();
                content.Processors.RegisterProcessor<OpenGLEffectImplementationProcessorFromXDocument>();
                content.Processors.RegisterProcessor<OpenGLEffectProcessorFromJObject>();
                content.Processors.RegisterProcessor<OpenGLEffectProcessorFromShaderSource>();
                content.Processors.RegisterProcessor<OpenGLEffectProcessorFromXDocument>();
                content.Processors.RegisterProcessor<OpenGLEffectSourceProcessorFromJObject>();
                content.Processors.RegisterProcessor<OpenGLEffectSourceProcessorFromShaderSource>();
                content.Processors.RegisterProcessor<OpenGLEffectSourceProcessorFromXDocument>();
                content.Processors.RegisterProcessor<OpenGLFragmentShaderProcessor>();
                content.Processors.RegisterProcessor<OpenGLVertexShaderProcessor>();
                content.Processors.RegisterProcessor<ShaderSourceProcessor>();
            }
            base.Initialize(uv, factory);
        }

        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            configuration.GraphicsConfiguration = this.configuration;

            base.Register(configuration);
        }

        // Graphics configuration settings.
        private readonly OpenGLGraphicsConfiguration configuration;
    }
}
