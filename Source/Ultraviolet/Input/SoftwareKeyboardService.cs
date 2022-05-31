using System;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SoftwareKeyboardService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="SoftwareKeyboardService"/> that was created.</returns>
    public delegate SoftwareKeyboardService SoftwareKeyboardServiceFactory();

    /// <summary>
    /// Represents a service which controls the software keyboard, if one is available on the current platform.
    /// </summary>
    public abstract class SoftwareKeyboardService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SoftwareKeyboardService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SoftwareKeyboardService"/> that was created.</returns>
        public static SoftwareKeyboardService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SoftwareKeyboardServiceFactory>()();
        }

        /// <summary>
        /// Shows the software keyboard, if one is available.
        /// </summary>
        /// <param name="mode">The display mode of the software keyboard.</param>
        /// <returns><see langword="true"/> if the software keyboard was shown; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean ShowSoftwareKeyboard(KeyboardMode mode);

        /// <summary>
        /// Hides the software keyboard.
        /// </summary>
        /// <returns><see langword="true"/> if the software keyboard was hidden; otherwise, false.</returns>
        public abstract Boolean HideSoftwareKeyboard();

        /// <summary>
        /// Gets or sets the region on the screen at which text is currently being entered.
        /// </summary>
        public abstract Rectangle? TextInputRegion { get; set; }

        /// <summary>
        /// Informs the Ultraviolet context that the software keyboard is being shown by publishing
        /// a <see cref="UltravioletMessages.SoftwareKeyboardShown"/> message.
        /// </summary>
        protected void OnShowingSoftwareKeyboard()
        {
            var uv = UltravioletContext.DemandCurrent();
            uv.Messages.Publish(UltravioletMessages.SoftwareKeyboardShown, null);
        }

        /// <summary>
        /// Informs the Ultraviolet context that the software keyboard is being hidden by publishing
        /// a <see cref="UltravioletMessages.SoftwareKeyboardHidden"/> message.
        /// </summary>
        protected void OnHidingSoftwareKeyboard()
        {
            var uv = UltravioletContext.DemandCurrent();
            uv.Messages.Publish(UltravioletMessages.SoftwareKeyboardHidden, null);
        }
    }
}
