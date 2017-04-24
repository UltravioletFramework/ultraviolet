
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which can subscribe to dependency property change notifications.
    /// </summary>
    internal interface IDependencyPropertyChangeNotificationSubscriber
    {
        /// <summary>
        /// Called when the object receives a dependency property change notification.
        /// </summary>
        /// <param name="dobj">The dependency object that was changed.</param>
        /// <param name="dprop">The dependency property that was changed.</param>
        void ReceiveDependencyPropertyChangeNotification(DependencyObject dobj, DependencyProperty dprop);
    }
}
