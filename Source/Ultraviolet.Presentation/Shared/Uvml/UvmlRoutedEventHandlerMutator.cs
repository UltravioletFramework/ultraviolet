using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets a routed event handler.
    /// </summary>
    internal sealed class UvmlRoutedEventHandlerMutator : UvmlEventMutator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlRoutedEventHandlerMutator"/> class.
        /// </summary>
        /// <param name="revtID">The event which is being mutated.</param>
        /// <param name="revtHandler">The event handler to add to the event.</param>
        public UvmlRoutedEventHandlerMutator(RoutedEvent revtID, UvmlNode revtHandler)
        {
            Contract.Require(revtID, nameof(revtID));
            Contract.Require(revtHandler, nameof(revtHandler));

            this.revtID = revtID;
            this.revtHandler = revtHandler;
        }

        /// <inheritdoc/>
        public override Object InstantiateValue(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            return revtHandler.Instantiate(uv, context);
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var value = InstantiateValue(uv, instance, context);
            Mutate(uv, instance, value, context);
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, Object value, UvmlInstantiationContext context)
        {
            var uiElement = instance as UIElement;
            if (uiElement == null)
                return;

            var eventHandlerName = ProcessPrecomputedValue<String>(value, context);
            if (eventHandlerName == null)
                throw new UvmlException(PresentationStrings.InvalidEventHandler.Format(revtID.Name, "(null)"));

            var eventHandlerDelegate = CreateEventHandlerDelegate(eventHandlerName, revtID.DelegateType, context);
            uiElement.AddHandler(revtID, eventHandlerDelegate);
        }

        // State values.
        private readonly RoutedEvent revtID;
        private readonly UvmlNode revtHandler;
    }
}
