using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the storage for a UI element's routed events.
    /// </summary>
    public sealed partial class RoutedEventManager
    {
        /// <summary>
        /// Adds a handler to the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event to which to add a handler.</param>
        /// <param name="handler">A delegate which represents the handler to add to the specified routed event.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public void Add(RoutedEvent evt, Delegate handler, Boolean handledEventsToo)
        {
            Contract.Require(evt, nameof(evt));
            Contract.Require(handler, nameof(handler));

            if (evt.DelegateType != handler.GetType())
                throw new ArgumentException(PresentationStrings.HandlerTypeMismatch.Format(handler.GetType().Name, evt.DelegateType.Name), "handler");

            lock (routedEventDelegates)
            {
                List<RoutedEventHandlerMetadata> events;
                if (!routedEventDelegates.TryGetValue(evt.ID, out events))
                {
                    events = new List<RoutedEventHandlerMetadata>();
                    routedEventDelegates[evt.ID] = events;
                }

                var routedEventInfo = new RoutedEventHandlerMetadata(handler, 0, 0, handledEventsToo);
                lock (events)
                {
                    events.Add(routedEventInfo);
                }
            }
        }

        /// <summary>
        /// Removes a handler from the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event from which to remove a handler.</param>
        /// <param name="handler">A delegate which represents the handler to remove from the specified routed event.</param>
        public void Remove(RoutedEvent evt, Delegate handler)
        {
            Contract.Require(evt, nameof(evt));
            Contract.Require(handler, nameof(handler));

            lock (routedEventDelegates)
            {
                List<RoutedEventHandlerMetadata> events;
                if (!routedEventDelegates.TryGetValue(evt.ID, out events))
                    return;

                lock (events)
                {
                    for (int i = 0; i < events.Count; i++)
                    {
                        if (events[i].Handler == handler)
                        {
                            events.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of registered event handlers for the specified event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event from which to retrieve a handler list.</param>
        /// <returns>The manager's internal list of handlers for the specified event.</returns>
        internal List<RoutedEventHandlerMetadata> GetHandlers(RoutedEvent evt)
        {
            List<RoutedEventHandlerMetadata> handlers;
            lock (routedEventDelegates)
            {
                routedEventDelegates.TryGetValue(evt.ID, out handlers);
            }
            return handlers;
        }

        // Tracks the delegates associated with each routed event for this element.
        private readonly Dictionary<Int64, List<RoutedEventHandlerMetadata>> routedEventDelegates = 
            new Dictionary<Int64, List<RoutedEventHandlerMetadata>>();
    }
}
