using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SamplerState"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SamplerState"/> that was created.</returns>
    public delegate SamplerState SamplerStateFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a graphics device's rasterizer state.
    /// </summary>
    public abstract class SamplerState : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="SamplerState"/> type.
        /// </summary>
        static SamplerState()
        {
            UltravioletContext.ContextInvalidated += (sender, e) => { InvalidateCache(); };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected SamplerState(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="SamplerState"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SamplerState"/> that was created.</returns>
        public static SamplerState Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SamplerStateFactory>()(uv);
        }

        /// <summary>
        /// Retrieves a built-in state object using point filtering and texture coordinate clamping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState PointClamp
        {
            get
            {
                if (cachedPointClamp != null)
                    return cachedPointClamp;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedPointClamp = uv.GetFactoryMethod<SamplerStateFactory>("PointClamp")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object using point filtering and texture coordinate wrapping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState PointWrap
        {
            get
            {
                if (cachedPointWrap != null)
                    return cachedPointWrap;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedPointWrap = uv.GetFactoryMethod<SamplerStateFactory>("PointWrap")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object using linear filtering and texture coordinate clamping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState LinearClamp
        {
            get
            {
                if (cachedLinearClamp != null)
                    return cachedLinearClamp;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedLinearClamp = uv.GetFactoryMethod<SamplerStateFactory>("LinearClamp")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object using linear filtering and texture coordinate wrapping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState LinearWrap
        {
            get
            {
                if (cachedLinearWrap != null)
                    return cachedLinearWrap;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedLinearWrap = uv.GetFactoryMethod<SamplerStateFactory>("LinearWrap")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object using anisotropic filtering and texture coordinate clamping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState AnisotropicClamp
        {
            get
            {
                if (cachedAnisotropicClamp != null)
                    return cachedAnisotropicClamp;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedAnisotropicClamp = uv.GetFactoryMethod<SamplerStateFactory>("AnisotropicClamp")(uv));
            }
        }

        /// <summary>
        /// Retrieves a built-in state object using anisotropic filtering and texture coordinate wrapping.
        /// </summary>
        /// <returns>The built-in <see cref="SamplerState"/> object that was retrieved.</returns>
        public static SamplerState AnisotropicWrap
        {
            get
            {
                if (cachedAnisotropicWrap != null)
                    return cachedAnisotropicWrap;

                var uv = UltravioletContext.DemandCurrent();
                return (cachedAnisotropicWrap = uv.GetFactoryMethod<SamplerStateFactory>("AnisotropicWrap")(uv));
            }
        }

        /// <summary>
        /// Gets or sets the texture address mode for the u-coordinate.
        /// </summary>
        public TextureAddressMode AddressU
        {
            get => addressU;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                addressU = value;
            }
        }

        /// <summary>
        /// Gets or sets the texture address mode for the v-coordinate.
        /// </summary>
        public TextureAddressMode AddressV
        {
            get => addressV;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                addressV = value;
            }
        }

        /// <summary>
        /// Gets or sets the texture address mode for the w-coordinate.
        /// </summary>
        public TextureAddressMode AddressW
        {
            get => addressW;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                addressW = value;
            }
        }

        /// <summary>
        /// Gets or sets the texture filtering mode.
        /// </summary>
        public TextureFilter Filter
        {
            get => filter;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                filter = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum anisotropy.
        /// </summary>
        public Int32 MaxAnisotropy
        {
            get => maxAnisotropy;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                maxAnisotropy = value;
            }
        }

        /// <summary>
        /// Gets or sets the mipmap level of detail bias.
        /// </summary>
        public Single MipMapLevelOfDetailBias
        {
            get => mipMapLevelOfDetailBias;
            set
            {
                Contract.EnsureNot(immutable, UltravioletStrings.StateIsImmutableAfterBind);

                mipMapLevelOfDetailBias = value;
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
            SafeDispose.DisposeRef(ref cachedPointClamp);
            SafeDispose.DisposeRef(ref cachedPointWrap);
            SafeDispose.DisposeRef(ref cachedLinearClamp);
            SafeDispose.DisposeRef(ref cachedLinearWrap);
            SafeDispose.DisposeRef(ref cachedAnisotropicClamp);
            SafeDispose.DisposeRef(ref cachedAnisotropicWrap);
        }

        // Cached state objects.
        private static SamplerState cachedPointClamp;
        private static SamplerState cachedPointWrap;
        private static SamplerState cachedLinearClamp;
        private static SamplerState cachedLinearWrap;
        private static SamplerState cachedAnisotropicClamp;
        private static SamplerState cachedAnisotropicWrap;

        // Property values.
        private TextureAddressMode addressU;
        private TextureAddressMode addressV;
        private TextureAddressMode addressW;
        private TextureFilter filter;
        private Int32 maxAnisotropy = 4;
        private Single mipMapLevelOfDetailBias;

        // State values.
        private Boolean immutable;
    }
}
