using System;
using Android.App;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents the implementation of the <see cref="AndroidActivityService"/> class.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class AndroidActivityServiceImpl : AndroidActivityService
    {
        /// <inheritdoc/>
        public override Activity Activity => UltravioletActivity.Instance;
    }
}