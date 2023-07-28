using Ultraviolet.Graphics;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="IconLoader"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="IconLoader"/> that was created.</returns>
    public delegate IconLoader IconLoaderFactory();

    /// <summary>
    /// Contains methods for loading the application icon.
    /// </summary>
    public abstract class IconLoader
    {
        /// <summary>
        /// Creates a new instance of the <see cref="IconLoader"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="IconLoader"/> that was created.</returns>
        public static IconLoader Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<IconLoaderFactory>()();
        }

        /// <summary>
        /// Loads the application's icon.
        /// </summary>
        /// <returns>A <see cref="Surface2D"/> that represents the loaded icon.</returns>
        public abstract Surface2D LoadIcon();
    }
}
