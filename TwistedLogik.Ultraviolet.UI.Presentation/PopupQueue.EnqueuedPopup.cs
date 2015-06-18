using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class PopupQueue
    {
        /// <summary>
        /// Represents a popup which has been queued for rendering.
        /// </summary>
        private struct EnqueuedPopup
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnqueuedPopup"/> structure.
            /// </summary>
            /// <param name="popup">The popup to enqueue for rendering.</param>
            /// <param name="transform">The popup's associated transformation matrix.</param>
            public EnqueuedPopup(Popup popup, Matrix? transform)
            {
                this.popup = popup;
                this.transform = transform;
            }

            /// <summary>
            /// Gets the enqueued <see cref="Popup"/> object.
            /// </summary>
            public Popup Popup
            {
                get { return popup; }
            }

            /// <summary>
            /// Gets the enqueued popup's associated transformation matrix.
            /// </summary>
            public Matrix? Transform
            {
                get { return transform; }
            }

            // Property values.
            private readonly Popup popup;
            private readonly Matrix? transform;
        }
    }
}
