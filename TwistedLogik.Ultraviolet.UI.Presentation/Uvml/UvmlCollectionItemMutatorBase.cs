using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets the items in a collection.
    /// </summary>
    internal abstract class UvmlCollectionItemMutatorBase : UvmlMutator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlCollectionItemMutatorBase"/> class.
        /// </summary>
        /// <param name="items">The collection of items to add to the mutated collection.</param>
        protected UvmlCollectionItemMutatorBase(IEnumerable<UvmlNode> items)
        {
            Contract.Require(items, nameof(items));

            this.items = new List<UvmlNode>(items);
        }

        /// <inheritdoc/>
        public override Object InstantiateValue(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            return items.Select(x => x.Instantiate(uv, context)).ToList();
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var value = InstantiateValue(uv, instance, context);
            Mutate(uv, instance, value, context);
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, Object value, UvmlInstantiationContext context)
        {
            var items = ProcessPrecomputedValue<List<Object>>(value, context);
            if (items == null)
                throw new ArgumentException(nameof(value));

            var propname = default(String);
            var collection = default(Object);
            if (!GetCollection(instance, out collection, out propname))
                throw new UvmlException(PresentationStrings.PropertyHasNoGetter.Format(propname));

            if (collection == null)
            {
                if (!CreateCollection(uv, instance, context, out collection))
                    throw new UvmlException(PresentationStrings.CollectionCannotBeCreated.Format(propname));

                if (!SetCollection(instance, collection))
                    throw new UvmlException(PresentationStrings.PropertyHasNoSetter.Format(propname));
            }

            var listType = collection.GetType();
            var listItemType = GetCollectionItemType(listType);
            if (listItemType == null)
                throw new UvmlException(PresentationStrings.CollectionTypeNotSupported.Format(listType));

            var listClearMethod = listType.GetMethod("Clear",
                BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (listClearMethod == null)
                throw new UvmlException(PresentationStrings.CollectionCannotBeCleared.Format(listType.Name));

            listClearMethod.Invoke(collection, null);

            var listAddRangeMethod = listType.GetMethod("AddRange",
                BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(IEnumerable<>).MakeGenericType(listItemType) }, null);
            if (listAddRangeMethod != null)
            {
                var itemInstances = Array.CreateInstance(listItemType, items.Count);
                for (int i = 0; i < items.Count; i++)
                {
                    var item = ProcessPrecomputedValue<Object>(items[i], context);
                    itemInstances.SetValue(item, i);
                }
                listAddRangeMethod.Invoke(collection, new[] { itemInstances });
            }
            else
            {
                var listAddMethodParameters = new Object[] { null };
                var listAddMethod = listType.GetMethod("Add",
                    BindingFlags.Public | BindingFlags.Instance, null, new[] { listItemType }, null);
                if (listAddMethod != null)
                {
                    foreach (var item in items)
                    {
                        listAddMethodParameters[0] = ProcessPrecomputedValue<Object>(item, context);
                        listAddMethod.Invoke(collection, listAddMethodParameters);
                    }
                }
                else
                {
                    throw new UvmlException(PresentationStrings.CollectionHasNoAddMethod.Format(listType.Name));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is a collection type
        /// which is supported by collection mutators.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <param name="itemType">The type of item contained by the collection, if it is a collection.</param>
        /// <returns><see langword="true"/> if the type is a supported collection type;
        /// otherwise, <see langword="false"/>.</returns>
        internal static Boolean IsSupportedCollectionType(Type type, out Type itemType)
        {
            itemType = GetCollectionItemType(type);
            return itemType != null;
        }

        /// <summary>
        /// Creates a new collection instance for the mutated property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance which is being mutated.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <param name="collection">The collection which was created.</param>
        /// <returns><see langword="true"/> if the collection was able to be created;
        /// otherwise, <see langword="false"/>.</returns>
        protected abstract Boolean CreateCollection(UltravioletContext uv,
            Object instance, UvmlInstantiationContext context, out Object collection);

        /// <summary>
        /// Gets the collection object which is being mutated.
        /// </summary>
        /// <param name="instance">The object instance which is being mutated.</param>
        /// <param name="collection">The collection object which is being mutated.</param>
        /// <param name="propname">The name of the property from which the collection is being retrieved.</param>
        /// <returns><see langword="true"/> if the collection was able to be retrieved;
        /// otherwise, <see langword="false"/>.</returns>
        protected abstract Boolean GetCollection(Object instance, out Object collection, out String propname);

        /// <summary>
        /// Sets the collection object which is being mutated.
        /// </summary>
        /// <param name="instance">The object instance which is being mutated.</param>
        /// <param name="collection">The collection object which is being mutated.</param>
        /// <returns><see langword="true"/> if the collection was able to be set;
        /// otherwise, <see langword="false"/>.</returns>
        protected abstract Boolean SetCollection(Object instance, Object collection);

        /// <summary>
        /// Creates a new collection instance of the specified type.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance which is being mutated.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <param name="type">The type of collection to create.</param>
        /// <param name="collection">The collection which was created.</param>
        /// <returns><see langword="true"/> if the collection was able to be created;
        /// otherwise, <see langword="false"/>.</returns>
        protected Boolean CreateCollectionOfType(UltravioletContext uv,
            Object instance, UvmlInstantiationContext context, Type type, out Object collection)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                collection = null;
                return false;
            }

            collection = ctor.Invoke(null);
            return true;
        }

        /// <summary>
        /// Gets the item type of the specified collection type.
        /// </summary>
        private static Type GetCollectionItemType(Type type)
        {
            var ifaces = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (ifaces.Count() > 1 || !ifaces.Any())
                return null;

            return ifaces.Single().GetGenericArguments()[0];
        }

        // State values.
        private readonly List<UvmlNode> items;
    }
}
