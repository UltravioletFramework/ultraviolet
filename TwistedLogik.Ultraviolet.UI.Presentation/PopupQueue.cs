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
    internal class PopupQueue
    {
        /// <summary>
        /// Gets a value indicating whether the queue is currently drawing the specified popup.
        /// </summary>
        /// <param name="popup">The popup to evaluate.</param>
        /// <returns><c>true</c> if the queue is currently drawing the specified popup; otherwise, <c>false</c>.</returns>
        public Boolean IsDrawingPopup(Popup popup)
        {
            return position != null && position.Value == popup;
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

                dc.Reset();

                position.Value.EnsureIsLoaded(true);
                position.Value.Draw(time, dc);

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
        public void Enqueue(Popup popup)
        {
            Contract.Require(popup, "popup");

            if (next == null)
            {
                queue.AddLast(popup);
            }
            else
            {
                queue.AddBefore(next, popup);
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
                var match = current.Value.HitTest(point);
                if (match != null)
                {
                    popup = current.Value;
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
        private readonly PooledLinkedList<Popup> queue = new PooledLinkedList<Popup>(8);
        private LinkedListNode<Popup> position;
        private LinkedListNode<Popup> next;
    }
}
