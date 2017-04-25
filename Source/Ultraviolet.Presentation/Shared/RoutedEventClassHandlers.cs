using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Manages the class handlers for routed events.
    /// </summary>
    internal static class RoutedEventClassHandlers
    {
        /// <summary>
        /// Registers a class handler for a routed event.
        /// </summary>
        /// <param name="classType">The type of the class that is declaring class handling.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> which identifies the event to handle.</param>
        /// <param name="handler">The delegate that represents the class handler to register.</param>
        public static void RegisterClassHandler(Type classType, RoutedEvent routedEvent, Delegate handler)
        {
            RegisterClassHandler(classType, routedEvent, handler, false);
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
            Contract.Require(classType, nameof(classType));
            Contract.Require(routedEvent, nameof(routedEvent));
            Contract.Require(handler, nameof(handler));

            var manager = GetClassHandlerManager(routedEvent, classType);
            manager.AddHandler(handler, handledEventsToo);

            Dictionary<Type, RoutedEventClassHandlerManager> otherManagers;
            if (managers.TryGetValue(routedEvent, out otherManagers))
            {
                foreach (var kvp in otherManagers)
                {
                    if (kvp.Key.IsSubclassOf(classType))
                    {
                        kvp.Value.AddHandler(classType, handler, handledEventsToo);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the internal list of class handlers for the specified routed event.
        /// </summary>
        /// <param name="classType">The type of the class for which to retrieve class handlers.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> which identifies the event for which to retrieve class handlers.</param>
        /// <returns>The internal list of handlers for the specified routed event.</returns>
        internal static List<RoutedEventHandlerMetadata> GetClassHandlers(Type classType, RoutedEvent routedEvent)
        {
            Contract.Require(classType, nameof(classType));
            Contract.Require(routedEvent, nameof(routedEvent));

            var manager = GetClassHandlerManager(routedEvent, classType, false);
            if (manager == null)
            {
                while (true)
                {
                    classType = classType.BaseType;

                    if (classType == null)
                        break;

                    manager = GetClassHandlerManager(routedEvent, classType, false);
                    if (manager != null)
                    {
                        return manager.GetClassHandlers();
                    }
                }
                return null;
            }
            return manager.GetClassHandlers();
        }

        /// <summary>
        /// Gets the <see cref="RoutedEventClassHandlerManager"/> for the specified event and type.
        /// </summary>
        /// <param name="routedEvent">The event for which to retrieve a class handler manager.</param>
        /// <param name="classType">The type for which to retrieve a class handler manager.</param>
        /// <param name="createIfMissing">A value specifying whether the manager should be created if it does not already exist.</param>
        /// <returns>The <see cref="RoutedEventClassHandlerManager"/> for the specified event and type.</returns>
        private static RoutedEventClassHandlerManager GetClassHandlerManager(RoutedEvent routedEvent, Type classType, Boolean createIfMissing = true)
        {
            Dictionary<Type, RoutedEventClassHandlerManager> managersByType;
            if (!managers.TryGetValue(routedEvent, out managersByType))
            {
                if (!createIfMissing)
                    return null;

                managersByType = new Dictionary<Type, RoutedEventClassHandlerManager>();
                managers[routedEvent] = managersByType;
            }

            RoutedEventClassHandlerManager manager;
            if (!managersByType.TryGetValue(classType, out manager))
            {
                if (!createIfMissing)
                    return null;

                manager = CreateClassHandlerManager(routedEvent, classType);
                managersByType[classType] = manager;
            }

            return manager;
        }

        /// <summary>
        /// Creates a new <see cref="RoutedEventClassHandlerManager"/> for the specified event and type,
        /// populating the manager with relevant handlers from existing types that have already been registered.
        /// </summary>
        /// <param name="routedEvent">The event for which to create a class handler manager.</param>
        /// <param name="classType">The type for which to create a class handler manager.</param>
        /// <returns>The <see cref="RoutedEventClassHandlerManager"/> that was created for the specified event and type.</returns>
        private static RoutedEventClassHandlerManager CreateClassHandlerManager(RoutedEvent routedEvent, Type classType)
        {
            var manager = new RoutedEventClassHandlerManager(routedEvent, classType);

            Dictionary<Type, RoutedEventClassHandlerManager> existing;
            if (managers.TryGetValue(routedEvent, out existing))
            {
                manager.SuspendSort();
                foreach (var kvp in existing)
                {
                    if (classType.IsSubclassOf(kvp.Key))
                    {
                        var handlers = kvp.Value.GetClassHandlers();
                        if (handlers == null)
                            continue;

                        lock (handlers)
                        {
                            foreach (var handler in handlers)
                            {
                                manager.AddHandler(kvp.Key, handler.Handler, handler.HandledEventsToo);
                            }
                        }
                    }
                }
                manager.ResumeSort();
            }

            return manager;
        }

        // State values.
        private static readonly Dictionary<RoutedEvent, Dictionary<Type, RoutedEventClassHandlerManager>> managers = 
            new Dictionary<RoutedEvent, Dictionary<Type, RoutedEventClassHandlerManager>>();
    }
}
