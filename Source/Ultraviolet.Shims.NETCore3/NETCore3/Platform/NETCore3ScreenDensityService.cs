using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore3.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="ScreenDensityService"/> class for the .NET Core 3.0 platform.
    /// </summary>
    public sealed partial class NETCore3ScreenDensityService : ScreenDensityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCore3ScreenDensityService"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="display">The <see cref="IUltravioletDisplay"/> for which to retrieve density information.</param>
        public NETCore3ScreenDensityService(UltravioletContext uv, IUltravioletDisplay display)
            : base(display)
        {
            Contract.Require(uv, nameof(uv));

            Refresh();
        }

        /// <inheritdoc/>
        public override Boolean Refresh()
        {
            this.densityX = 96f;
            this.densityY = 96f;
            this.densityScale = 1f;
            this.densityBucket = ScreenDensityBucket.Desktop;

            return false;
        }

        /// <inheritdoc/>
        public override Single DeviceScale
        {
            get { return 1f; }
        }

        /// <inheritdoc/>
        public override Single DensityScale
        {
            get { return densityScale; }
        }

        /// <inheritdoc/>
        public override Single DensityX
        {
            get { return densityX; }
        }

        /// <inheritdoc/>
        public override Single DensityY
        {
            get { return densityY; }
        }

        /// <inheritdoc/>
        public override ScreenDensityBucket DensityBucket
        {
            get { return densityBucket; }
        }

        // State values.
        private Single densityX;
        private Single densityY;
        private Single densityScale;
        private ScreenDensityBucket densityBucket;
    }
}