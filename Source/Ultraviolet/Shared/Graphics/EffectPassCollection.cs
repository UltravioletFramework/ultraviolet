using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect technique's collection of passes.
    /// </summary>
    public abstract class EffectPassCollection : UltravioletNamedCollection<EffectPass>, IEnumerable<EffectPass>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public new List<EffectPass>.Enumerator GetEnumerator()
        {
            return ((UltravioletCollection<EffectPass>)this).GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<EffectPass> IEnumerable<EffectPass>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected override String GetName(EffectPass item)
        {
            return item.Name;
        }
    }
}
