using System;

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
    public abstract partial class BasicEffect : Effect, 
        IEffectMatrices, IEffectFog, IEffectLights, IEffectTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements this effect.</param>
        protected BasicEffect(EffectImplementation impl)
            : base(impl)
        {
            // General parameters
            this.epSrgbColor = Parameters["SrgbColor"];
            this.epPreferPerPixelLighting = Parameters["PreferPerPixelLighting"];

            // Vertex color parameters
            this.epVertexColorEnabled = Parameters["VertexColorEnabled"];
            this.epAlpha = Parameters["Alpha"];
            this.epDiffuseColor = Parameters["DiffuseColor"];
            this.epEmissiveColor = Parameters["EmissiveColor"];
            this.epSpecularColor = Parameters["SpecularColor"];
            this.epSpecularPower = Parameters["SpecularPower"];

            // Fog parameters
            this.epFogEnabled = Parameters["FogEnabled"];
            this.epFogColor = Parameters["FogColor"];
            this.epFogStart = Parameters["FogStart"];
            this.epFogEnd = Parameters["FogEnd"];

            // Matrix parameters
            this.epWorld = Parameters["World"];
            this.epView = Parameters["View"];
            this.epProjection = Parameters["Projection"];
            this.epWorldViewProj = Parameters["WorldViewProj"];
            this.epEyePosition = Parameters["EyePosition"];

            // Texture parameters
            this.epTextureEnabled = Parameters["TextureEnabled"];
            this.epTexture = Parameters["Texture"];

            // Lighting parameters
            this.epLightingEnabled = Parameters["LightingEnabled"];
            this.epAmbientLightColor = Parameters["AmbientLightColor"];
            this.DirectionalLight0 = new DirectionalLight(
                Parameters["DirLight0Direction"], 
                Parameters["DirLight0DiffuseColor"], 
                Parameters["DirLight0SpecularColor"]);
            this.DirectionalLight1 = new DirectionalLight(
                Parameters["DirLight1Direction"],
                Parameters["DirLight1DiffuseColor"],
                Parameters["DirLight1SpecularColor"]);
            this.DirectionalLight2 = new DirectionalLight(
                Parameters["DirLight2Direction"],
                Parameters["DirLight2DiffuseColor"],
                Parameters["DirLight2SpecularColor"]);
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
        /// Sets up the standard lighting rig, consisting of a key, fill, and back light.
        /// </summary>
        public void EnableStandardLighting()
        {
            LightingEnabled = true;
            AmbientLightColor = new Color(0.05333332f, 0.09882354f, 0.1819608f, 1f);

            // Key light.
            DirectionalLight0.Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f);
            DirectionalLight0.DiffuseColor = new Color(1f, 0.9607844f, 0.8078432f, 1f);
            DirectionalLight0.SpecularColor = Color.Black;
            DirectionalLight0.Enabled = true;

            // Fill light.
            DirectionalLight1.Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f);
            DirectionalLight1.DiffuseColor = new Color(0.9647059f, 0.7607844f, 0.4078432f, 1f);
            DirectionalLight1.SpecularColor = Color.Black;
            DirectionalLight1.Enabled = true;

            // Back light.
            DirectionalLight2.Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f);
            DirectionalLight2.DiffuseColor = new Color(0.3231373f, 0.3607844f, 0.3937255f, 1f);
            DirectionalLight2.SpecularColor = new Color(0.3231373f, 0.3607844f, 0.3937255f, 1f);
            DirectionalLight2.Enabled = true;
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
                this.DirectionalLight0.SrgbColor = value;
                this.DirectionalLight1.SrgbColor = value;
                this.DirectionalLight2.SrgbColor = value;

                dirtyFlags |= DirtyFlags.SrgbColor | DirtyFlags.DiffuseColor | DirtyFlags.EmissiveColor | DirtyFlags.SpecularColor | DirtyFlags.FogColor | DirtyFlags.AmbientLightColor;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the effect should prefer per-pixel lighting
        /// over vertex lighting on platforms where it is supported.
        /// </summary>
        public Boolean PreferPerPixelLighting
        {
            get => preferPerPixelLighting;
            set
            {
                preferPerPixelLighting = value;
                dirtyFlags |= DirtyFlags.PreferPerPixelLighting;
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
        /// Gets or sets the material's specular color.
        /// </summary>
        public Color SpecularColor
        {
            get => specularColor;
            set
            {
                specularColor = value;
                dirtyFlags |= DirtyFlags.SpecularColor;
            }
        }

        /// <summary>
        /// Gets or sets the material's specular power.
        /// </summary>
        public Single SpecularPower
        {
            get => specularPower;
            set
            {
                specularPower = value;
                dirtyFlags |= DirtyFlags.SpecularPower;
            }
        }

        /// <inheritdoc/>
        public Matrix World
        {
            get => world;
            set
            {
                world = value;
                dirtyFlags |= DirtyFlags.World | DirtyFlags.Matrices;
            }
        }

        /// <inheritdoc/>
        public Matrix View
        {
            get => view;
            set
            {
                view = value;
                dirtyFlags |= DirtyFlags.View | DirtyFlags.Matrices;
            }
        }

        /// <inheritdoc/>
        public Matrix Projection
        {
            get => projection;
            set
            {
                projection = value;
                dirtyFlags |= DirtyFlags.Projection | DirtyFlags.Matrices;
            }
        }

        /// <inheritdoc/>
        public Boolean LightingEnabled
        {
            get => lightingEnabled;
            set
            {
                lightingEnabled = value;
                dirtyFlags |= DirtyFlags.LightingEnabled;
            }
        }

        /// <inheritdoc/>
        public Color AmbientLightColor
        {
            get => ambientLightColor;
            set
            {
                ambientLightColor = value;
                dirtyFlags |= DirtyFlags.AmbientLightColor;
            }
        }

        /// <inheritdoc/>
        public DirectionalLight DirectionalLight0 { get; }

        /// <inheritdoc/>
        public DirectionalLight DirectionalLight1 { get; }

        /// <inheritdoc/>
        public DirectionalLight DirectionalLight2 { get; }

        /// <inheritdoc/>
        public Boolean FogEnabled
        {
            get => fogEnabled;
            set
            {
                fogEnabled = value;
                dirtyFlags |= DirtyFlags.FogEnabled;
            }
        }

        /// <inheritdoc/>
        public Color FogColor
        {
            get => fogColor;
            set
            {
                fogColor = value;
                dirtyFlags |= DirtyFlags.FogColor;
            }
        }

        /// <inheritdoc/>
        public Single FogStart
        {
            get => fogStart;
            set
            {
                fogStart = value;
                dirtyFlags |= DirtyFlags.FogStart;
            }
        }

        /// <inheritdoc/>
        public Single FogEnd
        {
            get => fogEnd;
            set
            {
                fogEnd = value;
                dirtyFlags |= DirtyFlags.FogEnd;
            }
        }

        /// <inheritdoc/>
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
            // General parameters
            if ((dirtyFlags & DirtyFlags.SrgbColor) == DirtyFlags.SrgbColor)
            {
                epSrgbColor?.SetValue(srgbColor);
            }
            if ((dirtyFlags & DirtyFlags.PreferPerPixelLighting) == DirtyFlags.PreferPerPixelLighting)
            {
                epPreferPerPixelLighting?.SetValue(preferPerPixelLighting);
            }

            // Vertex color parameters.
            if ((dirtyFlags & DirtyFlags.VertexColorEnabled) == DirtyFlags.VertexColorEnabled)
            {
                epVertexColorEnabled?.SetValue(vertexColorEnabled);
            }

            if ((dirtyFlags & DirtyFlags.Alpha) == DirtyFlags.Alpha)
            {
                epAlpha?.SetValue(alpha);
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

            if ((dirtyFlags & DirtyFlags.SpecularColor) == DirtyFlags.SpecularColor)
            {
                if (epSpecularColor != null)
                {
                    var specularColorPremul = specularColor * alpha;
                    var specularColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(specularColorPremul) : specularColorPremul;
                    epSpecularColor.SetValue(specularColorSrgb);
                }
            }

            if ((dirtyFlags & DirtyFlags.SpecularPower) == DirtyFlags.SpecularPower)
            {
                epSpecularPower?.SetValue(specularPower);
            }

            // Fog parameters
            if ((dirtyFlags & DirtyFlags.FogEnabled) == DirtyFlags.FogEnabled)
            {
                epFogEnabled?.SetValue(fogEnabled);
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

            // Matrix parameters
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

            if ((dirtyFlags & DirtyFlags.Matrices) == DirtyFlags.Matrices)
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

            // Texture parameters
            if ((dirtyFlags & DirtyFlags.TextureEnabled) == DirtyFlags.TextureEnabled)
            {
                epTextureEnabled?.SetValue(textureEnabled);
            }

            if ((dirtyFlags & DirtyFlags.Texture) == DirtyFlags.Texture)
            {
                epTexture?.SetValue(texture);
            }

            // Lighting parameters
            if ((dirtyFlags & DirtyFlags.LightingEnabled) == DirtyFlags.LightingEnabled)
            {
                epLightingEnabled?.SetValue(lightingEnabled);
            }

            if ((dirtyFlags & DirtyFlags.AmbientLightColor) == DirtyFlags.AmbientLightColor)
            {
                var ambientLightColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(ambientLightColor) : diffuseColor;
                epAmbientLightColor?.SetValue(ambientLightColorSrgb);
            }

            DirectionalLight0.Apply();
            DirectionalLight1.Apply();
            DirectionalLight2.Apply();

            dirtyFlags = DirtyFlags.None;

            base.OnApply();
        }

        // General parameters.
        private readonly EffectParameter epSrgbColor;
        private readonly EffectParameter epPreferPerPixelLighting;

        // General parameter values.
        private Boolean srgbColor;
        private Boolean preferPerPixelLighting;

        // Vertex color parameters.
        private readonly EffectParameter epVertexColorEnabled;
        private readonly EffectParameter epAlpha;
        private readonly EffectParameter epDiffuseColor;
        private readonly EffectParameter epEmissiveColor;
        private readonly EffectParameter epSpecularColor;
        private readonly EffectParameter epSpecularPower;

        // Vertex color parameter values.
        private Boolean vertexColorEnabled;
        private Single alpha = 1.0f;
        private Color diffuseColor = Color.White;
        private Color emissiveColor;
        private Color specularColor = Color.White;
        private Single specularPower = 16f;

        // Fog parameters.
        private readonly EffectParameter epFogEnabled;
        private readonly EffectParameter epFogColor;
        private readonly EffectParameter epFogStart;
        private readonly EffectParameter epFogEnd;

        // Fog parameter values.
        private Boolean fogEnabled;
        private Color fogColor;
        private Single fogStart;
        private Single fogEnd;

        // Matrix parameters.
        private readonly EffectParameter epWorld;
        private readonly EffectParameter epView;
        private readonly EffectParameter epProjection;
        private readonly EffectParameter epWorldViewProj;
        private readonly EffectParameter epEyePosition;

        // Matrix parameter values.
        private Matrix world;
        private Matrix view;
        private Matrix projection;

        // Texture parameters.
        private readonly EffectParameter epTextureEnabled;
        private readonly EffectParameter epTexture;

        // Texture parameter values.
        private Boolean textureEnabled;
        private Texture2D texture;

        // Lighting parameters.
        private readonly EffectParameter epLightingEnabled;
        private readonly EffectParameter epAmbientLightColor;

        // Lighting parameter values.
        private Boolean lightingEnabled;
        private Color ambientLightColor;

        // Dirty flag tracking.
        private DirtyFlags dirtyFlags = DirtyFlags.Alpha | DirtyFlags.DiffuseColor | DirtyFlags.SpecularColor | DirtyFlags.SpecularPower;
    }
}
