using System;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SoftwareInputService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="SoftwareInputService"/> that was created.</returns>
    public delegate SoftwareInputService SoftwareInputServiceFactory();

    /// <summary>
    /// Contains methods for interacting with software-based input 
    /// methods, such as software keyboards, on platforms which support them.
    /// </summary>
    public abstract class SoftwareInputService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SoftwareInputService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SoftwareInputService"/> that was created.</returns>
        public static SoftwareInputService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SoftwareInputServiceFactory>()();
        }

        /// <summary>
        /// Displays the software keyboard, if one is available.
        /// </summary>
        /// <returns><c>true</c> if the software keyboard was displayed; otherwise, <c>false</c>.</returns>
        public abstract Boolean ShowSoftwareKeyboard();

        /// <summary>
        /// Hides the software keyboard.
        /// </summary>
        /// <returns><c>true</c> if the software keyboard was hidden; otherwise, <c>false</c>.</returns>
        public abstract Boolean HideSoftwareKeyboard();
    }
}
