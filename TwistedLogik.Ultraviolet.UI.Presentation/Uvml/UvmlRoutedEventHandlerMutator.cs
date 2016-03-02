using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets a routed event handler.
    /// </summary>
    internal sealed class UvmlRoutedEventHandlerMutator : UvmlMutator
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
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var uiElement = instance as UIElement;
            if (uiElement == null)
                return;

            var eventHandlerName = revtHandler.Instantiate(uv, context) as String;
            if (eventHandlerName == null)
                throw new UvmlException(PresentationStrings.InvalidEventHandler.Format(revtID.Name, "(null)"));

            var eventHandlerMethod = context.DataSourceType.GetMethod(eventHandlerName);
            if (eventHandlerMethod == null)
                throw new UvmlException(PresentationStrings.InvalidEventHandler.Format(revtID.Name, eventHandlerName));

            var eventHandlerDelegate = Delegate.CreateDelegate(revtID.DelegateType, eventHandlerMethod);
            uiElement.AddHandler(revtID, eventHandlerDelegate);
        }

        // State values.
        private readonly RoutedEvent revtID;
        private readonly UvmlNode revtHandler;
    }
}
