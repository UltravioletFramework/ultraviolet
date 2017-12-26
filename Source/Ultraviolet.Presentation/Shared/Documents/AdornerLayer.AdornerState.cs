using System;

namespace Ultraviolet.Presentation.Documents
{
    partial class AdornerLayer
    {
        /// <summary>
        /// Contains state data associated with one of the layer's adorners.
        /// </summary>
        private class AdornerState
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AdornerState"/> class.
            /// </summary>
            /// <param name="adorner">The <see cref="Adorner"/> that this state instance represents.</param>
            public AdornerState(Adorner adorner)
            {
                this.adorner = adorner;
            }

            /// <summary>
            /// Gets the <see cref="Adorner"/> that this state instance represents.
            /// </summary>
            public Adorner Adorner
            {
                get { return adorner; }
            }

            /// <summary>
            /// Gets or sets the absolute x-coordinate of the adorned element as of the last time the adorner was updated.
            /// </summary>
            public Double LastAbsoluteX
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the absolute y-coordinate of the adorned element as of the last time the adorner was updated.
            /// </summary>
            public Double LastAbsoluteY
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the render width of the adorned element as of the last time the adorner was updated.
            /// </summary>
            public Double LastRenderWidth
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the render height of the adorned element as of the last time the adorner was updated.
            /// </summary>
            public Double LastRenderHeight
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the last transform matrix that was applied to the adorner.
            /// </summary>
            public Matrix LastTransform
            {
                get;
                set;
            }

            // Property values.
            private readonly Adorner adorner;
        }
    }
}
