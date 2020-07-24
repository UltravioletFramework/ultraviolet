using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="Ultraviolet.Graphics.BasicEffect"/> class.
    /// </summary>
    public sealed class OpenGLBasicEffect : BasicEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLBasicEffect"/> class.
        /// </summary>
        public OpenGLBasicEffect(UltravioletContext uv)
            : base(CreateEffectImplementation(uv))
        {

        }

        /// <inheritdoc/>
        protected override void OnApply()
        {
            var index = 0;

            if (TextureEnabled)
            {
                index += 4;
            }

            if (VertexColorEnabled)
            {
                index += 2;
            }

            if (FogEnabled)
            {
                index += 1;
            }

            ((OpenGLEffectPass)CurrentTechnique.Passes[0]).ProgramIndex = index;

            base.OnApply();
        }

        /// <summary>
        /// Creates the effect implementation.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The effect implementation.</returns>
        private static EffectImplementation CreateEffectImplementation(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var programs = new[]
            {
                new OpenGLShaderProgram(uv, vertShader, fragShader, false),
                new OpenGLShaderProgram(uv, vertShaderFog, fragShader, false),
                new OpenGLShaderProgram(uv, vertShaderColored, fragShader, false),
                new OpenGLShaderProgram(uv, vertShaderColoredFog, fragShader, false),
                new OpenGLShaderProgram(uv, vertShaderTextured, fragShaderTextured, false),
                new OpenGLShaderProgram(uv, vertShaderTexturedFog, fragShaderTextured, false),
                new OpenGLShaderProgram(uv, vertShaderColoredTextured, fragShaderTextured, false),
                new OpenGLShaderProgram(uv, vertShaderColoredTexturedFog, fragShaderTextured, false),
            };

            var passes = new[] { new OpenGLEffectPass(uv, null, programs) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        // Shaders - basic
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShader = 
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffect.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader = 
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffect.frag")); });

        // Shaders - basic, fog
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderFog =
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectFog.vert")); });
        
        // Shaders - colored
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColored = 
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColored.vert")); });

        // Shaders - colored, fog
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColoredFog =
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColoredFog.vert")); });

        // Shaders - textured
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderTextured = 
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectTextured.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShaderTextured = 
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectTextured.frag")); });

        // Shaders - textured, fog
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderTexturedFog =
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectTexturedFog.vert")); });

        // Shaders - colored, textured
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColoredTextured = 
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode, 
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColoredTextured.vert")); });

        // Shaders - colored, textured, fog
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColoredTexturedFog =
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColoredTexturedFog.vert")); });
    }
}
