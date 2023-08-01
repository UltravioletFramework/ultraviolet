using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="BlendState"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="BlendState"/> that was created.</returns>
    public delegate BlendState BlendStateFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a graphics device's blend state.
    /// </summary>
    public abstract class BlendState : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="BlendState"/> type.
        /// </summary>
        static BlendState()
        {
            UltravioletContext.ContextInvalidated += (sender, e) => { InvalidateCache(); };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected BlendState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="BlendState"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="BlendState"/> that was created.</returns>
        public static BlendState Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<BlendStateFactory>()(uv);
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for opaque blending.
        /// </summary>
        /// <returns>The built-in <see cref="BlendState"/> object that was retrieved.</returns>
        public static BlendState Opaque
        {
            get
            {
                if (cachedOpaque != null)
                    return cachedOpaque;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedOpaque = uv.GetFactoryMethod<BlendStateFactory>("Opaque")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for alpha blending.
        /// </summary>
        /// <returns>The built-in <see cref="BlendState"/> object that was retrieved.</returns>
        public static BlendState AlphaBlend
        {
            get
            {
                if (cachedAlphaBlend != null)
                    return cachedAlphaBlend;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedAlphaBlend = uv.GetFactoryMethod<BlendStateFactory>("AlphaBlend")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for additive blending.
        /// </summary>
        /// <returns>The built-in <see cref="BlendState"/> object that was retrieved.</returns>
        public static BlendState Additive
        {
            get
            {
                if (cachedAdditive != null)
                    return cachedAdditive;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedAdditive = uv.GetFactoryMethod<BlendStateFactory>("Additive")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for non-premultiplied blending.
        /// </summary>
        /// <returns>The built-in <see cref="BlendState"/> object that was retrieved.</returns>
        public static BlendState NonPremultiplied
        {
            get
            {
                if (cachedNonPremultiplied != null)
                    return cachedNonPremultiplied;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedNonPremultiplied = uv.GetFactoryMethod<BlendStateFactory>("NonPremultiplied")(uv));
            }
        }

        /// <summary>
        /// Gets or sets the alpha blending function.
        /// </summary>
        public BlendFunction AlphaBlendFunction
        {
            get => alphaBlendFunction;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);
                
                alphaBlendFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets the source factor for alpha blending.
        /// </summary>
        public Blend AlphaSourceBlend
        {
            get => alphaSourceBlend;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);
                
                alphaSourceBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the destination factor for alpha blending.
        /// </summary>
        public Blend AlphaDestinationBlend
        {
            get => alphaDestinationBlend;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                alphaDestinationBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the color blending function.
        /// </summary>
        public BlendFunction ColorBlendFunction
        {
            get => colorBlendFunction;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                colorBlendFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets the source factor for color blending.
        /// </summary>
        public Blend ColorSourceBlend
        {
            get => colorSourceBlend;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                colorSourceBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the destination factor for color blending.
        /// </summary>
        public Blend ColorDestinationBlend
        {
            get => colorDestinationBlend;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                colorDestinationBlend = value;
            }
        }

        /// <summary>
        /// Gets or sets the blend factor used for alpha blending.
        /// </summary>
        public Color BlendFactor
        {
            get => blendFactor;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                blendFactor = value;
            }
        }

        /// <summary>
        /// Gets a value specifying which color channels are written when this blend state is in effect.
        /// </summary>
        public ColorWriteChannels ColorWriteChannels
        {
            get => colorWriteChannels;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                colorWriteChannels = value;
            }
        }

        /// <summary>
        /// Makes the state object immutable.  Further attempts to modify
        /// the object will throw an exception.
        /// </summary>
        protected void MakeImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Invalidates the cached state objects.
        /// </summary>
        private static void InvalidateCache()
        {
            SafeDispose.DisposeRef(ref cachedOpaque);
            SafeDispose.DisposeRef(ref cachedAlphaBlend);
            SafeDispose.DisposeRef(ref cachedAdditive);
            SafeDispose.DisposeRef(ref cachedNonPremultiplied);
        }

        // Cached state objects.
        private static BlendState cachedOpaque;
        private static BlendState cachedAlphaBlend;
        private static BlendState cachedAdditive;
        private static BlendState cachedNonPremultiplied;

        // Property values.
        private BlendFunction alphaBlendFunction;
        private Blend alphaSourceBlend;
        private Blend alphaDestinationBlend;
        private BlendFunction colorBlendFunction;
        private Blend colorSourceBlend;
        private Blend colorDestinationBlend;
        private Color blendFactor;
        private ColorWriteChannels colorWriteChannels = ColorWriteChannels.All;

        // State values.
        private Boolean immutable;
    }
}
