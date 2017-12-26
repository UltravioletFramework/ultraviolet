using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for managing notification subscriptions for routed event.
    /// </summary>
    internal sealed class RoutedEventRaisedNotificationServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventRaisedNotificationServer"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event that this notification server represents.</param>
        public RoutedEventRaisedNotificationServer(RoutedEvent routedEvent)
        {
            Contract.Require(routedEvent, nameof(routedEvent));

            this.routedEvent = routedEvent;
        }

        /// <summary>
        /// Adds a subscriber to the server's notification list.
        /// </summary>
        /// <param name="target">The target object for which the subscriber is requesting notifications.</param>
        /// <param name="subscriber">The subscriber that wishes to receive notifications for the specified target.</param>
        public void Subscribe(DependencyObject target, IRoutedEventRaisedNotificationSubscriber subscriber)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IRoutedEventRaisedNotificationSubscriber> subscribers;
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
        public void Unsubscribe(DependencyObject target, IRoutedEventRaisedNotificationSubscriber subscriber)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IRoutedEventRaisedNotificationSubscriber> subscribers;
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
        /// <param name="data">The routed event data.</param>
        public void Notify(DependencyObject target, RoutedEventData data)
        {
            lock (subscriptions)
            {
                PooledLinkedList<IRoutedEventRaisedNotificationSubscriber> subscribers;
                if (!subscriptions.TryGetValue(target, out subscribers))
                    return;

                for (var current = subscribers.First; current != null; current = current.Next)
                {
                    current.Value.ReceiveRoutedEventRaisedNotification(target, routedEvent, data);
                }
            }
        }

        // The global pool of subscriber lists.
        private static readonly IPool<PooledLinkedList<IRoutedEventRaisedNotificationSubscriber>> subscriberListPool = 
            new ExpandingPool<PooledLinkedList<IRoutedEventRaisedNotificationSubscriber>>(16, () => new PooledLinkedList<IRoutedEventRaisedNotificationSubscriber>(1));

        // State values.
        private readonly RoutedEvent routedEvent;
        private readonly Dictionary<DependencyObject, PooledLinkedList<IRoutedEventRaisedNotificationSubscriber>> subscriptions = 
            new Dictionary<DependencyObject, PooledLinkedList<IRoutedEventRaisedNotificationSubscriber>>(1);
    }
}
