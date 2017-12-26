using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for managing change notification subscriptions for dependency properties.
    /// </summary>
    internal sealed class DependencyPropertyChangeNotificationServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyChangeNotificationServer"/> class.
        /// </summary>
        /// <param name="dprop">The dependency property that this notification server represents.</param>
        public DependencyPropertyChangeNotificationServer(DependencyProperty dprop)
        {
            Contract.Require(dprop, nameof(dprop));

            this.dprop = dprop;
        }

        /// <summary>
        /// Adds a subscriber to the server's notification list.
        /// </summary>
        /// <param name="target">The target object for which the subscriber is requesting notifications.</param>
        /// <param name="subscriber">The subscriber that wishes to receive notifications for the specified target.</param>
        public void Subscribe(DependencyObject target, IDependencyPropertyChangeNotificationSubscriber subscriber)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber> subscribers;
                if (!subscriptions.TryGetValue(target, out subscribers))
                {
                    lock (subscriberListPool)
                    {
                        subscribers = subscriberListPool.Retrieve();
                    }
                    subscriptions[target] = subscribers;
                }

                subscribers.AddLast(subscriber);
            }
        }

        /// <summary>
        /// Removes a subscriber from the server's notification list.
        /// </summary>
        /// <param name="target">The target object for which the subscriber was requesting notifications.</param>
        /// <param name="subscriber">The subscriber that wishes to stop receiving notifications for the specified target.</param>
        public void Unsubscribe(DependencyObject target, IDependencyPropertyChangeNotificationSubscriber subscriber)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber> subscribers;
                if (!subscriptions.TryGetValue(target, out subscribers))
                    return;

                subscribers.Remove(subscriber);

                if (subscribers.Count == 0)
                {
                    subscriptions.Remove(target);

                    lock (subscriberListPool)
                    {
                        subscriberListPool.Release(subscribers);
                    }
                }
            }
        }

        /// <summary>
        /// Raises a property change notification for all subscribers to the specified target.
        /// </summary>
        /// <param name="target">The target object for which to raise a notification.</param>
        public void Notify(DependencyObject target)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber> subscribers;
                if (!subscriptions.TryGetValue(target, out subscribers))
                    return;

                for (var current = subscribers.First; current != null; current = current.Next)
                {
                    current.Value.ReceiveDependencyPropertyChangeNotification(target, dprop);
                }
            }
        }

        // The global pool of subscriber lists.
        private static readonly IPool<PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber>> subscriberListPool = 
            new ExpandingPool<PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber>>(16, () => new PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber>(1));

        // State values.
        private readonly DependencyProperty dprop;
        private readonly Dictionary<DependencyObject, PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber>> subscriptions = 
            new Dictionary<DependencyObject, PooledLinkedList<IDependencyPropertyChangeNotificationSubscriber>>(1);
    }
}
