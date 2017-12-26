using System;

namespace Ultraviolet.Presentation
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
        None = 0x00,
        
        /// <summary>
        /// Forces the element to recalculate its position and clipping rectangle,
        /// even if it believes that its position hasn't changed.
        /// </summary>
        ForceInvalidatePosition = 0x01,
    }
}
