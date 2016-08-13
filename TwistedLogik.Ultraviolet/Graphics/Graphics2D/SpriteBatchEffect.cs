using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SpriteBatchEffect"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SpriteBatchEffect"/> that was created.</returns>
    public delegate SpriteBatchEffect SpriteBatchEffectFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the <see cref="Effect"/> used by <see cref="SpriteBatchBase{VertexType, SpriteData}"/> to render sprites.
    /// </summary>
    public abstract class SpriteBatchEffect : Effect, ISpriteBatchEffect, IEffectTextureSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements the effect.</param>
        protected SpriteBatchEffect(EffectImplementation impl)
            : base(impl)
        {
            this.epMatrixTransform = Parameters["MatrixTransform"];
            this.epTextureSize = Parameters["TextureSize"];
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpriteBatchEffect"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SpriteBatchEffect"/> that was created.</returns>
        public static SpriteBatchEffect Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SpriteBatchEffectFactory>()(uv);
        }

        /// <inheritdoc/>
        public Matrix MatrixTransform
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epMatrixTransform.GetValueMatrix();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epMatrixTransform.SetValue(value);
            }
        }

        /// <inheritdoc/>
        public Size2 TextureSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (Size2)epTextureSize.GetValueVector2();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epTextureSize.SetValue((Vector2)value);
            }
        }

        // Cached effect parameters.
        private readonly EffectParameter epMatrixTransform;
        private readonly EffectParameter epTextureSize;
    }
}
