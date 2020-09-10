using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a <see cref="Material"/> which uses <see cref="BasicEffect"/> to shade meshes.
    /// </summary>
    public class BasicMaterial : Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicMaterial"/> class.
        /// </summary>
        /// <param name="effectInstance">The <see cref="BasicEffect"/> instance which this material uses when
        /// drawing meshes, or <see langword="null"/> to use a shared effect instance.</param>
        public BasicMaterial(BasicEffect effectInstance = null)
            : base(effectInstance ?? SharedEffect)
        {

        }

        /// <summary>
        /// Gets the shared <see cref="BasicEffect"/> instance which <see cref="BasicMaterial"/> instances use by default when drawing meshes.
        /// </summary>
        public static BasicEffect SharedEffect => sharedEffectSingleton.Value;

        /// <inheritdoc/>
        public override void Apply()
        {
            Effect.Alpha = Alpha;
            Effect.DiffuseColor = DiffuseColor;
            Effect.EmissiveColor = EmissiveColor;
            Effect.SpecularColor = SpecularColor;
            Effect.SpecularPower = SpecularPower;
            Effect.Texture = Texture;
        }

        /// <summary>
        /// Gets the <see cref="BasicEffect"/> instance which this material uses when drawing meshes.
        /// </summary>
        public new BasicEffect Effect => (BasicEffect)base.Effect;

        /// <summary>
        /// Gets or sets the material's alpha value.
        /// </summary>
        public Single Alpha { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the material's diffuse color.
        /// </summary>
        public Color DiffuseColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the material's emissive color.
        /// </summary>
        public Color EmissiveColor { get; set; }

        /// <summary>
        /// Gets or sets the material's specular color.
        /// </summary>
        public Color SpecularColor { get; set; }

        /// <summary>
        /// Gets or sets the material's specular power.
        /// </summary>
        public Single SpecularPower { get; set; } = 16f;
        
        /// <summary>
        /// Gets or sets the material's texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        // Shared effect.
        private static readonly UltravioletSingleton<BasicEffect> sharedEffectSingleton =
            new UltravioletSingleton<BasicEffect>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => BasicEffect.Create());
    }
}
