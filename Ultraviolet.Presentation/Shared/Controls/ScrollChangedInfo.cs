using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a change in the state of a <see cref="ScrollViewer"/> instance's scrolling properties.
    /// </summary>
    public struct ScrollChangedInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollChangedInfo"/> structure.
        /// </summary>
        internal ScrollChangedInfo(
            Double horizontalOffset, Double horizontalChange,
            Double verticalOffset, Double verticalChange,
            Double extentWidth, Double extentWidthChange,
            Double extentHeight, Double extentHeightChange,
            Double viewportWidth, Double viewportWidthChange,
            Double viewportHeight, Double viewportHeightChange)
        {
            this.horizontalOffset = horizontalOffset;
            this.horizontalChange = horizontalChange;
            this.verticalOffset = verticalOffset;
            this.verticalChange = verticalChange;
            this.extentWidth = extentWidth;
            this.extentWidthChange = extentWidthChange;
            this.extentHeight = extentHeight;
            this.extentHeightChange = extentHeightChange;
            this.viewportWidth = viewportWidth;
            this.viewportWidthChange = viewportWidthChange;
            this.viewportHeight = viewportHeight;
            this.viewportHeightChange = viewportHeightChange;
        }

        /// <summary>
        /// Gets the horizontal offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double HorizontalOffset { get { return horizontalOffset; } }

        /// <summary>
        /// Gets the change in the horizontal offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double HorizontalChange { get { return horizontalChange; } }

        /// <summary>
        /// Gets the vertical offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double VerticalOffset { get { return verticalOffset; } }

        /// <summary>
        /// Gets the change in the vertical offset of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double VerticalChange { get { return verticalChange; } }

        /// <summary>
        /// Gets the width of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentWidth { get { return extentWidth; } }

        /// <summary>
        /// Gets the change in width of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentWidthChange { get { return extentWidthChange; } }

        /// <summary>
        /// Gets the height of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentHeight { get { return extentHeight; } }

        /// <summary>
        /// Gets the change in height of the <see cref="ScrollViewer"/> control.
        /// </summary>
        public Double ExtentHeightChange { get { return extentHeightChange; } }

        /// <summary>
        /// Gets the width of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportWidth { get { return viewportWidth; } }

        /// <summary>
        /// Gets the change in width of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportWidthChange { get { return viewportWidthChange; } }

        /// <summary>
        /// Gets the height of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportHeight { get { return viewportHeight; } }

        /// <summary>
        /// Gets the change in height of the <see cref="ScrollViewer"/> control's viewport.
        /// </summary>
        public Double ViewportHeightChange { get { return viewportHeightChange; } }

        // Property values.
        private readonly Double horizontalOffset;
        private readonly Double horizontalChange;
        private readonly Double verticalOffset;
        private readonly Double verticalChange;
        private readonly Double extentWidth;
        private readonly Double extentWidthChange;
        private readonly Double extentHeight;
        private readonly Double extentHeightChange;
        private readonly Double viewportWidth;
        private readonly Double viewportWidthChange;
        private readonly Double viewportHeight;
        private readonly Double viewportHeightChange;
    }
}
