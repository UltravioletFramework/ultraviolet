
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which can subscribe to routed event notifications.
    /// </summary>
    internal interface IRoutedEventRaisedNotificationSubscriber
    {
        /// <summary>
        /// Called when the object receives a routed event notification.
        /// </summary>
        /// <param name="dobj">The dependency object for which the event was raised.</param>
        /// <param name="evt">The routed event that was raised.</param>
        /// <param name="data">The routed event's metadata.</param>
        void ReceiveRoutedEventRaisedNotification(DependencyObject dobj, RoutedEvent evt, RoutedEventData data);
    }
}
