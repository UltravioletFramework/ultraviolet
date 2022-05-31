using System;
using System.Collections.Generic;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets the items in a collection 
    /// which is represented by a standard property.
    /// </summary>
    internal sealed class UvmlStandardPropertyCollectionItemMutator : UvmlCollectionItemMutatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlStandardPropertyCollectionItemMutator"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property which is being mutated.</param>
        /// <param name="propertyItems">The collection of items to set on the property.</param>
        public UvmlStandardPropertyCollectionItemMutator(PropertyInfo propertyInfo, IEnumerable<UvmlNode> propertyItems)
            : base(propertyItems)
        {
            Contract.Require(propertyInfo, nameof(propertyInfo));

            this.propertyInfo = propertyInfo;
        }

        /// <inheritdoc/>
        protected override Boolean CreateCollection(UltravioletContext uv, Object instance, UvmlInstantiationContext context, out Object collection)
        {
            return CreateCollectionOfType(uv, instance, context, propertyInfo.PropertyType, out collection);
        }

        /// <inheritdoc/>
        protected override Boolean GetCollection(Object instance, out Object collection, out String propname)
        {
            propname = propertyInfo.Name;

            if (!propertyInfo.CanRead)
            {
                collection = null;
                return false;
            }

            collection = propertyInfo.GetValue(instance, null);
            return true;
        }

        /// <inheritdoc/>
        protected override Boolean SetCollection(Object instance, Object collection)
        {
            if (!propertyInfo.CanWrite)
                return false;

            propertyInfo.SetValue(instance, collection, null);
            return true;
        }

        // State values.
        private readonly PropertyInfo propertyInfo;
    }
}
