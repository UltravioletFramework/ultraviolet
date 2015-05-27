using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives.Templates.HScrollBar.xml")]
    public class HScrollBar : ScrollBarBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public HScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineLeftButton.
        /// </summary>
        private void HandleClickLineLeft(DependencyObject element, ref RoutedEventData data)
        {
            DecreaseSmall();
            RaiseScrollEvent(ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineRightButton.
        /// </summary>
        private void HandleClickLineRight(DependencyObject element, ref RoutedEventData data)
        {
            IncreaseSmall();
            RaiseScrollEvent(ScrollEventType.LargeDecrement);
        }
    }
}
