using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the storage for a UI element's routed events.
    /// </summary>
    public sealed class RoutedEventManager
    {
        /// <summary>
        /// Adds a handler to the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event to which to add a handler.</param>
        /// <param name="handler">A delegate which represents the handler to add to the specified routed event.</param>
        public void Add(RoutedEvent evt, Delegate handler)
        {
            Contract.Require(evt, "evt");
            Contract.Require(handler, "handler");

            Delegate existing;
            if (routedEventDelegates.TryGetValue(evt.ID, out existing))
            {
                existing = Delegate.Combine(existing, handler);
                routedEventDelegates[evt.ID] = existing;
            }
            else
            {
                routedEventDelegates[evt.ID] = handler;
            }
        }

        /// <summary>
        /// Removes a handler from the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event from which to remove a handler.</param>
        /// <param name="handler">A delegate which represents the handler to remove from the specified routed event.</param>
        public void Remove(RoutedEvent evt, Delegate handler)
        {
            Contract.Require(evt, "evt");
            Contract.Require(handler, "handler");

            Delegate existing;
            if (routedEventDelegates.TryGetValue(evt.ID, out existing))
            {
                existing = Delegate.Remove(existing, handler);
                if (existing == null)
                {
                    routedEventDelegates.Remove(evt.ID);
                }
                else
                {
                    routedEventDelegates[evt.ID] = existing;
                }
            }
        }

        /// <summary>
        /// Gets the delegate which represents the handler for the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event for which to retrieve a handler.</param>
        /// <returns>The delegate which represents the handler for the specified routed event, or <c>null</c> if no handlers are registered.</returns>
        public Delegate Get(RoutedEvent evt)
        {
            Delegate handler;
            routedEventDelegates.TryGetValue(evt.ID, out handler);
            return handler;
        }

        /// <summary>
        /// Gets the delegate which represents the handler for the specified routed event.
        /// </summary>
        /// <typeparam name="T">The type of delegate to retrieve.</typeparam>
        /// <param name="evt">A <see cref="RoutedEvent"/> value which identifies the event for which to retrieve a handler.</param>
        /// <returns>The delegate which represents the handler for the specified routed event, or <c>null</c> if no handlers are registered.</returns>
        public T Get<T>(RoutedEvent evt) where T : class
        {
            return (T)(Object)Get(evt);
        }

        // Tracks the delegates associated with each routed event for this element.
        private readonly Dictionary<Int64, Delegate> routedEventDelegates = 
            new Dictionary<Int64, Delegate>();
    }
}
