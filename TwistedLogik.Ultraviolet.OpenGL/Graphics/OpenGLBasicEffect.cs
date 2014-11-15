using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of <see cref="TwistedLogik.Ultraviolet.Graphics.BasicEffect"/>.
    /// </summary>
    public sealed class OpenGLBasicEffect : BasicEffect
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLSpriteBatchEffect class.
        /// </summary>
        public OpenGLBasicEffect(UltravioletContext uv)
            : base(CreateEffectImplementation(uv))
        {

        }

        /// <summary>
        /// Occurs when the value of the VertexColorEnabled property changes.
        /// </summary>
        protected override void OnVertexColorEnabledChanged()
        {
            UpdateProgramIndex();
        }

        /// <summary>
        /// Occurs when the value of the TextureEnabled property changes.
        /// </summary>
        protected override void OnTextureEnabledChanged()
        {
            UpdateProgramIndex();
        }

        /// <summary>
        /// Creates the effect implementation.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The effect implementation.</returns>
        private static EffectImplementation CreateEffectImplementation(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            var programs = new[] 
            { 
                new OpenGLShaderProgram(uv, vertShader, fragShader, false),
                new OpenGLShaderProgram(uv, vertShaderColored, fragShaderColored, false),
                new OpenGLShaderProgram(uv, vertShaderTextured, fragShaderTextured, false),
                new OpenGLShaderProgram(uv, vertShaderColoredTextured, fragShaderColoredTextured, false),
            };

            var passes     = new[] { new OpenGLEffectPass(uv, null, programs) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        /// <summary>
        /// Changes the effect's program index based on its current settings.
        /// </summary>
        private void UpdateProgramIndex()
        {
            var index = 0;

            if (TextureEnabled)
            {
                index += 2;
            }

            if (VertexColorEnabled)
            {
                index += 1;
            }

            ((OpenGLEffectPass)CurrentTechnique.Passes[0]).ProgramIndex = index;
        }

        // Shaders - basic
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShader = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffect.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffect.frag")); });

        // Shaders - colored
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColored = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColored.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShaderColored = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColored.frag")); });

        // Shaders - textured
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderTextured = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectTextured.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShaderTextured = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectTextured.frag")); });

        // Shaders - colored & textured
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShaderColoredTextured = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColoredTextured.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShaderColoredTextured = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BasicEffectColoredTextured.frag")); });
    }
}
