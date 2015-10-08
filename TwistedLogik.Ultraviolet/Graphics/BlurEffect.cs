using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="BlurEffect"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="BlurEffect"/> that was created.</returns>
    public delegate BlurEffect BlurEffectFactory(UltravioletContext uv);

    /// <summary>
    /// Represents an <see cref="Effect"/> which draws 2D drop shadows using a two-pass Gaussian blur.
    /// </summary>
    public class BlurEffect : Effect, IEffectMatrices, IEffectTextureSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlurEffect"/> class.
        /// </summary>
        /// <param name="impl">The <see cref="EffectImplementation"/> that implements this effect.</param>
        protected BlurEffect(EffectImplementation impl)
            : base(impl)
        {
            this.epWorld      = Parameters["World"];
            this.epView       = Parameters["View"];
            this.epProjection = Parameters["Projection"];
            this.epMix        = Parameters["Mix"];
            this.epRadius     = Parameters["Radius"];

            this.Radius = 5f;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BlurEffect"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="BlurEffect"/> that was created.</returns>
        public static BlurEffect Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<BlurEffectFactory>()(uv);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Size2 TextureSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return textureSize;
            }
            set
            {

                Contract.EnsureNotDisposed(this, Disposed);

                if (textureSize != value)
                {
                    textureSize = value;
                    OnTextureSizeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the blur mixing value. A value of 0f specifies that the output of the blur should consist entirely
        /// of the texture color; a value of 1f specifies that the output of the blur should consist entirely of the vertex color.
        /// </summary>
        public Single Mix
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epMix.GetValueSingle();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epMix.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the blur radius of the drop shadow.
        /// </summary>
        public Single Radius
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return epRadius.GetValueSingle();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                epRadius.SetValue(value);
                OnRadiusChanged();
            }
        }

        /// <summary>
        /// Gets or sets the direction in which the blur is applied.
        /// </summary>
        public BlurDirection Direction
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return direction;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (direction != value)
                {
                    direction = value;
                    OnDirectionChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextureSize"/> property changes.
        /// </summary>
        protected virtual void OnTextureSizeChanged()
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Radius"/> property changes.
        /// </summary>
        protected virtual void OnRadiusChanged()
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Direction"/> property changes.
        /// </summary>
        protected virtual void OnDirectionChanged()
        {

        }

        // Property values.
        private BlurDirection direction;
        private Size2 textureSize;

        // Cached effect parameters.
        private readonly EffectParameter epWorld;
        private readonly EffectParameter epView;
        private readonly EffectParameter epProjection;
        private readonly EffectParameter epMix;
        private readonly EffectParameter epRadius;      
    }
}
