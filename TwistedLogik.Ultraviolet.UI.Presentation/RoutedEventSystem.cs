using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
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
        /// <param name="uvssName">The routed event's name within the UVSS styling system.</param>
        /// <param name="routingStrategy">The routed event's routing strategy.</param>
        /// <param name="delegateType">The routed event's delegate type.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the registered routed event.</returns>
        public static RoutedEvent Register(String name, String uvssName, RoutingStrategy routingStrategy, Type delegateType, Type ownerType)
        {
            Contract.Require(name, nameof(name));
            Contract.Require(delegateType, nameof(delegateType));
            Contract.Require(ownerType, nameof(ownerType));

            var evt = new RoutedEvent(reid++, name, uvssName, routingStrategy, delegateType, ownerType);
            RegisterInternal(evt, ownerType);
            return evt;
        }

        /// <summary>
        /// Finds the routed event with the specified name.
        /// </summary>
        /// <param name="name">The name of the routed event for which to search.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the specified routed event, 
        /// or <see langword="null"/> if no such routed event exists.</returns>
        public static RoutedEvent FindByName(String name, Type ownerType)
        {
            Contract.Require(name, nameof(name));
            Contract.Require(ownerType, nameof(ownerType));

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
        /// Finds the routed event with the specified styling name.
        /// </summary>
        /// <param name="name">The styling name of the routed event for which to search.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the specified routed event, 
        /// or <see langword="null"/> if no such routed event exists.</returns>
        public static RoutedEvent FindByStylingName(String name, Type ownerType)
        {
            Contract.Require(name, nameof(name));
            Contract.Require(ownerType, nameof(ownerType));

            var type = ownerType;
            while (type != null)
            {
                var evt = default(RoutedEvent);
                var evtDomain = GetStylingEventDomain(type);
                if (evtDomain.TryGetValue(name, out evt))
                {
                    return evt;
                }
                type = type.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Adds a new owning type to the specified routed event.
        /// </summary>
        /// <param name="routedEvent">The routed event to which to add an owner type.</param>
        /// <param name="ownerType">The owner type to add to the specified routed event.</param>
        internal static RoutedEvent AddOwner(RoutedEvent routedEvent, Type ownerType)
        {
            Contract.Require(routedEvent, nameof(routedEvent));
            Contract.Require(ownerType, nameof(ownerType));

            RegisterInternal(routedEvent, ownerType);

            return routedEvent;
        }

        /// <summary>
        /// Registers the specified routed event.
        /// </summary>
        /// <param name="evt">The routed event to register.</param>
        /// <param name="ownerType">The owner type for which to register the routed event.</param>
        private static void RegisterInternal(RoutedEvent evt, Type ownerType)
        {
            var propertyDomain = GetEventDomain(ownerType);
            if (propertyDomain.ContainsKey(evt.Name))
                throw new ArgumentException(PresentationStrings.RoutedEventAlreadyRegistered);

            propertyDomain[evt.Name] = evt;

            var stylingPropertyDomain = GetStylingEventDomain(ownerType);
            if (stylingPropertyDomain.ContainsKey(evt.UvssName))
                throw new ArgumentException(PresentationStrings.RoutedEventAlreadyRegistered);

            stylingPropertyDomain[evt.UvssName] = evt;
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

        /// <summary>
        /// Gets the routed event styling domain associated with the specified owner type.
        /// </summary>
        /// <param name="type">The type for which to retrieve a routed event styling domain.</param>
        /// <returns>The routed event styling domain associated with the specified owner type.</returns>
        private static Dictionary<String, RoutedEvent> GetStylingEventDomain(Type type)
        {
            Dictionary<String, RoutedEvent> propertyDomain;
            if (!routedEventStylingRegistry.TryGetValue(type, out propertyDomain))
            {
                propertyDomain = new Dictionary<String, RoutedEvent>();
                routedEventStylingRegistry[type] = propertyDomain;
            }
            return propertyDomain;
        }

        // State values.
        private static readonly Dictionary<Type, Dictionary<String, RoutedEvent>> routedEventRegistry =
            new Dictionary<Type, Dictionary<String, RoutedEvent>>();
        private static readonly Dictionary<Type, Dictionary<String, RoutedEvent>> routedEventStylingRegistry =
            new Dictionary<Type, Dictionary<String, RoutedEvent>>();
        private static Int64 reid = 1;
    }
}
