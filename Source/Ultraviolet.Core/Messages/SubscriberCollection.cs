using System;
using System.Collections.Generic;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Core.Messages
{
    /// <summary>
    /// Represents a collection of message subscribers.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message received by the subscribers in this collection.</typeparam>
    public class SubscriberCollection<TMessageType> where TMessageType : IEquatable<TMessageType>
    {
        /// <summary>
        /// Pushes the specified message to all subscribers to the specified message type.
        /// </summary>
        /// <param name="messageType">The type of message being pushed.</param>
        /// <param name="data">The data for the message being pushed.</param>
        public void ReceiveMessage(TMessageType messageType, MessageData data)
        {
            var tempstorage = default(List<IMessageSubscriber<TMessageType>>);

            lock (tempstoragePool)
                tempstorage = tempstoragePool.Retrieve();
            
            foreach (var sub in this[messageType])
                tempstorage.Add(sub);

            foreach (var subscriber in tempstorage)
            {
                subscriber.ReceiveMessage(messageType, data);
            }

            lock (tempstoragePool)
                tempstoragePool.Release(tempstorage);
        }

        /// <summary>
        /// Purges the subscriber list associated with the specified message type.
        /// </summary>
        /// <param name="messageType">The type of message for which to purge subscribers.</param>
        public void Purge(TMessageType messageType)
        {
            subscribers.Remove(messageType);
        }

        /// <summary>
        /// Purges the subscriber lists associated with all message types.
        /// </summary>
        public void PurgeAll()
        {
            subscribers.Clear();
        }

        /// <summary>
        /// Removes the specified subscriber from all subscriber lists.
        /// </summary>
        /// <param name="subscriber">The subscriber to remove.</param>
        public void RemoveFromAll(IMessageSubscriber<TMessageType> subscriber)
        {
            Contract.Require(subscriber, nameof(subscriber));

            foreach (var collection in subscribers)
            {
                collection.Value.Remove(subscriber);
            }
        }

        /// <summary>
        /// Gets the set of subscribers which are subscribed to the specified message type.
        /// </summary>
        /// <param name="type">The type of message for which to get a subscriber list.</param>
        /// <returns>The set of subscribers which are subscribed to the specified message type.</returns>
        public HashSet<IMessageSubscriber<TMessageType>> this[TMessageType type]
        {
            get
            {
                HashSet<IMessageSubscriber<TMessageType>> subscribersToMessageType;
                if (!subscribers.TryGetValue(type, out subscribersToMessageType))
                {
                    subscribersToMessageType = new HashSet<IMessageSubscriber<TMessageType>>();
                    subscribers[type] = subscribersToMessageType;
                }
                return subscribersToMessageType;
            }
        }

        // The underlying table of subscribers for each message type.
        private readonly Dictionary<TMessageType, HashSet<IMessageSubscriber<TMessageType>>> subscribers =
            new Dictionary<TMessageType, HashSet<IMessageSubscriber<TMessageType>>>();

        // A list which temporarily stores the list of subscribers while
        // publishing messages, so that the subscription table can be updated during enumeration.
        private readonly IPool<List<IMessageSubscriber<TMessageType>>> tempstoragePool =
            new ExpandingPool<List<IMessageSubscriber<TMessageType>>>(1, () => new List<IMessageSubscriber<TMessageType>>(), item => item.Clear());
    }
}
