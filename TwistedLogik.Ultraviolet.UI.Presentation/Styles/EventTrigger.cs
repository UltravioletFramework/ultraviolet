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
        /// <param name="handled">A value indicating whether this trigger should respond to handled events.</param>
        /// <param name="setHandled">A value indicating whether this trigger should mark the event as handled.</param>
        internal EventTrigger(String eventName, Boolean handled, Boolean setHandled)
        {
            Contract.RequireNotEmpty(eventName, "eventName");

            this.eventName  = new UvmlName(eventName);
            this.handled    = handled;
            this.setHandled = setHandled;
        }

        /// <inheritdoc/>
        void IRoutedEventRaisedNotificationSubscriber.ReceiveRoutedEventRaisedNotification(DependencyObject dobj, RoutedEvent evt, ref RoutedEventData data)
        {
            if (!data.Handled || handled)
            {
                Activate(dobj);

                if (setHandled)
                {
                    data.Handled = true;
                }
            }
        }

        /// <inheritdoc/>
        public override String CanonicalName
        {
            get { return eventName.QualifiedName; }
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

            base.Attach(dobj);
        }

        /// <inheritdoc/>
        protected internal override void Detach(DependencyObject dobj)
        {
            var routedEvent = EventManager.FindByStylingName(Ultraviolet, dobj, eventName.Owner, eventName.Name);
            if (routedEvent == null)
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(eventName, dobj.GetType()));

            RoutedEvent.UnregisterRaisedNotification(dobj, routedEvent, this);

            base.Detach(dobj);
        }

        // Property values.
        private readonly UvmlName eventName;
        private readonly Boolean handled;
        private readonly Boolean setHandled;
    }
}
