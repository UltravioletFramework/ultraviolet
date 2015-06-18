using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the queue that manages the Presentation Foundation's list of active popup windows.
    /// </summary>
    internal partial class PopupQueue
    {
        /// <summary>
        /// Gets the transformation matrix associated with the popup that is currently being drawn.
        /// </summary>
        /// <returns>The transformation matrix associated with the popup that is currently being drawn.</returns>
        public Matrix? GetCurrentTransformMatrix()
        {
            if (position == null)
                return null;

            return position.Value.Transform;
        }

        /// <summary>
        /// Gets a value indicating whether the queue is currently drawing the specified popup.
        /// </summary>
        /// <param name="popup">The popup to evaluate.</param>
        /// <returns><c>true</c> if the queue is currently drawing the specified popup; otherwise, <c>false</c>.</returns>
        public Boolean IsDrawingPopup(Popup popup)
        {
            return position != null && position.Value.Popup == popup;
        }

        /// <summary>
        /// Gets a value indicating whether the popup which is being drawn has a non-identity transformation matrix.
        /// </summary>
        /// <returns><c>true</c> if the current popup is being transformed; otherwise, <c>false</c>.</returns>
        public Boolean IsTransformed()
        {
            return position != null && position.Value.Transform.HasValue;
        }

        /// <summary>
        /// Draws the contents of the queue.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        public void Draw(UltravioletTime time, DrawingContext dc)
        {
            if (queue.Count == 0)
                return;

            position = null;
            next     = null;

            while (true)
            {
                position = (position == null) ? queue.First : position.Next;
                next     = position.Next;

                var popup = position.Value.Popup;

                dc.Reset(popup.View.Display);

                popup.EnsureIsLoaded(true);
                popup.Draw(time, dc);

                if (next == null)
                    break;
            }

            position = null;
            next     = null;
        }

        /// <summary>
        /// Clears the queue of its contents.
        /// </summary>
        public void Clear()
        {
            queue.Clear();
        }

        /// <summary>
        /// Enqueues a popup at the next position within the queue.
        /// </summary>
        /// <param name="popup">The popup to enqueue.</param>
        /// <param name="transform">The popup's transformation matrix.</param>
        public void Enqueue(Popup popup, Matrix? transform)
        {
            Contract.Require(popup, "popup");

            if (next == null)
            {
                queue.AddLast(new EnqueuedPopup(popup, transform));
            }
            else
            {
                queue.AddBefore(next, new EnqueuedPopup(popup, transform));
            }
        }

        /// <summary>
        /// Performs a hit test against the contents of the queue.
        /// </summary>
        /// <param name="point">A point in device-independent screen space to test against the contents of the queue.</param>
        /// <param name="popup">The popup that contains the resulting visual.</param>
        /// <returns>The topmost <see cref="Visual"/> that contains the specified point, 
        /// or <c>null</c> if none of the items in the queue contain the point.</returns>
        public Visual HitTest(Point2D point, out Popup popup)
        {
            if (queue.Count == 0)
            {
                popup = null;
                return null;
            }

            for (var current = queue.Last; current != null; current = current.Previous)
            {
                var match = current.Value.Popup.PopupHitTest(point);
                if (match != null)
                {
                    popup = current.Value.Popup;
                    return match;
                }
            }

            popup = null;
            return null;
        }

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public Int32 Count
        {
            get { return queue.Count; }
        }

        // State values.
        private readonly PooledLinkedList<EnqueuedPopup> queue = new PooledLinkedList<EnqueuedPopup>(8);
        private LinkedListNode<EnqueuedPopup> position;
        private LinkedListNode<EnqueuedPopup> next;
    }
}
