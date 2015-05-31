using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.VScrollBar.xml")]
    public class VScrollBar : ScrollBarBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public VScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineUpButton.
        /// </summary>
        private void HandleClickLineUp(DependencyObject element, ref RoutedEventData data)
        {
            DecreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineDownButton.
        /// </summary>
        private void HandleClickLineDown(DependencyObject element, ref RoutedEventData data)
        {
            IncreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallIncrement);
        }
    }
}
