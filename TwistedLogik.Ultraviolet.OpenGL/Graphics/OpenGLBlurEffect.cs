using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of <see cref="BlurEffect"/>.
    /// </summary>
    public sealed class OpenGLBlurEffect : BlurEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLBlurEffect"/> class.
        /// </summary>
        public OpenGLBlurEffect(UltravioletContext uv)
            : base(CreateEffectImplementation(uv))
        {
            epDirection = Parameters["Direction"];
            epResolution = Parameters["Resolution"];

            UpdateDirection();
        }

        /// <inheritdoc/>
        protected override void OnDirectionChanged()
        {
            UpdateDirection();
            base.OnDirectionChanged();
        }

        /// <inheritdoc/>
        protected override void OnTextureSizeChanged()
        {
            UpdateResolution();
            base.OnTextureSizeChanged();
        }

        /// <summary>
        /// Updates the value of the Direction effect parameter.
        /// </summary>
        private void UpdateDirection()
        {
            var directionVector = (Direction == BlurDirection.Horizontal) ? Vector2.UnitX : Vector2.UnitY;
            epDirection.SetValue(directionVector);

            UpdateResolution();
        }

        /// <summary>
        /// Updates the value of the Resolution effect parameter.
        /// </summary>
        private void UpdateResolution()
        {
            var resolution = (Direction == BlurDirection.Horizontal) ? TextureWidth : TextureHeight;
            epResolution.SetValue((Single)resolution);
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
            };

            var passes     = new[] { new OpenGLEffectPass(uv, null, programs) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        // Shaders
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShader = 
            new UltravioletSingleton<OpenGLVertexShader>((uv) => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffect.vert")); });
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader = 
            new UltravioletSingleton<OpenGLFragmentShader>((uv) => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffect.frag")); });

        // Cached effect parameters
        private readonly EffectParameter epDirection;
        private readonly EffectParameter epResolution;        
    }
}
