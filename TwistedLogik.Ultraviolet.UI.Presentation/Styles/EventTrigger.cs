using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a routed event is raised.
    /// </summary>
    public sealed class EventTrigger : Trigger, IRoutedEventRaisedNotificationSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        /// <param name="eventName">The name of the event that causes this trigger to be applied.</param>
        internal EventTrigger(String eventName)
        {
            Contract.RequireNotEmpty(eventName, "eventName");

            this.eventName = new UvmlName(eventName);
        }

        /// <inheritdoc/>
        void IRoutedEventRaisedNotificationSubscriber.ReceiveRoutedEventRaisedNotification(DependencyObject dobj, RoutedEvent evt)
        {
            Activate(dobj);
        }

        /// <summary>
        /// Gets the name of the event that causes this trigger to be applied.
        /// </summary>
        public UvmlName EventName
        {
            get { return eventName; }
        }

        /// <inheritdoc/>
        protected internal override void Attach(DependencyObject dobj)
        {
            var routedEvent = EventManager.FindByStylingName(Ultraviolet, dobj, eventName.Owner, eventName.Name);
            if (routedEvent == null)
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(eventName, dobj.GetType()));

            RoutedEvent.RegisterRaisedNotification(dobj, routedEvent, this);
        }

        /// <inheritdoc/>
        protected internal override void Detach(DependencyObject dobj)
        {
            var routedEvent = EventManager.FindByStylingName(Ultraviolet, dobj, eventName.Owner, eventName.Name);
            if (routedEvent == null)
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(eventName, dobj.GetType()));

            RoutedEvent.UnregisterRaisedNotification(dobj, routedEvent, this);
        }

        // Property values.
        private readonly UvmlName eventName;
    }
}
