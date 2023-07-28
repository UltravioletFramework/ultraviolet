using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the queue that manages the Presentation Foundation's list of active popup windows.
    /// </summary>
    internal partial class PopupQueue
    {
        /// <summary>
        /// Gets a value indicating whether the queue is currently drawing the specified popup.
        /// </summary>
        /// <param name="popup">The popup to evaluate.</param>
        /// <returns><see langword="true"/> if the queue is currently drawing the specified popup; otherwise, <see langword="false"/>.</returns>
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

            while (true)
            {
                position = (position == null) ? queue.First : position.Next;
                next     = position.Next;

                var popup = position.Value;

                dc.Reset(popup.View.Display);

                popup.EnsureIsLoaded(true);
                popup.Draw(time, dc);

                if (position.Next == null)
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
            Contract.Require(popup, nameof(popup));

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
        /// or <see langword="null"/> if none of the items in the queue contain the point.</returns>
        public Visual HitTest(Point2D point, out Popup popup)
        {
            if (queue.Count == 0)
            {
                popup = null;
                return null;
            }

            for (var current = queue.Last; current != null; current = current.Previous)
            {
                if (current.Value.IsHitTestVisible)
                {
                    var match = current.Value.PopupHitTest(point);
                    if (match != null)
                    {
                        popup = current.Value;
                        return match;
                    }
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
