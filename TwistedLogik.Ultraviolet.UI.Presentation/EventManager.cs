using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for registering routed events and their associated handlers.
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// Registers a new routed event.
        /// </summary>
        /// <param name="name">The routed event's name.</param>
        /// <param name="routingStrategy">The routed event's routing strategy.</param>
        /// <param name="delegateType">The routed event's delegate type.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the registered routed event.</returns>
        public static RoutedEvent RegisterRoutedEvent(String name, RoutingStrategy routingStrategy, Type delegateType, Type ownerType)
        {
            return RoutedEventSystem.Register(name, null, routingStrategy, delegateType, ownerType);
        }

        /// <summary>
        /// Registers a new routed event.
        /// </summary>
        /// <param name="name">The routed event's name.</param>
        /// <param name="uvssName">The dependency property's name within the UVSS styling system.</param>
        /// <param name="routingStrategy">The routed event's routing strategy.</param>
        /// <param name="delegateType">The routed event's delegate type.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the registered routed event.</returns>
        public static RoutedEvent RegisterRoutedEvent(String name, String uvssName, RoutingStrategy routingStrategy, Type delegateType, Type ownerType)
        {
            return RoutedEventSystem.Register(name, uvssName, routingStrategy, delegateType, ownerType);
        }

        /// <summary>
        /// Registers a class handler for a routed event.
        /// </summary>
        /// <param name="classType">The type of the class that is declaring class handling.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> which identifies the event to handle.</param>
        /// <param name="handler">The delegate that represents the class handler to register.</param>
        public static void RegisterClassHandler(Type classType, RoutedEvent routedEvent, Delegate handler)
        {
            RoutedEventClassHandlers.RegisterClassHandler(classType, routedEvent, handler);
        }

        /// <summary>
        /// Registers a class handler for a routed event.
        /// </summary>
        /// <param name="classType">The type of the class that is declaring class handling.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> which identifies the event to handle.</param>
        /// <param name="handler">The delegate that represents the class handler to register.</param>
        /// <param name="handledEventsToo">A value indicating whether to invoke the handler even if it has already been handled.</param>
        public static void RegisterClassHandler(Type classType, RoutedEvent routedEvent, Delegate handler, Boolean handledEventsToo)
        {
            RoutedEventClassHandlers.RegisterClassHandler(classType, routedEvent, handler, handledEventsToo);
        }

        /// <summary>
        /// Gets the invocation delegate for the specified routed event.
        /// </summary>
        /// <typeparam name="T">The type of invocation delegate to retrieve.</typeparam>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to retrieve an invocation delegate.</param>
        /// <returns>A delegate of the requested type which will invoke the specified event.</returns>
        public static T GetInvocationDelegate<T>(RoutedEvent evt) where T : class
        {
            Contract.Require(evt, nameof(evt));

            return evt.InvocationDelegate as T;
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
            return RoutedEventSystem.FindByName(name, ownerType);
        }

        /// <summary>
        /// Finds the routed event with the specified name.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object which is searching for a routed event.</param>
        /// <param name="owner">The name of the routed event's owner type.</param>
        /// <param name="name">The name of the routed event.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the specified routed event, 
        /// or <see langword="null"/> if no such routed event exists.</returns>
        public static RoutedEvent FindByName(UltravioletContext uv, DependencyObject dobj, String owner, String name)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));
            Contract.RequireNotEmpty(name, nameof(name));

            var type = String.IsNullOrEmpty(owner) ? dobj.GetType() : null;
            if (type == null)
            {
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(owner, false, out type))
                    throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(owner));
            }

            return FindByName(name, type);
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
            return RoutedEventSystem.FindByStylingName(name, ownerType);
        }

        /// <summary>
        /// Finds the routed event with the specified styling name.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object which is searching for a routed event.</param>
        /// <param name="owner">The name of the routed event's owner type.</param>
        /// <param name="name">The styling name of the routed event.</param>
        /// <returns>A <see cref="RoutedEvent"/> instance which represents the specified routed event, 
        /// or <see langword="null"/> if no such routed event exists.</returns>
        public static RoutedEvent FindByStylingName(UltravioletContext uv, DependencyObject dobj, String owner, String name)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));
            Contract.RequireNotEmpty(name, nameof(name));

            var type = String.IsNullOrEmpty(owner) ? dobj.GetType() : null;
            if (type == null)
            {
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(owner, false, out type))
                    throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(owner));
            }

            return FindByStylingName(name, type);
        }
    }
}