using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the OpenGL implementation of <see cref="BlurEffect"/>.
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
        protected override void OnRadiusChanged()
        {
            UpdateRadius();
            base.OnRadiusChanged();
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
        /// Gets a value indicating whether the arbitary-radius blur shader should be loaded.
        /// </summary>
        private static Boolean IsArbitaryRadiusBlurAvailable => !GL.IsGLES2;

        /// <summary>
        /// Updates the value of the Radius effect parameter.
        /// </summary>
        private void UpdateRadius()
        {
            var programIndex = UnrolledFragmentShaderCount;

            var nearestUnrolledRadius = (Int32)Radius;

            if (nearestUnrolledRadius > 9)
                nearestUnrolledRadius = 9;

            if (nearestUnrolledRadius % 2 == 0)
                nearestUnrolledRadius--;

            if (nearestUnrolledRadius < 1)
                nearestUnrolledRadius = 1;

            var useUnrolledShader = GL.IsGLES2 || (Int32)Radius == nearestUnrolledRadius;
            if (useUnrolledShader)
            {
                programIndex = (nearestUnrolledRadius - 1) / 2;
            }
            else
            {
                Parameters["Radius"].SetValue(Radius);
            }

            ((OpenGLEffectPass)CurrentTechnique.Passes[0]).ProgramIndex = programIndex;
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
            var resolution = (Direction == BlurDirection.Horizontal) ? TextureSize.Width : TextureSize.Height;
            epResolution.SetValue((Single)resolution);
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
                new OpenGLShaderProgram(uv, vertShader, fragShader_Radius1, false),
                new OpenGLShaderProgram(uv, vertShader, fragShader_Radius3, false),
                new OpenGLShaderProgram(uv, vertShader, fragShader_Radius5, false),
                new OpenGLShaderProgram(uv, vertShader, fragShader_Radius7, false),
                new OpenGLShaderProgram(uv, vertShader, fragShader_Radius9, false),
                IsArbitaryRadiusBlurAvailable ? new OpenGLShaderProgram(uv, vertShader, fragShader, false) : null,
            };

            var passes     = new[] { new OpenGLEffectPass(uv, null, programs.Where(x => x != null).ToArray()) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        // Unrolled fragment shaders
        const Int32 UnrolledFragmentShaderCount = 5;

        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader_Radius1 =
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy, 
                uv => new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffectRadius1.frag")));
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader_Radius3 =
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy, 
                uv => new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffectRadius3.frag")));
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader_Radius5 =
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffectRadius5.frag")));
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader_Radius7 =
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffectRadius7.frag")));
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader_Radius9 =
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffectRadius9.frag")));

        // Shaders
        private static readonly UltravioletSingleton<OpenGLVertexShader> vertShader = 
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SpriteBatchEffect.vert")));
        private static readonly UltravioletSingleton<OpenGLFragmentShader> fragShader = 
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => IsArbitaryRadiusBlurAvailable ? new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("BlurEffect.frag")) : null);

        // Cached effect parameters
        private readonly EffectParameter epDirection;
        private readonly EffectParameter epResolution;        
    }
}
