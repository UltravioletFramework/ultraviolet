using System;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionReset"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    public delegate void CollectionResetEventHandler(INotifyCollectionChanged collection);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionItemAdded"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was added to the collection.</param>
    public delegate void CollectionItemAddedEventHandler(INotifyCollectionChanged collection, Object item);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionItemRemoved"/> event is raised.
    /// </summary>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was removed from the collection.</param>
    public delegate void CollectionItemRemovedEventHandler(INotifyCollectionChanged collection, Object item);

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
