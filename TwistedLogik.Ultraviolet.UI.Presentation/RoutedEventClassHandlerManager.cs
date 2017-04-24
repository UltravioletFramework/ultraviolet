using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the storage for class handlers for a particular UI element type.
    /// </summary>
    internal class RoutedEventClassHandlerManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventClassHandlerManager"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event with which the registry is associated.</param>
        /// <param name="ownerType">The type with which this registry is associated.</param>
        public RoutedEventClassHandlerManager(RoutedEvent routedEvent, Type ownerType)
        {
            Contract.Require(routedEvent, nameof(routedEvent));
            Contract.Require(ownerType, nameof(ownerType));

            this.routedEvent = routedEvent;
            this.ownerType   = ownerType;
        }

        /// <summary>
        /// Adds a class handler to the manager.
        /// </summary>
        /// <param name="handler">A delegate which represents the class handler to add to the type.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public void AddHandler(Delegate handler, Boolean handledEventsToo)
        {
            AddHandler(ownerType, handler, handledEventsToo);
        }

        /// <summary>
        /// Adds a class handler to the manager.
        /// </summary>
        /// <param name="classType">The type for which the handler is defined; must be a supertype of the manager's owner type.</param>
        /// <param name="handler">A delegate which represents the class handler to add to the type.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public void AddHandler(Type classType, Delegate handler, Boolean handledEventsToo)
        {
            if (classType != this.ownerType && !this.ownerType.IsSubclassOf(classType))
                throw new ArgumentException(PresentationStrings.ClassTypeMustBeSubclassOfOwnerType.Format(classType.Name, ownerType.Name));

            if (routedEvent.DelegateType != handler.GetType())
                throw new ArgumentException(PresentationStrings.HandlerTypeMismatch.Format(handler.Method.Name, routedEvent.DelegateType.Name), "handler");

            lock (handlers)
            {
                var ordinalByType     = (short)0;
                var ordinalWithinType = ordinal++;

                var currentType = this.ownerType;
                while (currentType != classType)
                {
                    ordinalByType++;
                    currentType = currentType.BaseType;
                }

                var metadata = new RoutedEventHandlerMetadata(handler, ordinalByType, ordinalWithinType, handledEventsToo);
                handlers.Add(metadata);

                if (!sortSuspended)
                {
                    Sort();
                }
            }
        }

        /// <summary>
        /// Removes a class handler from the manager.
        /// </summary>
        /// <param name="handler">A delegate which represents the handler to remove from the specified routed event.</param>
        public void RemoveHandler(Delegate handler)
        {
            lock (handlers)
            {
                for (int i = 0; i < handlers.Count; i++)
                {
                    var current = handlers[i];
                    if (current.Handler == handler)
                    {
                        handlers.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Suspends automatic sorting operations.
        /// </summary>
        public void SuspendSort()
        {
            sortSuspended = true;
        }

        /// <summary>
        /// Resumes automatic sorting operations.
        /// </summary>
        public void ResumeSort()
        {
            sortSuspended = false;
            Sort();
        }

        /// <summary>
        /// Gets the type with which this manager is associated.
        /// </summary>
        public Type OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets the manager's internal list of class handlers.
        /// </summary>
        /// <returns>The manager's internal list of class handlers.</returns>
        internal List<RoutedEventHandlerMetadata> GetClassHandlers()
        {
            return handlers;
        }

        /// <summary>
        /// Sorts the class handlers by type.
        /// </summary>
        private void Sort()
        {
            lock (handlers)
            {
                handlers.Sort((m1, m2) =>
                {
                    if (m1.OrdinalByType == m2.OrdinalByType)
                    {
                        return m1.OrdinalWithinType.CompareTo(m2.OrdinalWithinType);
                    }
                    return m1.OrdinalByType.CompareTo(m2.OrdinalByType);
                });
            }
        }

        // Property values.
        private readonly Type ownerType;
        private readonly RoutedEvent routedEvent;

        // State values.
        private Int16 ordinal;
        private Boolean sortSuspended;
        private readonly List<RoutedEventHandlerMetadata> handlers = 
            new List<RoutedEventHandlerMetadata>();
    }
}
