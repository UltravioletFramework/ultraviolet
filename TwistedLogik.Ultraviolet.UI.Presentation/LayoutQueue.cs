using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a queue of elements with invalidated layouts.
    /// </summary>
    internal partial class LayoutQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutQueue"/> class.
        /// </summary>
        /// <param name="invalidate">An action which invalidates the element state associated with this queue.</param>
        /// <param name="bubble">A value indicating whether to bubble invalidation up through the visual tree.</param>
        public LayoutQueue(Action<UIElement> invalidate, Boolean bubble = true)
        {
            Contract.Require(invalidate, "invalidate");

            this.invalidate = invalidate;
            this.bubble     = bubble;
        }

        /// <summary>
        /// Adds an element to the queue.
        /// </summary>
        /// <param name="element">The element to add to the queue.</param>
        public void Enqueue(UIElement element)
        {
            Contract.Require(element, "element");

            var current = element;
            var parent  = element;

            while (current != null)
            {
                invalidate(current);

                parent = VisualTreeHelper.GetParent(current) as UIElement;
                if (parent == null)
                {
                    var entry = new Entry(current.LayoutDepth, current);
                    if (queue.ContainsKey(entry))
                        return;

                    queue.Add(entry, current);
                }
                else
                {
                    var entry = new Entry(current.LayoutDepth, current);
                    queue.Remove(entry);
                }

                if (!bubble)
                    break;

                current = parent;
            }
        }

        /// <summary>
        /// Removes the specified element from the queue.
        /// </summary>
        /// <param name="element">The element to remove from the queue.</param>
        public void Remove(UIElement element)
        {
            Contract.Require(element, "element");

            var entry = new Entry(element.LayoutDepth, element);
            queue.Remove(entry);
        }

        /// <summary>
        /// Removes an element from the end of the queue.
        /// </summary>
        /// <returns>The element that was removed from the queue.</returns>
        public UIElement Dequeue()
        {
            var element = queue.Values[0];
            queue.RemoveAt(0);

            return element;
        }

        /// <summary>
        /// Retrieves the element at the end of the queue, but does not remove it.
        /// </summary>
        /// <returns>The element at the end of the queue.</returns>
        public UIElement Peek()
        {
            return queue.Values[0];
        }

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public Int32 Count
        {
            get { return queue.Count; }
        }

        /* An action which invalidates any elements along the path between
        /* an element which is added to the queue and its visual parent. */
        private readonly Action<UIElement> invalidate;
        private readonly Boolean bubble;

        // The sorted list which represents our queue's storage.
        private readonly SortedList<Entry, UIElement> queue = 
            new SortedList<Entry, UIElement>(32, new EntryComparer());
    }
}
