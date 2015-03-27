using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the collection which makes up the content of an <see cref="ItemsControl"/>.
    /// </summary>
    public sealed partial class ItemCollection : INotifyCollectionChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCollection"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ItemsControl"/> that owns the collection.</param>
        public ItemCollection(ItemsControl owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }

        /// <inheritdoc/>
        public event CollectionResetEventHandler CollectionReset;

        /// <inheritdoc/>
        public event CollectionItemAddedEventHandler CollectionItemAdded;

        /// <inheritdoc/>
        public event CollectionItemRemovedEventHandler CollectionItemRemoved;

        /// <summary>
        /// Sets the collection's items source.
        /// </summary>
        /// <param name="itemsSource">The items source for this collection.</param>
        internal void SetItemsSource(IEnumerable itemsSource)
        {
            if (this.itemsSource != null)
                UnhookItemsSourceEvents();

            this.itemsSource = itemsSource;

            if (this.itemsSource != null)
                HookItemsSourceEvents();

            OnCollectionReset();
        }

        /// <summary>
        /// Gets the <see cref="ItemsControl"/> that owns the collection.
        /// </summary>
        internal ItemsControl Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// Gets the items source to which the collection is currently bound, if any.
        /// </summary>
        internal IEnumerable ItemsSource
        {
            get { return itemsSource; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is currently bound to an items source.
        /// </summary>
        internal Boolean IsBoundToItemsSource
        {
            get { return itemsSource != null; }
        }

        /// <summary>
        /// If the collection has an observable items source, this method hooks into
        /// the events used to track collection changes.
        /// </summary>
        private void HookItemsSourceEvents()
        {
            var observable = itemsSource as INotifyCollectionChanged;
            if (observable == null)
                return;

            observable.CollectionReset       += ItemsSourceCollectionReset;
            observable.CollectionItemAdded   += ItemsSourceCollectionItemAdded;
            observable.CollectionItemRemoved += ItemsSourceCollectionItemRemoved;
        }

        /// <summary>
        /// If the collection has an observable items source, this method unhooks from
        /// the events used to track collection changes.
        /// </summary>
        private void UnhookItemsSourceEvents()
        {
            var observable = itemsSource as INotifyCollectionChanged;
            if (observable == null)
                return;

            observable.CollectionReset       -= ItemsSourceCollectionReset;
            observable.CollectionItemAdded   -= ItemsSourceCollectionItemAdded;
            observable.CollectionItemRemoved -= ItemsSourceCollectionItemRemoved;
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionReset"/> event for the bound items source.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        private void ItemsSourceCollectionReset(INotifyCollectionChanged collection)
        {
            OnCollectionReset();
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event for the bound items source.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was added to the collection.</param>
        private void ItemsSourceCollectionItemAdded(INotifyCollectionChanged collection, Object item)
        {
            OnCollectionItemAdded(item);
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event for the bound items source.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was removed from the collection.</param>
        private void ItemsSourceCollectionItemRemoved(INotifyCollectionChanged collection, Object item)
        {
            OnCollectionItemRemoved(item);
        }

        /// <summary>
        /// Called when the item collection dramatically changes.
        /// </summary>
        private void OnCollectionReset()
        {
            var temp = CollectionReset;
            if (temp != null)
            {
                temp(this);
            }
        }
        
        /// <summary>
        /// Called when an item is added to the item collection.
        /// </summary>
        /// <param name="item">The item that was added to the collection.</param>
        private void OnCollectionItemAdded(Object item)
        {
            var temp = CollectionItemAdded;
            if (temp != null)
            {
                temp(this, item);
            }
        }

        /// <summary>
        /// Called when an item is removed from the item collection.
        /// </summary>
        /// <param name="item">The item that was removed from the collection.</param>
        private void OnCollectionItemRemoved(Object item)
        {
            var temp = CollectionItemRemoved;
            if (temp != null)
            {
                temp(this, item);
            }
        }

        /// <summary>
        /// Throws an exception if the collection is currently bound to an items source.
        /// </summary>
        private void EnsureNotBoundToItemsSource()
        {
            if (IsBoundToItemsSource)
            {
                throw new InvalidOperationException(PresentationStrings.CollectionIsBoundToItemsSource);
            }
        }

        // Property values.
        private readonly ItemsControl owner;
        private IEnumerable itemsSource;

        // The backing storage for this collection when it is not bound to an items source.
        private readonly List<Object> itemsStorage = new List<Object>();
    }
}
