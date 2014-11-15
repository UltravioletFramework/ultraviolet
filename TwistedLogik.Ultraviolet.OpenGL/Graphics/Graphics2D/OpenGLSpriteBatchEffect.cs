using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the sprite batch custom effect.
    /// </summary>
    public sealed class OpenGLSpriteBatchEffect : SpriteBatchEffect
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLSpriteBatchEffect class.
        /// </summary>
        public OpenGLSpriteBatchEffect(UltravioletContext uv)
            : base(CreateEffectImplementation(uv))
        {

        }

        /// <summary>
        /// Creates the effect implementation.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The effect implementation.</returns>
        private static EffectImplementation CreateEffectImplementation(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            var programs   = new[] { new OpenGLShaderProgram(uv, vertShader, fragShader, false) };
            var passes     = new[] { new OpenGLEffectPass(uv, null, programs) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        // The shaders that make up this effect.
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShader = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { 
                return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SpriteBatchEffect.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => {
                return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("SpriteBatchEffect.frag")); });
    }
}
