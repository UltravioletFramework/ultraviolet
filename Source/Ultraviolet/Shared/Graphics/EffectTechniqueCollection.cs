using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect's collection of techniques.
    /// </summary>
    public abstract class EffectTechniqueCollection : UltravioletNamedCollection<EffectTechnique>, IEnumerable<EffectTechnique>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public new List<EffectTechnique>.Enumerator GetEnumerator()
        {
            return ((UltravioletCollection<EffectTechnique>)this).GetEnumerator();
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
        IEnumerator<EffectTechnique> IEnumerable<EffectTechnique>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected override String GetName(EffectTechnique item)
        {
            return item.Name;
        }
    }
}
