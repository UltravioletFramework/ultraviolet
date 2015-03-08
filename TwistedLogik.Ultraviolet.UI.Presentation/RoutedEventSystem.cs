using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the global state of the routed event system.
    /// </summary>
    internal static class RoutedEventSystem
    {
        /// <summary>
        /// Registers a new routed event.
        /// </summary>
        /// <param name="name">The routed event's name.</param>
        /// <param name="routingStrategy">The routed event's routing strategy.</param>
        /// <param name="delegateType">The routed event's delegate type.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the registered routed event.</returns>
        public static RoutedEvent Register(String name, RoutingStrategy routingStrategy, Type delegateType, Type ownerType)
        {
            Contract.Require(name, "name");
            Contract.Require(delegateType, "delegateType");
            Contract.Require(ownerType, "ownerType");

            var evtDomain = GetEventDomain(ownerType);
            if (evtDomain.ContainsKey(name))
            {
                throw new ArgumentException(PresentationStrings.RoutedEventAlreadyRegistered);
            }
            var re = new RoutedEvent(reid++, name, routingStrategy, delegateType, ownerType);
            evtDomain[name] = re;
            return re;
        }

        /// <summary>
        /// Finds the routed event with the specified name.
        /// </summary>
        /// <param name="name">The name of the routed event for which to search.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the specified routed event, 
        /// or <c>null</c> if no such routed event exists.</returns>
        public static RoutedEvent FindByName(String name, Type ownerType)
        {
            Contract.Require(name, "name");
            Contract.Require(ownerType, "ownerType");

            var type = ownerType;
            while (type != null)
            {
                var evt = default(RoutedEvent);
                var evtDomain = GetEventDomain(type);
                if (evtDomain.TryGetValue(name, out evt))
                {
                    return evt;
                }
                type = type.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Gets the routed event domain associated with the specified owner type.
        /// </summary>
        /// <param name="type">The type for which to retrieve a routed event domain.</param>
        /// <returns>The routed event domain associated with the specified owner type.</returns>
        private static Dictionary<String, RoutedEvent> GetEventDomain(Type type)
        {
            Dictionary<String, RoutedEvent> eventDomain;
            if (!routedEventRegistry.TryGetValue(type, out eventDomain))
            {
                eventDomain = new Dictionary<String, RoutedEvent>();
                routedEventRegistry[type] = eventDomain;
            }
            return eventDomain;
        }

        // State values.
        private static readonly Dictionary<Type, Dictionary<String, RoutedEvent>> routedEventRegistry =
            new Dictionary<Type, Dictionary<String, RoutedEvent>>();
        private static Int64 reid = 1;
    }
}
