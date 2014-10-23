using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a collection of <see cref="EffectParameter"/> instances.
    /// </summary>
    public abstract class EffectParameterCollection : UltravioletNamedCollection<EffectParameter>, IEnumerable<EffectParameter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">The set of parameters to add to the collection.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected EffectParameterCollection(IEnumerable<EffectParameter> parameters)
        {
            Contract.Require(parameters, "parameters");

            foreach (var parameter in parameters)
            {
                AddInternal(parameter);
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public new List<EffectParameter>.Enumerator GetEnumerator()
        {
            return ((UltravioletCollection<EffectParameter>)this).GetEnumerator();
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
        IEnumerator<EffectParameter> IEnumerable<EffectParameter>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected sealed override String GetName(EffectParameter item)
        {
            return item.Name;
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        protected sealed override void ClearInternal()
        {
            base.ClearInternal();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        protected sealed override void AddInternal(EffectParameter item)
        {
            base.AddInternal(item);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><c>true</c> if the item was removed from the collection; otherwise, <c>false</c>.</returns>
        protected sealed override Boolean RemoveInternal(EffectParameter item)
        {
            return base.RemoveInternal(item);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
        protected sealed override Boolean ContainsInternal(EffectParameter item)
        {
            return base.ContainsInternal(item);
        }
    }
}
