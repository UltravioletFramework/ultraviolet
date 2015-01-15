using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the options that can be specified for the <see cref="UIElement.Arrange(RectangleD, ArrangeOptions)"/> method.
    /// </summary>
    [Flags]
    public enum ArrangeOptions
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the element should attempt to fill all available space,
        /// even if it doesn't need that much space.
        /// </summary>
        Fill = 1,
    }
}
