using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the method that is called when an interface element raises an event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    public delegate void UpfEventHandler(DependencyObject element);

    /// <summary>
    /// Represents the method that is called when an interface element raises a routed event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UpfRoutedEventHandler(DependencyObject element, ref RoutedEventData data);

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
