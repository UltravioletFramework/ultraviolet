using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="DepthStencilState"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="DepthStencilState"/> that was created.</returns>
    public delegate DepthStencilState DepthStencilStateFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a graphics device's depth/stencil state.
    /// </summary>
    public abstract class DepthStencilState : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="DepthStencilState"/> type.
        /// </summary>
        static DepthStencilState()
        {
            UltravioletContext.ContextInvalidated += (sender, e) => { InvalidateCache(); };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected DepthStencilState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="DepthStencilState"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="DepthStencilState"/> that was created.</returns>
        public static DepthStencilState Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<DepthStencilStateFactory>()(uv);
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for using a default depth/stencil test.
        /// </summary>
        /// <returns>The built-in <see cref="DepthStencilState"/> object that was retrieved.</returns>
        public static DepthStencilState Default
        {
            get
            {
                if (cachedDefault != null)
                    return cachedDefault;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedDefault = uv.GetFactoryMethod<DepthStencilStateFactory>("Default")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for using a read-only depth/stencil test.
        /// </summary>
        /// <returns>The built-in <see cref="DepthStencilState"/> object that was retrieved.</returns>
        public static DepthStencilState DepthRead
        {
            get
            {
                if (cachedDepthRead != null)
                    return cachedDepthRead;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedDepthRead = uv.GetFactoryMethod<DepthStencilStateFactory>("DepthRead")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object with settings for not using a depth/stencil test.
        /// </summary>
        /// <returns>The built-in <see cref="DepthStencilState"/> object that was retrieved.</returns>
        public static DepthStencilState None
        {
            get
            {
                if (cachedNone != null)
                    return cachedNone;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedNone = uv.GetFactoryMethod<DepthStencilStateFactory>("None")(uv));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth buffering is enabled.
        /// </summary>
        public Boolean DepthBufferEnable
        {
            get => depthBufferEnable;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                depthBufferEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether writing to the depth buffer is enabled.
        /// </summary>
        public Boolean DepthBufferWriteEnable
        {
            get => depthBufferWriteEnable;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                depthBufferWriteEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets the comparison function used by the depth buffer test.
        /// </summary>
        public CompareFunction DepthBufferFunction
        {
            get => depthBufferFunction;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                depthBufferFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether stencil buffering is enabled.
        /// </summary>
        public Boolean StencilEnable
        {
            get => stencilEnable;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilEnable = value;
            }
        }

        /// <summary>
        /// Gets or sets the comparison function used by the stencil buffer test.
        /// </summary>
        public CompareFunction StencilFunction
        {
            get => stencilFunction;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test passes.
        /// </summary>
        public StencilOperation StencilPass
        {
            get => stencilPass;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilPass = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test fails.
        /// </summary>
        public StencilOperation StencilFail
        {
            get => stencilFail;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test passes and the depth buffer test fails.
        /// </summary>
        public StencilOperation StencilDepthBufferFail
        {
            get => stencilDepthBufferFail;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilDepthBufferFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the comparison function used by the stencil buffer test when
        /// testing counter-clockwise triangles.
        /// </summary>
        public CompareFunction CounterClockwiseStencilFunction
        {
            get => counterClockwiseStencilFunction;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                counterClockwiseStencilFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test passes
        /// when testing counter-clockwise triangles.
        /// </summary>
        public StencilOperation CounterClockwiseStencilPass
        {
            get => counterClockwiseStencilPass;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                counterClockwiseStencilPass = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test fails
        /// when testing counter-clockwise triangles.
        /// </summary>
        public StencilOperation CounterClockwiseStencilFail
        {
            get => counterClockwiseStencilFail;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                counterClockwiseStencilFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the stencil operation to perform if the stencil test passes and the depth buffer test fails
        /// when testing counter-clockwise triangles.
        /// </summary>
        public StencilOperation CounterClockwiseStencilDepthBufferFail
        {
            get => counterClockwiseStencilDepthBufferFail;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                counterClockwiseStencilDepthBufferFail = value;
            }
        }

        /// <summary>
        /// Gets or sets the mask applied to determine the significant bits of values read from the stencil buffer.
        /// </summary>
        public Int32 StencilMask
        {
            get => stencilMask;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the mask applied to determine the significant bits of values written to the stencil buffer.
        /// </summary>
        public Int32 StencilWriteMask
        {
            get => stencilWriteMask;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                stencilWriteMask = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference value used by the stencil test.
        /// </summary>
        public Int32 ReferenceStencil
        {
            get => referenceStencil;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                referenceStencil = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether two-sided stenciling is enabled.
        /// </summary>
        public Boolean TwoSidedStencilMode
        {
            get => twoSidedStencilMode;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                twoSidedStencilMode = value;
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
            SafeDispose.DisposeRef(ref cachedDefault);
            SafeDispose.DisposeRef(ref cachedDepthRead);
            SafeDispose.DisposeRef(ref cachedNone);
        }

        // Cached state objects.
        private static DepthStencilState cachedDefault;
        private static DepthStencilState cachedDepthRead;
        private static DepthStencilState cachedNone;

        // Property values.
        private Boolean depthBufferEnable = true;
        private Boolean depthBufferWriteEnable = true;
        private CompareFunction depthBufferFunction = CompareFunction.LessEqual;
        private Boolean stencilEnable = false;
        private StencilOperation stencilPass = StencilOperation.Keep;
        private StencilOperation stencilFail = StencilOperation.Keep;
        private StencilOperation stencilDepthBufferFail = StencilOperation.Keep;
        private CompareFunction stencilFunction = CompareFunction.Always;
        private StencilOperation counterClockwiseStencilPass = StencilOperation.Keep;
        private StencilOperation counterClockwiseStencilFail = StencilOperation.Keep;
        private StencilOperation counterClockwiseStencilDepthBufferFail = StencilOperation.Keep;
        private CompareFunction counterClockwiseStencilFunction = CompareFunction.Always;
        private Int32 stencilMask = Int32.MaxValue;
        private Int32 stencilWriteMask = Int32.MaxValue;
        private Int32 referenceStencil;
        private Boolean twoSidedStencilMode;

        // State values.
        private Boolean immutable;
    }
}
