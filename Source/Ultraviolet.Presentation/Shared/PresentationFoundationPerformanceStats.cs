using System;
using System.Diagnostics;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents various statistics relating to performance which are collected by the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationPerformanceStats
    {
        /// <summary>
        /// Starts the timer measuring draw performance.
        /// </summary>
        public void BeginDraw()
        {
            if (++countDraw > 1)
                return;

            swDraw.Reset();
            swDraw.Start();
        }

        /// <summary>
        /// Stops the timer measuring draw performance and adds its recorded time to the total for this frame.
        /// </summary>
        public void EndDraw()
        {
            if (--countDraw < 0)
                throw new InvalidOperationException();

            swDraw.Stop();

            ticksInDraw += swDraw.ElapsedTicks;
        }

        /// <summary>
        /// Starts the timer measuring update performance.
        /// </summary>
        public void BeginUpdate()
        {
            if (++countUpdate > 1)
                return;

            swUpdate.Reset();
            swUpdate.Start();
        }

        /// <summary>
        /// Stops the timer measuring update performance and adds its recorded time to the total for this frame.
        /// </summary>
        public void EndUpdate()
        {
            if (--countUpdate < 0)
                throw new InvalidOperationException();

            swUpdate.Stop();

            ticksInUpdate += swUpdate.ElapsedTicks;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateStyle(Boolean)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 InvalidateStyleCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateStyle(Boolean)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 InvalidateStyleCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateMeasure()"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 InvalidateMeasureCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateMeasure()"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 InvalidateMeasureCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateArrange(bool)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 InvalidateArrangeCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.InvalidateArrange(bool)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 InvalidateArrangeCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Style(Styles.UvssDocument)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 StyleCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Style(Styles.UvssDocument)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 StyleCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Measure(Size2D)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 MeasureCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Measure(Size2D)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 MeasureCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Arrange(RectangleD, ArrangeOptions)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 ArrangeCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Arrange(RectangleD, ArrangeOptions)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 ArrangeCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Position(Size2D)"/> calls that have been made in the current frame.
        /// </summary>
        public Int32 PositionCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of <see cref="UIElement.Position(Size2D)"/> calls that were made in the previous frame.
        /// </summary>
        public Int32 PositionCountLastFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount of time that has been spent updating the Presentation Foundation during the current frame.
        /// </summary>
        public TimeSpan TimeInUpdate
        {
            get { return TimeSpan.FromTicks(ticksInUpdate); }
        }

        /// <summary>
        /// Gets the amount of time that has been spent updating the Presentation Foundation during the current frame.
        /// </summary>
        public TimeSpan TimeInDraw
        {
            get { return TimeSpan.FromTicks(ticksInDraw); }
        }

        /// <summary>
        /// Gets the amount of time that was spent updating the Presentation Foundation during the previous frame.
        /// </summary>
        public TimeSpan TimeInUpdateLastFrame
        {
            get { return TimeSpan.FromTicks(ticksInUpdateLastFrame); }
        }

        /// <summary>
        /// Gets the amount of time that was spent updating the Presentation Foundation during the previous frame.
        /// </summary>
        public TimeSpan TimeInDrawLastFrame
        {
            get { return TimeSpan.FromTicks(ticksInDrawLastFrame); }
        }

        /// <summary>
        /// Indicates that a new frame has begun.
        /// </summary>
        internal void OnFrameStart()
        {
            InvalidateStyleCountLastFrame = InvalidateStyleCount;
            InvalidateStyleCount = 0;

            InvalidateMeasureCountLastFrame = InvalidateMeasureCount;
            InvalidateMeasureCount = 0;

            InvalidateArrangeCountLastFrame = InvalidateArrangeCount;
            InvalidateArrangeCount = 0;

            StyleCountLastFrame = StyleCount;
            StyleCount = 0;

            MeasureCountLastFrame = MeasureCount;
            MeasureCount = 0;

            ArrangeCountLastFrame = ArrangeCount;
            ArrangeCount = 0;

            PositionCountLastFrame = PositionCount;
            PositionCount = 0;

            ticksInUpdateLastFrame = ticksInUpdate;
            ticksInDrawLastFrame   = ticksInDraw;

            ticksInUpdate = 0;
            ticksInDraw   = 0;
        }

        // Property values.
        private Int64 ticksInUpdate;
        private Int64 ticksInUpdateLastFrame;
        private Int64 ticksInDraw;
        private Int64 ticksInDrawLastFrame;

        // Stopwatches used for timing.
        private readonly Stopwatch swDraw = new Stopwatch();
        private readonly Stopwatch swUpdate = new Stopwatch();
        private Int32 countDraw;
        private Int32 countUpdate;
    }
}
