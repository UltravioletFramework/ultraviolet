using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="RasterizerState"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="RasterizerState"/> that was created.</returns>
    public delegate RasterizerState RasterizerStateFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a graphics device's rasterizer state.
    /// </summary>
    public abstract class RasterizerState : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="RasterizerState"/> type.
        /// </summary>
        static RasterizerState()
        {
            UltravioletContext.ContextInvalidated += (sender, e) => { InvalidateCache(); };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected RasterizerState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="RasterizerState"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="RasterizerState"/> that was created.</returns>
        public static RasterizerState Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<RasterizerStateFactory>()(uv);
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for culling clockwise faces.
        /// </summary>
        /// <returns>The built-in <see cref="RasterizerState"/> object that was retrieved.</returns>
        public static RasterizerState CullClockwise
        {
            get
            {
                if (cachedCullClockwise != null)
                    return cachedCullClockwise;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedCullClockwise = uv.GetFactoryMethod<RasterizerStateFactory>("CullClockwise")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for culling counter-clockwise faces.
        /// </summary>
        /// <returns>The built-in <see cref="RasterizerState"/> object that was retrieved.</returns>
        public static RasterizerState CullCounterClockwise
        {
            get
            {
                if (cachedCullCounterClockwise != null)
                    return cachedCullCounterClockwise;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedCullCounterClockwise = uv.GetFactoryMethod<RasterizerStateFactory>("CullCounterClockwise")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for disabling back face culling.
        /// </summary>
        /// <returns>The built-in <see cref="RasterizerState"/> object that was retrieved.</returns>
        public static RasterizerState CullNone
        {
            get
            {
                if (cachedCullNone != null)
                    return cachedCullNone;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedCullNone = uv.GetFactoryMethod<RasterizerStateFactory>("CullNone")(uv));
            }
        }

        /// <summary>
        /// Gets or sets the cull mode.
        /// </summary>
        public CullMode CullMode
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cullMode;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                cullMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        public FillMode FillMode
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return fillMode;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                fillMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth bias applied to primitives.
        /// </summary>
        public Single DepthBias
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return depthBias;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                depthBias = value;
            }
        }

        /// <summary>
        /// Gets or sets the coplanar depth bias applied to primitives.
        /// </summary>
        public Single SlopeScaleDepthBias
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return slopeScaleDepthBias;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                slopeScaleDepthBias = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scissor testing is enabled.
        /// </summary>
        public Boolean ScissorTestEnable
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return scissorTestEnable;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                scissorTestEnable = value;
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
            SafeDispose.DisposeRef(ref cachedCullClockwise);
            SafeDispose.DisposeRef(ref cachedCullCounterClockwise);
            SafeDispose.DisposeRef(ref cachedCullNone);
        }

        // Cached state objects.
        private static RasterizerState cachedCullClockwise;
        private static RasterizerState cachedCullCounterClockwise;
        private static RasterizerState cachedCullNone;

        // Property values.
        private CullMode cullMode = CullMode.CullCounterClockwiseFace;
        private FillMode fillMode = FillMode.Solid;
        private Single depthBias;
        private Single slopeScaleDepthBias;
        private Boolean scissorTestEnable = false;

        // State values.
        private Boolean immutable;
    }
}
