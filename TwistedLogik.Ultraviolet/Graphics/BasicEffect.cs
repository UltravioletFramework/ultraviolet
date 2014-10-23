using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
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
    public abstract class BasicEffect : Effect, IEffectMatrices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements this effect.</param>
        protected BasicEffect(EffectImplementation impl)
            : base(impl)
        {
            this.epWorld        = Parameters["World"];
            this.epView         = Parameters["View"];
            this.epProjection   = Parameters["Projection"];
            this.epDiffuseColor = Parameters["DiffuseColor"];
            this.epDiffuseColor.SetValue(Color.White);
            this.epTexture      = Parameters["Texture"];
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
        /// Gets or sets the effect's world matrix.
        /// </summary>
        public Matrix World
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epWorld.GetValueMatrix();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epWorld.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the effect's view matrix.
        /// </summary>
        public Matrix View
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epView.GetValueMatrix();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epView.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the effect's projection matrix.
        /// </summary>
        public Matrix Projection
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epProjection.GetValueMatrix();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epProjection.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertex colors are enabled for this effect.
        /// </summary>
        public Boolean VertexColorEnabled
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vertexColorEnabled;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (vertexColorEnabled != value)
                {
                    vertexColorEnabled = value;
                    OnVertexColorEnabledChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the material's diffuse color.
        /// </summary>
        public Color DiffuseColor
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epDiffuseColor.GetValueColor();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epDiffuseColor.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether textures are enabled for this effect.
        /// </summary>
        public Boolean TextureEnabled
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return textureEnabled;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (textureEnabled != value)
                {
                    textureEnabled = value;
                    OnTextureEnabledChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture that is applied to geometry rendered by this effect.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epTexture.GetValueTexture2D();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epTexture.SetValue(value);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VertexColorEnabled"/> property changes.
        /// </summary>
        protected virtual void OnVertexColorEnabledChanged()
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextureEnabled"/> property changes.
        /// </summary>
        protected virtual void OnTextureEnabledChanged()
        {

        }

        // Cached effect parameters.
        private readonly EffectParameter epWorld;
        private readonly EffectParameter epView;
        private readonly EffectParameter epProjection;
        private readonly EffectParameter epDiffuseColor;
        private readonly EffectParameter epTexture;

        // Property values.
        private Boolean vertexColorEnabled;
        private Boolean textureEnabled;
    }
}
