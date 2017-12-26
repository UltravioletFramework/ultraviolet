using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a collection of grid rows or columns.
    /// </summary>
    internal interface IDefinitionBaseCollection
    {
        /// <summary>
        /// Gets the item at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the definition to retrieve.</param>
        /// <returns>The definition at the specified index within the collection.</returns>
        DefinitionBase this[Int32 ix]
        {
            get;
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        Int32 Count { get; }
    }
}
