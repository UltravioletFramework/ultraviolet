
namespace Ultraviolet.Presentation
{
    partial class DrawingContext
    {
        /// <summary>
        /// Represents a clipping rectangle that was pushed onto the drawing context's render stack.
        /// </summary>
        private struct ClipState
        {
            /// <summary> 
            /// Initializes a new instance of the <see cref="ClipState"/> structure.
            /// </summary>
            /// <param name="clipRectangle">The local clipping rectangle.</param>
            /// <param name="cumulativeClipRectangle">The cumulative clipping rectangle.</param>
            public ClipState(RectangleD clipRectangle, RectangleD cumulativeClipRectangle)
            {
                this.clipRectangle           = clipRectangle;
                this.cumulativeClipRectangle = cumulativeClipRectangle;
            }

            /// <summary>
            /// Gets the local clipping rectangle.
            /// </summary>
            public RectangleD ClipRectangle { get { return clipRectangle; } }

            /// <summary>
            /// Gets the cumulative clipping rectangle.
            /// </summary>
            public RectangleD CumulativeClipRectangle { get { return cumulativeClipRectangle; } }

            // Property values.
            private readonly RectangleD clipRectangle;
            private readonly RectangleD cumulativeClipRectangle;
        }
    }
}
