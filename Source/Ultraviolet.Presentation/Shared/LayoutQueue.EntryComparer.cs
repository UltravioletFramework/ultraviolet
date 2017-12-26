using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    partial class LayoutQueue
    {
        /// <summary>
        /// Represents a comparer which compares entries in a layout queue.
        /// </summary>
        private class EntryComparer : IComparer<Entry>
        {
            /// <inheritdoc/>
            public Int32 Compare(Entry x, Entry y)
            {
                var priorityComparison = x.Priority.CompareTo(y.Priority);
                if (priorityComparison == 0)
                {
                    return x.PriorityTiebreaker.CompareTo(y.PriorityTiebreaker);
                }
                return priorityComparison;
            }
        }
    }
}
