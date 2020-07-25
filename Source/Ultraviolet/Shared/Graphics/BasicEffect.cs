using System;
using System.Drawing;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="BasicEffect"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="BasicEffect"/> that was created.</returns>
    public delegate BasicEffect BasicEffectFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a basic rendering effect.
    /// </summary>
    public abstract partial class BasicEffect : Effect, IEffectMatrices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements this effect.</param>
        protected BasicEffect(EffectImplementation impl)
            : base(impl)
        {
            this.epAlpha = Parameters["Alpha"];
            this.epAmbientLightColor = Parameters["AmbientLightColor"];
            this.epDiffuseColor = Parameters["DiffuseColor"];
            this.epEmissiveColor = Parameters["EmissiveColor"];
            this.epFogColor = Parameters["FogColor"];
            this.epFogStart = Parameters["FogStart"];
            this.epFogEnd = Parameters["FogEnd"];
            this.epWorld = Parameters["World"];
            this.epView = Parameters["View"];
            this.epProjection = Parameters["Projection"];
            this.epWorldViewProj = Parameters["WorldViewProj"];
            this.epEyePosition = Parameters["EyePosition"];
            this.epVertexColorEnabled = Parameters["VertexColorEnabled"];
            this.epTextureEnabled = Parameters["TextureEnabled"];
            this.epFogEnabled = Parameters["FogEnabled"];
            this.epSrgbColor = Parameters["SrgbColor"];
            this.epTexture = Parameters["Texture"];

            this.epDiffuseColor.SetValue(Color.White);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BasicEffect"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="BasicEffect"/> that was created.</returns>
        public static BasicEffect Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<BasicEffectFactory>()(uv);
        }

        /// <summary>
        /// Gets or sets the material alpha, which determines its transparency. Range is between 1.0f (fully opaque) and 0.0f (fully transparent).
        /// </summary>
        public Single Alpha
        {
            get => alpha;
            set
            {
                alpha = value;
                dirtyFlags |= DirtyFlags.Alpha;
            }
        }

        /// <summary>
        /// Gets or sets the material's ambient light color.
        /// </summary>
        public Color AmbientLightColor
        {
            get => AmbientLightColor;
            set
            {
                ambientLightColor = value;
                dirtyFlags |= DirtyFlags.AmbientLightColor;
            }
        }

        /// <summary>
        /// Gets or sets the material's diffuse color.
        /// </summary>
        public Color DiffuseColor
        {
            get => diffuseColor;
            set
            {
                diffuseColor = value;
                dirtyFlags |= DirtyFlags.DiffuseColor;
            }
        }

        /// <summary>
        /// Gets or sets the material's emissive color.
        /// </summary>
        public Color EmissiveColor
        {
            get => emissiveColor;
            set
            {
                emissiveColor = value;
                dirtyFlags |= DirtyFlags.EmissiveColor;
            }
        }

        /// <summary>
        /// Gets or sets the material's fog color.
        /// </summary>
        public Color FogColor
        {
            get => fogColor;
            set
            {
                fogColor = value;
                dirtyFlags |= DirtyFlags.FogColor;
            }
        }

        /// <summary>
        /// Gets or sets the minimum z-value for fog.
        /// </summary>
        public Single FogStart
        {
            get => fogStart;
            set
            {
                fogStart = value;
                dirtyFlags |= DirtyFlags.FogStart;
            }
        }

        /// <summary>
        /// Gets or sets the maximum z-value for fog.
        /// </summary>
        public Single FogEnd
        {
            get => fogEnd;
            set
            {
                fogEnd = value;
                dirtyFlags |= DirtyFlags.FogEnd;
            }
        }

        /// <summary>
        /// Gets or sets the effect's world matrix.
        /// </summary>
        public Matrix World
        {
            get => world;
            set
            {
                world = value;
                dirtyFlags |= DirtyFlags.World | DirtyFlags.WorldViewProj;
            }
        }

        /// <summary>
        /// Gets or sets the effect's view matrix.
        /// </summary>
        public Matrix View
        {
            get => view;
            set
            {
                view = value;
                dirtyFlags |= DirtyFlags.View | DirtyFlags.WorldViewProj;
            }
        }

        /// <summary>
        /// Gets or sets the effect's projection matrix.
        /// </summary>
        public Matrix Projection
        {
            get => projection;
            set
            {
                projection = value;
                dirtyFlags |= DirtyFlags.Projection | DirtyFlags.WorldViewProj;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertex colors are enabled for this effect.
        /// </summary>
        public Boolean VertexColorEnabled
        {
            get => vertexColorEnabled;
            set
            {
                vertexColorEnabled = value;
                dirtyFlags |= DirtyFlags.VertexColorEnabled;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether textures are enabled for this effect.
        /// </summary>
        public Boolean TextureEnabled
        {
            get => textureEnabled;
            set
            {
                textureEnabled = value;
                dirtyFlags |= DirtyFlags.TextureEnabled;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether fog is enabled for this effect.
        /// </summary>
        public Boolean FogEnabled
        {
            get => fogEnabled;
            set
            {
                fogEnabled = value;
                dirtyFlags |= DirtyFlags.FogEnabled;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the colors used by this effect should be
        /// converted from the sRGB color space to the linear color space in the vertex shader.
        /// </summary>
        public Boolean SrgbColor
        {
            get => srgbColor;
            set
            {
                srgbColor = value;
                dirtyFlags |= DirtyFlags.SrgbColor;
            }
        }

        /// <summary>
        /// Gets or sets the texture that is applied to geometry rendered by this effect.
        /// </summary>
        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                dirtyFlags |= DirtyFlags.Texture;
            }
        }

        /// <inheritdoc/>
        protected internal override void OnApply()
        {
            if ((dirtyFlags & DirtyFlags.Alpha) == DirtyFlags.Alpha)
            {
                epAlpha?.SetValue(alpha);
            }

            if ((dirtyFlags & DirtyFlags.AmbientLightColor) == DirtyFlags.AmbientLightColor)
            {
                var ambientLightColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(ambientLightColor) : diffuseColor;
                epAmbientLightColor?.SetValue(ambientLightColorSrgb);
            }

            if ((dirtyFlags & DirtyFlags.DiffuseColor) == DirtyFlags.DiffuseColor)
            {
                if (epDiffuseColor != null)
                {
                    var diffuseColorPremul = diffuseColor * alpha;
                    var diffuseColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(diffuseColorPremul) : diffuseColorPremul;
                    epDiffuseColor.SetValue(diffuseColorSrgb);
                }
            }

            if ((dirtyFlags & DirtyFlags.EmissiveColor) == DirtyFlags.EmissiveColor)
            {
                if (epEmissiveColor != null)
                {
                    var emissiveColorPremul = emissiveColor * alpha;
                    var emissiveColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(emissiveColorPremul) : emissiveColorPremul;
                    epEmissiveColor.SetValue(emissiveColorSrgb);
                }
            }

            if ((dirtyFlags & DirtyFlags.FogColor) == DirtyFlags.FogColor)
            {
                var fogColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(fogColor) : fogColor;
                epFogColor?.SetValue(fogColorSrgb);
            }

            if ((dirtyFlags & DirtyFlags.FogStart) == DirtyFlags.FogStart)
            {
                epFogStart?.SetValue(fogStart);
            }

            if ((dirtyFlags & DirtyFlags.FogEnd) == DirtyFlags.FogEnd)
            {
                epFogEnd?.SetValue(fogEnd);
            }

            if ((dirtyFlags & DirtyFlags.World) == DirtyFlags.World)
            {
                epWorld?.SetValue(world);
            }

            if ((dirtyFlags & DirtyFlags.View) == DirtyFlags.View)
            {
                epView?.SetValue(view);
            }

            if ((dirtyFlags & DirtyFlags.Projection) == DirtyFlags.Projection)
            {
                epProjection?.SetValue(projection);
            }

            if ((dirtyFlags & DirtyFlags.WorldViewProj) == DirtyFlags.WorldViewProj)
            {
                if (epWorldViewProj != null)
                {
                    var worldViewProj = Matrix.Identity;
                    Matrix.Multiply(ref worldViewProj, ref world, out worldViewProj);
                    Matrix.Multiply(ref worldViewProj, ref view, out worldViewProj);
                    Matrix.Multiply(ref worldViewProj, ref projection, out worldViewProj);
                    epWorldViewProj.SetValue(worldViewProj);
                }

                if (epEyePosition != null)
                {
                    var invertedViewMatrix = Matrix.Invert(View);
                    epEyePosition.SetValue(invertedViewMatrix.Translation);
                }
            }

            if ((dirtyFlags & DirtyFlags.VertexColorEnabled) == DirtyFlags.VertexColorEnabled)
            {
                epVertexColorEnabled?.SetValue(vertexColorEnabled);
            }

            if ((dirtyFlags & DirtyFlags.TextureEnabled) == DirtyFlags.TextureEnabled)
            {
                epTextureEnabled?.SetValue(textureEnabled);
            }

            if ((dirtyFlags & DirtyFlags.FogEnabled) == DirtyFlags.FogEnabled)
            {
                epFogEnabled?.SetValue(fogEnabled);
            }

            if ((dirtyFlags & DirtyFlags.SrgbColor) == DirtyFlags.SrgbColor)
            {
                epSrgbColor?.SetValue(srgbColor);
            }

            if ((dirtyFlags & DirtyFlags.Texture) == DirtyFlags.Texture)
            {
                epTexture?.SetValue(texture);
            }

            dirtyFlags = DirtyFlags.None;

            base.OnApply();
        }

        // Cached effect parameters.
        private readonly EffectParameter epAlpha;
        private readonly EffectParameter epAmbientLightColor;
        private readonly EffectParameter epDiffuseColor;
        private readonly EffectParameter epEmissiveColor;
        private readonly EffectParameter epFogColor;
        private readonly EffectParameter epFogStart;
        private readonly EffectParameter epFogEnd;
        private readonly EffectParameter epWorld;
        private readonly EffectParameter epView;
        private readonly EffectParameter epProjection;
        private readonly EffectParameter epWorldViewProj;
        private readonly EffectParameter epEyePosition;
        private readonly EffectParameter epVertexColorEnabled;
        private readonly EffectParameter epTextureEnabled;
        private readonly EffectParameter epFogEnabled;
        private readonly EffectParameter epSrgbColor;
        private readonly EffectParameter epTexture;

        // Effect parameter values.
        private Single alpha = 1.0f;
        private Color ambientLightColor;
        private Color diffuseColor = Color.White;
        private Color emissiveColor;
        private Color fogColor;
        private Single fogStart;
        private Single fogEnd;
        private Matrix world;
        private Matrix view;
        private Matrix projection;
        private Boolean vertexColorEnabled;
        private Boolean textureEnabled;
        private Boolean fogEnabled;
        private Boolean srgbColor;
        private Texture2D texture;

        // Dirty flag tracking.
        private DirtyFlags dirtyFlags = DirtyFlags.None;
    }
}
