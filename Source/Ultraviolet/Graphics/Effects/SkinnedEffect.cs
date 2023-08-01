using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SkinnedEffect"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SkinnedEffect"/> that was created.</returns>
    public delegate SkinnedEffect SkinnedEffectFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a basic rendering effect.
    /// </summary>
    public abstract partial class SkinnedEffect : Effect,
        IEffectMatrices, IEffectFog, IEffectLights, IEffectTexture, IEffectMaterialColor, IEffectSkin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements this effect.</param>
        protected SkinnedEffect(EffectImplementation impl)
            : base(impl)
        {
            DirectionalLight0 = CreateDirectionalLight(0);
            DirectionalLight1 = CreateDirectionalLight(1);
            DirectionalLight2 = CreateDirectionalLight(2);
        }

        /// <summary>
        /// Creates the directional light with the specified index.
        /// </summary>
        /// <param name="index">The index of the directional light to create.</param>
        /// <returns>The directional light which was created.</returns>
        protected abstract DirectionalLight CreateDirectionalLight(Int32 index);

        /// <summary>
        /// Creates a new instance of the <see cref="SkinnedEffect"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SkinnedEffect"/> that was created.</returns>
        public static SkinnedEffect Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SkinnedEffectFactory>()(uv);
        }

        /// <summary>
        /// Sets up the standard lighting rig, consisting of a key, fill, and back light.
        /// </summary>
        public void EnableStandardLighting()
        {
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

        /// <inheritdoc/>
        public abstract void SetBoneTransforms(Matrix[] boneTransforms);

        /// <inheritdoc/>
        public abstract void GetBoneTransforms(Matrix[] boneTransforms);

        /// <inheritdoc/>
        public abstract void GetBoneTransforms(Matrix[] boneTransforms, Int32 count);

        /// <inheritdoc/>
        Int32 IEffectSkin.MaxBoneCount { get; } = MaxBoneCount;

        /// <summary>
        /// Gets the maximum number of bones which <see cref="SkinnedEffect"/> supports.
        /// </summary>
        public const Int32 MaxBoneCount = 72;

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
                OnSrgbColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="SrgbColor"/> property is set.
        /// </summary>
        protected virtual void OnSrgbColorSet() { }
        private Boolean srgbColor;

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
                OnPreferPerPixelLightingSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="PreferPerPixelLighting"/> property is set.
        /// </summary>
        protected virtual void OnPreferPerPixelLightingSet() { }
        private Boolean preferPerPixelLighting;

        /// <summary>
        /// Gets or sets the material alpha, which determines its transparency. Range is between 1.0f (fully opaque) and 0.0f (fully transparent).
        /// </summary>
        public Single Alpha
        {
            get => alpha;
            set
            {
                alpha = value;
                OnAlphaSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Alpha"/> property is set.
        /// </summary>
        protected virtual void OnAlphaSet() { }
        private Single alpha = 1.0f;

        /// <summary>
        /// Gets or sets the material's diffuse color.
        /// </summary>
        public Color DiffuseColor
        {
            get => diffuseColor;
            set
            {
                diffuseColor = value;
                OnDiffuseColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="DiffuseColor"/> property is set.
        /// </summary>
        protected virtual void OnDiffuseColorSet() { }
        private Color diffuseColor = Color.White;

        /// <summary>
        /// Gets or sets the material's emissive color.
        /// </summary>
        public Color EmissiveColor
        {
            get => emissiveColor;
            set
            {
                emissiveColor = value;
                OnEmissiveColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="EmissiveColor"/> property is set.
        /// </summary>
        protected virtual void OnEmissiveColorSet() { }
        private Color emissiveColor;

        /// <summary>
        /// Gets or sets the material's specular color.
        /// </summary>
        public Color SpecularColor
        {
            get => specularColor;
            set
            {
                specularColor = value;
                OnSpecularColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="SpecularColor"/> property is set.
        /// </summary>
        protected virtual void OnSpecularColorSet() { }
        private Color specularColor = Color.White;

        /// <summary>
        /// Gets or sets the material's specular power.
        /// </summary>
        public Single SpecularPower
        {
            get => specularPower;
            set
            {
                specularPower = value;
                OnSpecularPowerSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="SpecularPower"/> property is set.
        /// </summary>
        protected virtual void OnSpecularPowerSet() { }
        private Single specularPower = 16.0f;

        /// <inheritdoc/>
        public Matrix World
        {
            get => world;
            set
            {
                world = value;
                OnWorldSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="World"/> property is set.
        /// </summary>
        protected virtual void OnWorldSet() { }
        private Matrix world;

        /// <inheritdoc/>
        public Matrix View
        {
            get => view;
            set
            {
                view = value;
                OnViewSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="View"/> property is set.
        /// </summary>
        protected virtual void OnViewSet() { }
        private Matrix view;

        /// <inheritdoc/>
        public Matrix Projection
        {
            get => projection;
            set
            {
                projection = value;
                OnProjectionSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Projection"/> property is set.
        /// </summary>
        protected virtual void OnProjectionSet() { }
        private Matrix projection;

        /// <inheritdoc/>
        public Color AmbientLightColor
        {
            get => ambientLightColor;
            set
            {
                ambientLightColor = value;
                OnAmbientLightColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="AmbientLightColor"/> property is set.
        /// </summary>
        protected virtual void OnAmbientLightColorSet() { }
        private Color ambientLightColor;

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
                OnFogEnabledSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="FogEnabled"/> property is set.
        /// </summary>
        protected virtual void OnFogEnabledSet() { }
        private Boolean fogEnabled;

        /// <inheritdoc/>
        public Color FogColor
        {
            get => fogColor;
            set
            {
                fogColor = value;
                OnFogColorSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="FogColor"/> property is set.
        /// </summary>
        protected virtual void OnFogColorSet() { }
        private Color fogColor;

        /// <inheritdoc/>
        public Single FogStart
        {
            get => fogStart;
            set
            {
                fogStart = value;
                OnFogStartSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="FogStart"/> property is set.
        /// </summary>
        protected virtual void OnFogStartSet() { }
        private Single fogStart;

        /// <inheritdoc/>
        public Single FogEnd
        {
            get => fogEnd;
            set
            {
                fogEnd = value;
                OnFogEndSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="FogEnd"/> property is set.
        /// </summary>
        protected virtual void OnFogEndSet() { }
        private Single fogEnd;

        /// <inheritdoc/>
        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                OnTextureSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Texture"/> property is set.
        /// </summary>
        protected virtual void OnTextureSet() { }
        private Texture2D texture;

        /// <inheritdoc/>
        public Int32 WeightsPerVertex
        {
            get => weightsPerVertex;
            set
            {
                if (value != 1 && value != 2 && value != 4)
                    throw new ArgumentOutOfRangeException(nameof(value));

                weightsPerVertex = value;
                OnWeightsPerVertexSet();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="WeightsPerVertex"/> property is set.
        /// </summary>
        protected virtual void OnWeightsPerVertexSet() { }
        private Int32 weightsPerVertex = 4;

        /// <inheritdoc/>
        Boolean IEffectLights.LightingEnabled
        {
            get => true;
            set => throw new NotSupportedException();
        }
    }
}
