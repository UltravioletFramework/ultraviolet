using System;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged.CollectionReset"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    public delegate void CollectionResetEventHandler(INotifyCollectionChanged collection);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was added to the collection.</param>
    /// <param name="index">The index at which the item was added to the collection, if this is an indexed collection; otherwise, <see langword="null"/>.</param>
    public delegate void CollectionItemAddedEventHandler(INotifyCollectionChanged collection, Int32? index, Object item);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was removed from the collection.</param>
    /// <param name="index">The index at which the item was added to the collection, if this is an indexed collection; otherwise, <see langword="null"/>.</param>
    public delegate void CollectionItemRemovedEventHandler(INotifyCollectionChanged collection, Int32? index, Object item);

    /// <summary>
    /// Represents a collection which raises events when its content changes.
    /// </summary>
    public interface INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when the contents of the collection are dramatically changed.
        /// </summary>
        event CollectionResetEventHandler CollectionReset;

        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        event CollectionItemAddedEventHandler CollectionItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        event CollectionItemRemovedEventHandler CollectionItemRemoved;
    }
}
