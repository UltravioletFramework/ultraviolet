using System;

namespace Ultraviolet.Presentation
{
    partial class DrawingContext
    {
        /// <summary>
        /// Represents an opacity that was pushed onto the drawing context's render stack.
        /// </summary>
        private struct OpacityState
        {
            /// <summary> 
            /// Initializes a new instance of the <see cref="OpacityState"/> structure.
            /// </summary>
            /// <param name="opacity">The local opacity value.</param>
            /// <param name="cumulativeOpacity">The cumulative opacity value.</param>
            public OpacityState(Single opacity, Single cumulativeOpacity)
            {
                this.opacity           = opacity;
                this.cumulativeOpacity = cumulativeOpacity;
            }

            /// <summary>
            /// Gets the local opacity value.
            /// </summary>
            public Single Opacity { get { return opacity; } }

            /// <summary>
            /// Gets the cumulative opacity value.
            /// </summary>
            public Single CumulativeOpacity { get { return cumulativeOpacity; } }

            // Property values.
            private readonly Single opacity;
            private readonly Single cumulativeOpacity;
        }
    }
}
