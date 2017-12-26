using System;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the data for a scroll event.
    /// </summary>
    public class ScrollChangedRoutedEventData : RoutedEventData
    {
        /// <summary>
        /// Retrieves an instance of the <see cref="ScrollChangedRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="ScrollChangedRoutedEventData"/> instance that was retrieved.</returns>
        public static new ScrollChangedRoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            return Retrieve(source, default(ScrollChangedInfo), handled, autorelease);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="ScrollChangedRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="info">A <see cref="ScrollChangedInfo"/> which describes the change.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="ScrollChangedRoutedEventData"/> instance that was retrieved.</returns>
        public static ScrollChangedRoutedEventData Retrieve(Object source, ScrollChangedInfo info, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(ScrollChangedRoutedEventData);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, source, handled, autorelease);
            data.HorizontalOffset = info.HorizontalOffset;
            data.HorizontalChange = info.HorizontalChange;
            data.VerticalOffset = info.VerticalOffset;
            data.VerticalChange = info.VerticalChange;
            data.ExtentWidth = info.ExtentWidth;
            data.ExtentWidthChange = info.ExtentWidthChange;
            data.ExtentHeight = info.ExtentHeight;
            data.ExtentHeightChange = info.ExtentHeightChange;
            data.ViewportWidth = info.ViewportWidth;
            data.ViewportWidthChange = info.ViewportWidthChange;
            data.ViewportHeight = info.ViewportHeight;
            data.ViewportHeightChange = info.ViewportHeightChange;

            return data;
        }

        /// <summary>
        /// Gets the horizontal offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double HorizontalOffset { get; private set; }

        /// <summary>
        /// Gets the change in the horizontal offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double HorizontalChange { get; private set; }

        /// <summary>
        /// Gets the vertical offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double VerticalOffset { get; private set; }

        /// <summary>
        /// Gets the change in the vertical offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double VerticalChange { get; private set; }

        /// <summary>
        /// Gets the width of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentWidth { get; private set; }

        /// <summary>
        /// Gets the change in width of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentWidthChange { get; private set; }

        /// <summary>
        /// Gets the height of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentHeight { get; private set; }

        /// <summary>
        /// Gets the change in height of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentHeightChange { get; private set; }

        /// <summary>
        /// Gets the width of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportWidth { get; private set; }

        /// <summary>
        /// Gets the change in width of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportWidthChange { get; private set; }

        /// <summary>
        /// Gets the height of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportHeight { get; private set; }

        /// <summary>
        /// Gets the change in height of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportHeightChange { get; private set; }

        /// <inheritdoc/>
        protected override void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            HorizontalOffset = 0;
            HorizontalChange = 0;
            VerticalOffset = 0;
            VerticalChange = 0;
            ExtentWidth = 0;
            ExtentWidthChange = 0;
            ExtentHeight = 0;
            ExtentHeightChange = 0;
            ViewportWidth = 0;
            ViewportWidthChange = 0;
            ViewportHeight = 0;
            ViewportHeightChange = 0;

            base.OnRetrieved(origin, source, handled, autorelease);
        }

        /// <inheritdoc/>
        protected override void OnReleased()
        {
            HorizontalOffset = 0;
            HorizontalChange = 0;
            VerticalOffset = 0;
            VerticalChange = 0;
            ExtentWidth = 0;
            ExtentWidthChange = 0;
            ExtentHeight = 0;
            ExtentHeightChange = 0;
            ViewportWidth = 0;
            ViewportWidthChange = 0;
            ViewportHeight = 0;
            ViewportHeightChange = 0;

            base.OnReleased();
        }

        // The global pool of scroll changed data objects.
        private static readonly Pool<ScrollChangedRoutedEventData> pool =
            new ExpandingPool<ScrollChangedRoutedEventData>(1, 4, () => new ScrollChangedRoutedEventData());
    }
}
