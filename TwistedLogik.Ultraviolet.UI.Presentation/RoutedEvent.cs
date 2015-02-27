using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the identifier of a routed event.
    /// </summary>
    public sealed class RoutedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEvent"/> class.
        /// </summary>
        /// <param name="id">The event's unique identifier within the routed events system.</param>
        /// <param name="name">The routed event's name.</param>
        /// <param name="routingStrategy">The routed event's routing strategy.</param>
        /// <param name="delegateType">The routed event's delegate type.</param>
        /// <param name="ownerType">The routed event's owner type.</param>
        internal RoutedEvent(Int64 id, String name, RoutingStrategy routingStrategy, Type delegateType, Type ownerType)
        {
            this.id                 = id;
            this.name               = name;
            this.routingStrategy    = routingStrategy;
            this.delegateType       = delegateType;
            this.ownerType          = ownerType;
            this.invocationDelegate = RoutedEventInvocation.CreateInvocationDelegate(this);
        }

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
            return RoutedEventSystem.Register(name, routingStrategy, delegateType, ownerType);
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
            return RoutedEventSystem.FindByName(name, ownerType);
        }

        /// <summary>
        /// Gets the invocation delegate for the specified routed event.
        /// </summary>
        /// <typeparam name="T">The type of invocation delegate to retrieve.</typeparam>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to retrieve an invocation delegate.</param>
        /// <returns>A delegate of the requested type which will invoke the specified event.</returns>
        public static T GetInvocationDelegate<T>(RoutedEvent evt) where T : class
        {
            Contract.Require(evt, "evt");

            return evt.InvocationDelegate as T;
        }

        /// <summary>
        /// Gets or sets the event's unique identifier within the routed events system.
        /// </summary>
        internal Int64 ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the routed event's name.
        /// </summary>
        internal String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the routed event's routing strategy.
        /// </summary>
        internal RoutingStrategy RoutingStrategy
        {
            get { return routingStrategy; }
        }

        /// <summary>
        /// Gets the routed event's delegate type.
        /// </summary>
        internal Type DelegateType
        {
            get { return delegateType; }
        }

        /// <summary>
        /// Gets the routed event's owner type.
        /// </summary>
        internal Type OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets the event's invocation delegate.
        /// </summary>
        internal Delegate InvocationDelegate
        {
            get { return invocationDelegate; }
        }

        // Property values.
        private readonly Int64 id;
        private readonly String name;
        private readonly RoutingStrategy routingStrategy;
        private readonly Type delegateType;
        private readonly Type ownerType;
        private readonly Delegate invocationDelegate;
    }
}
