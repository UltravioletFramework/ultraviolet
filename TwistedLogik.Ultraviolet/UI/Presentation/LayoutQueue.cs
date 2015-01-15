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
        /// Adds an element to the queue.
        /// </summary>
        /// <param name="element">The element to add to the queue.</param>
        public void Enqueue(UIElement element)
        {
            Contract.Require(element, "element");

            var entry = new Entry(element.LayoutDepth, element);
            if (queue.ContainsKey(entry))
                return;

            queue.Add(entry, element);
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

        // The sorted list which represents our queue's storage.
        private readonly SortedList<Entry, UIElement> queue = 
            new SortedList<Entry, UIElement>(32, new EntryComparer());
    }
}
