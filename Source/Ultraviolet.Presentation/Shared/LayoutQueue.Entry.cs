using System;

namespace Ultraviolet.Presentation
{
    partial class LayoutQueue
    {
        /// <summary>
        /// Represents an entry in the layout queue.
        /// </summary>
        private struct Entry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> structure.
            /// </summary>
            /// <param name="priority">The entry's priority within the queue.</param>
            /// <param name="element">The element pending a layout operation.</param>
            public Entry(Int32 priority, UIElement element)
            {
                this.priority = priority;
                this.element  = element;
            }

            /// <summary>
            /// Gets the entry's priority within the queue.
            /// </summary>
            public Int32 Priority
            {
                get { return priority; }
            }

            /// <summary>
            /// Gets the value used to break ties when two entries have the same <see cref="Priority"/> value.
            /// </summary>
            public Int32 PriorityTiebreaker
            {
                get { return Element.GetHashCode(); }
            }

            /// <summary>
            /// Gets the element pending a layout operation.
            /// </summary>
            public UIElement Element
            {
                get { return element; }
            }

            // Property values.
            private readonly Int32 priority;
            private readonly UIElement element;
        }
    }
}
