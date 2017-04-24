using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger which fires when a routed event is raised.
    /// </summary>
    public sealed class UvssEventTrigger : UvssTrigger, IRoutedEventRaisedNotificationSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEventTrigger"/> class.
        /// </summary>
        /// <param name="eventName">The name of the event that causes this trigger to be applied.</param>
        /// <param name="handled">A value indicating whether this trigger should respond to handled events.</param>
        /// <param name="setHandled">A value indicating whether this trigger should mark the event as handled.</param>
        /// <param name="isImportant">A value indicating whether this trigger is considered important.</param>
        internal UvssEventTrigger(String eventName, Boolean handled, Boolean setHandled, Boolean isImportant)
            : base(isImportant)
        {
            Contract.RequireNotEmpty(eventName, nameof(eventName));

            this.eventName = new DependencyName(eventName);
            this.handled = handled;
            this.setHandled = setHandled;
        }

        /// <inheritdoc/>
        void IRoutedEventRaisedNotificationSubscriber.ReceiveRoutedEventRaisedNotification(DependencyObject dobj, RoutedEvent evt, RoutedEventData data)
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
        public DependencyName EventName
        {
            get { return eventName; }
        }

        /// <inheritdoc/>
        protected override void Attach(DependencyObject dobj)
        {
            var routedEvent = EventManager.FindByStylingName(Ultraviolet, dobj, eventName.Owner, eventName.Name);
            if (routedEvent == null)
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(eventName, dobj.GetType()));

            RoutedEvent.RegisterRaisedNotification(dobj, routedEvent, this);

            base.Attach(dobj);
        }

        /// <inheritdoc/>
        protected override void Detach(DependencyObject dobj)
        {
            var routedEvent = EventManager.FindByStylingName(Ultraviolet, dobj, eventName.Owner, eventName.Name);
            if (routedEvent == null)
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(eventName, dobj.GetType()));

            RoutedEvent.UnregisterRaisedNotification(dobj, routedEvent, this);

            base.Detach(dobj);
        }

        // Property values.
        private readonly DependencyName eventName;
        private readonly Boolean handled;
        private readonly Boolean setHandled;
    }
}
