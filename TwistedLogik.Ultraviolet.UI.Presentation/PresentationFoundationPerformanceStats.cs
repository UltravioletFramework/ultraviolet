using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents various statistics relating to performance which are collected by the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationPerformanceStats
    {
        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateStyle(Boolean)"/> calls made in the last frame.
        /// </summary>
        public Int32 InvalidateStyleCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateMeasure()"/> calls made in the last frame.
        /// </summary>
        public Int32 InvalidateMeasureCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateArrange()"/> calls made in the last frame.
        /// </summary>
        public Int32 InvalidateArrangeCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Style(UvssDocument)"/> calls that were actually handled in the last frame.
        /// </summary>
        public Int32 StyleCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Measure(Size2D)"/> calls that were actually handled in the last frame.
        /// </summary>
        public Int32 MeasureCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Arrange(RectangleD, ArrangeOptions)"/> calls that were actually handled in the last frame.
        /// </summary>
        public Int32 ArrangeCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Position(Size2D)"/> calls that were actually handled in the last frame.
        /// </summary>
        public Int32 PositionCountLastFrame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Indicates that a new frame has begun.
        /// </summary>
        internal void BeginFrame()
        {
            InvalidateStyleCountLastFrame    = 0;
            InvalidateMeasureCountLastFrame  = 0;
            InvalidateArrangeCountLastFrame  = 0;

            StyleCountLastFrame    = 0;
            MeasureCountLastFrame  = 0;
            ArrangeCountLastFrame  = 0;
            PositionCountLastFrame = 0;
        }
    }
}
