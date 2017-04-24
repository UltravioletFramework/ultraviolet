using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which can be indexed.
    /// </summary>
    public interface IIndexable
    {
        /// <summary>
        /// Gets the object at the specified index within this collection.
        /// </summary>
        /// <param name="index">The index of the object to retrieve.</param>
        /// <returns>The object at the specified index within the collection.</returns>
        Object this[Int32 index] { get; }

        /// <summary>
        /// Gets the number of indexable items.
        /// </summary>
        Int32 Count { get; }
    }
}
