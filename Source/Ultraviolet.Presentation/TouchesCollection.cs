using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a collection of touch events.
    /// </summary>
    public sealed partial class TouchesCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchesCollection"/> class.
        /// </summary>
        internal TouchesCollection(UIElement element, DependencyPropertyKey associatedPropertyKey)
        {
            this.element = element;
            this.associatedPropertyKey = associatedPropertyKey;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified touch.
        /// </summary>
        /// <param name="id">The unique identifier of the touch to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified touch;
        /// otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Int64 id) =>
            touches?.Contains(id) ?? false;

        /// <summary>
        /// Gets the number of touches in the collection.
        /// </summary>
        public Int32 Count =>
            touches?.Count ?? 0;

        /// <summary>
        /// Adds the specified touch to the collection.
        /// </summary>
        /// <param name="id">The unique identifier of the touch to add.</param>
        /// <returns><see langword="true"/> if the touch was added; otherwise, <see langword="false"/>.</returns>
        internal Boolean Add(Int64 id)
        {
            if (touches == null)
                touches = new HashSet<Int64>();

            var added = touches.Add(id);
            if (added && touches.Count == 1)
                element.SetValue(associatedPropertyKey, true);

            return added;
        }

        /// <summary>
        /// Removes the specified touch from the collection.
        /// </summary>
        /// <param name="id">The unique identifier of the touch to remove.</param>
        /// <returns><see langword="true"/> if the touch was removed; otherwise, <see langword="false"/>.</returns>
        internal Boolean Remove(Int64 id)
        {
            if (touches == null)
                return false;

            var removed = touches.Remove(id);
            if (removed && touches.Count == 0)
                element.SetValue(associatedPropertyKey, false);

            return removed;
        }

        // State values.
        private readonly UIElement element;
        private readonly DependencyPropertyKey associatedPropertyKey;
        private HashSet<Int64> touches; 
    }
}
