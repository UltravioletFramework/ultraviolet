using System;
using Android.App;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="AndroidActivityService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="AndroidActivityService"/> that was created.</returns>
    [CLSCompliant(false)]
    public delegate AndroidActivityService AndroidActivityServiceFactory();

    /// <summary>
    /// Represents a service which retrieves a reference to the main Activity.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class AndroidActivityService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AndroidActivityService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="AndroidActivityService"/> that was created.</returns>
        public static AndroidActivityService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<AndroidActivityServiceFactory>()();
        }

        /// <summary>
        /// Gets the main Android activity.
        /// </summary>
        public abstract Activity Activity { get; }
    }
}