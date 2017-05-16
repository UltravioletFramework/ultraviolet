using System;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="ClipboardService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="ClipboardService"/> that was created.</returns>
    public delegate ClipboardService ClipboardServiceFactory();

    /// <summary>
    /// Contains methods for interacting with the system clipboard.
    /// </summary>
    public abstract class ClipboardService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ClipboardService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="ClipboardService"/> that was created.</returns>
        public static ClipboardService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<ClipboardServiceFactory>()();
        }

        /// <summary>
        /// Gets or sets the clipboard text.
        /// </summary>
        public abstract String Text
        {
            get;
            set;
        }
    }
}
