using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a <see cref="Material"/> which uses <see cref="SkinnedEffect"/> to shade meshes.
    /// </summary>
    public class SkinnedMaterial : Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedMaterial"/> class.
        /// </summary>
        /// <param name="effectInstance">The <see cref="SkinnedEffect"/> instance which this material uses when
        /// drawing meshes, or <see langword="null"/> to use a shared effect instance.</param>
        public SkinnedMaterial(SkinnedEffect effectInstance = null)
            : base(effectInstance ?? SharedEffect)
        {

        }

        /// <summary>
        /// Gets the shared <see cref="SkinnedEffect"/> instance which <see cref="SkinnedMaterial"/> instances use by default when drawing meshes.
        /// </summary>
        public static SkinnedEffect SharedEffect => sharedEffectSingleton.Value;

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
        /// Gets the <see cref="SkinnedEffect"/> instance which this material uses when drawing meshes.
        /// </summary>
        public new SkinnedEffect Effect => (SkinnedEffect)base.Effect;

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
        private static readonly UltravioletSingleton<SkinnedEffect> sharedEffectSingleton =
            new UltravioletSingleton<SkinnedEffect>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => SkinnedEffect.Create());
    }
}
