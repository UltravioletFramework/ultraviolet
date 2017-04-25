namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{TKey, TValue}.CollectionReset"/> event is raised.
    /// </summary>
    /// <typeparam name="TKey">The type of key or index used by the collection.</typeparam>
    /// <typeparam name="TValue">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    public delegate void CollectionResetEventHandler<TKey, in TValue>(INotifyCollectionChanged<TKey, TValue> collection);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{TKey, TValue}.CollectionItemAdded"/> event is raised.
    /// </summary>
    /// <typeparam name="TKey">The type of key or index used by the collection.</typeparam>
    /// <typeparam name="TValue">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="key">The key or index of the item that was added to the collection.</param>
    /// <param name="item">The item that was added to the collection.</param>
    public delegate void CollectionItemAddedEventHandler<TKey, in TValue>(INotifyCollectionChanged<TKey, TValue> collection, TKey key, TValue item);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{TKey, TValue}.CollectionItemRemoved"/> event is raised.
    /// </summary>
    /// <typeparam name="TKey">The type of key or index used by the collection.</typeparam>
    /// <typeparam name="TValue">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was removed from the collection.</param>
    /// <param name="key">The key or index of the item that was removed from the collection.</param>
    public delegate void CollectionItemRemovedEventHandler<TKey, in TValue>(INotifyCollectionChanged<TKey, TValue> collection, TKey key, TValue item);

    /// <summary>
    /// Represents a collection which raises events when its content changes.
    /// </summary>
    /// <typeparam name="TKey">The type of key or index used by the collection.</typeparam>
    /// <typeparam name="TValue">The type of item contained by the collection.</typeparam>
    public interface INotifyCollectionChanged<TKey, out TValue> : INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when the contents of the collection are dramatically changed.
        /// </summary>
        new event CollectionResetEventHandler<TKey, TValue> CollectionReset;

        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        new event CollectionItemAddedEventHandler<TKey, TValue> CollectionItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        new event CollectionItemRemovedEventHandler<TKey, TValue> CollectionItemRemoved;
    }
}
