using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of triggers which are defined by a style rule.
    /// </summary>
    public sealed partial class UvssTriggerCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTriggerCollection"/> class.
        /// </summary>
        /// <param name="triggers">A collection containing the triggers to add to the list.</param>
        internal UvssTriggerCollection(IEnumerable<Trigger> triggers)
        {
            if (triggers != null)
            {
                this.triggers.AddRange(triggers);
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        internal void Clear()
        {
            triggers.Clear();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="trigger">The trigger to add to the collection.</param>
        internal void Add(Trigger trigger)
        {
            triggers.Add(trigger);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="trigger">The trigger to remove from the collection.</param>
        /// <returns><c>true</c> if the item was removed from the collection; otherwise, <c>false</c>.</returns>
        internal Boolean Remove(Trigger trigger)
        {
            return triggers.Remove(trigger);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="trigger">The trigger to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
        internal Boolean Contains(Trigger trigger)
        {
            return triggers.Contains(trigger);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return triggers.Count; }
        }

        // State values.
        private readonly List<Trigger> triggers = 
            new List<Trigger>();
    }
}
