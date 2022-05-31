using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets the items for an object which is itself a collection.
    /// </summary>
    internal class UvmlCollectionItemMutator : UvmlCollectionItemMutatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlCollectionItemMutator"/> class.
        /// </summary>
        /// <param name="propertyItems">The collection of items to set on the property.</param>
        public UvmlCollectionItemMutator(IEnumerable<UvmlNode> propertyItems)
            : base(propertyItems)
        {

        }

        /// <inheritdoc/>
        protected override Boolean CreateCollection(UltravioletContext uv, Object instance, UvmlInstantiationContext context, out Object collection)
        {
            collection = null;
            return false;
        }

        /// <inheritdoc/>
        protected override Boolean GetCollection(Object instance, out Object collection, out String propname)
        {
            propname = null;
            collection = instance;
            return true;
        }

        /// <inheritdoc/>
        protected override Boolean SetCollection(Object instance, Object collection)
        {
            return false;
        }
    }
}
