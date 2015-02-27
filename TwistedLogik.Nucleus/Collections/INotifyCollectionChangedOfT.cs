
namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionReset"/> event is raised.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    public delegate void CollectionResetEventHandler<in T>(INotifyCollectionChanged<T> collection);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionItemAdded"/> event is raised.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was added to the collection.</param>
    public delegate void CollectionItemAddedEventHandler<in T>(INotifyCollectionChanged<T> collection, T item);

    /// <summary>
    /// Represents the method that is called when the <see cref="INotifyCollectionChanged{T}.CollectionItemRemoved"/> event is raised.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection.</typeparam>
    /// <param name="collection">The collection that was changed.</param>
    /// <param name="item">The item that was removed from the collection.</param>
    public delegate void CollectionItemRemovedEventHandler<in T>(INotifyCollectionChanged<T> collection, T item);

    /// <summary>
    /// Represents a collection which raises events when its content changes.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection.</typeparam>
    public interface INotifyCollectionChanged<out T> : INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when the contents of the collection are dramatically changed.
        /// </summary>
        new event CollectionResetEventHandler<T> CollectionReset;

        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        new event CollectionItemAddedEventHandler<T> CollectionItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        new event CollectionItemRemovedEventHandler<T> CollectionItemRemoved;
    }
}
