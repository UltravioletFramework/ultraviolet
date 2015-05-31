using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Generates item containers for an instance of the <see cref="ItemsControl"/> class.
    /// </summary>
    public sealed class ItemContainerGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ItemsControl"/> that owns this generator.</param>
        internal ItemContainerGenerator(ItemsControl owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }

        /// <summary>
        /// Returns the item container associated with the specified item.
        /// </summary>
        /// <param name="item">The item for which to retrieve an item container.</param>
        /// <returns>The item container for the specified item, or <c>null</c> if no such item exists.</returns>
        public DependencyObject ContainerFromItem(Object item)
        {
            DependencyObject container;
            itemToContainerMapping.TryGetValue(item, out container);
            return container;
        }

        /// <summary>
        /// Returns the item container at the specified index within the item collection.
        /// </summary>
        /// <param name="index">The index of the item container to retrieve.</param>
        /// <returns>The item container at the specified index, or <c>null</c> if no such item exists.</returns>
        public DependencyObject ContainerFromIndex(Int32 index)
        {
            if (index >= 0 && index < owner.ItemContainers.Count)
            {
                return owner.ItemContainers[index];
            }
            return null;
        }

        /// <summary>
        /// Retrieves the item that is contained by the specified item container.
        /// </summary>
        /// <param name="container">The item container from which to retrieve an item.</param>
        /// <returns>The item that is contained by the specified item container.</returns>
        public Object ItemFromContainer(DependencyObject container)
        {
            Contract.Require(container, "container");

            return container.GetValue<Object>(AssociatedItemProperty);
        }

        /// <summary>
        /// Gets the index of the specified container within the item control which owns it.
        /// </summary>
        /// <param name="container">The item container from which to retrieve an index.</param>
        /// <returns>The index of the specified container within the item control which owns it.</returns>
        public Int32 IndexFromContainer(DependencyObject container)
        {
            Contract.Require(container, "container");

            return owner.ItemContainers.IndexOf(container);
        }

        /// <summary>
        /// Associates an item container with the specified item.
        /// </summary>
        /// <param name="container">The item container.</param>
        /// <param name="item">The contained item.</param>
        internal void AssociateContainerWithItem(DependencyObject container, Object item)
        {
            Contract.Require(container, "container");

            if (item == null)
            {
                container.ClearLocalValue(AssociatedItemProperty);
                itemToContainerMapping.Remove(item);
            }
            else
            {
                container.SetLocalValue<Object>(AssociatedItemProperty, item);
                itemToContainerMapping.Add(item, container);
            }
        }

        /// <summary>
        /// Represents an attached property which is used to associate containers with items.
        /// </summary>
        private static readonly DependencyProperty AssociatedItemProperty = DependencyProperty.RegisterAttached("AssociatedItem", typeof(Object), typeof(ItemContainerGenerator),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.None));

        // State values.
        private readonly ItemsControl owner;

        // Maps items to their respective containers.
        private readonly Dictionary<Object, DependencyObject> itemToContainerMapping = 
            new Dictionary<Object, DependencyObject>();
    }
}
