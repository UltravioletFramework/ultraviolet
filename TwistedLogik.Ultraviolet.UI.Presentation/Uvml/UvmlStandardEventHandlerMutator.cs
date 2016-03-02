using System;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets a standard event handler.
    /// </summary>
    internal sealed class UvmlStandardEventHandlerMutator : UvmlMutator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlStandardEventHandlerMutator"/> class.
        /// </summary>
        /// <param name="eventInfo">The event which is being mutated.</param>
        /// <param name="eventHandler">The event handler to add to the event.</param>
        public UvmlStandardEventHandlerMutator(EventInfo eventInfo, UvmlNode eventHandler)
        {
            Contract.Require(eventInfo, nameof(eventInfo));
            Contract.Require(eventHandler, nameof(eventHandler));

            this.eventInfo = eventInfo;
            this.eventHandler = eventHandler;
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var eventHandlerName = eventHandler.Instantiate(uv, context) as String;
            if (eventHandlerName == null)
                throw new UvmlException(PresentationStrings.InvalidEventHandler.Format(eventInfo.Name, "(null)"));

            var eventHandlerMethod = context.DataSourceType.GetMethod(eventHandlerName);
            if (eventHandlerMethod == null)
                throw new UvmlException(PresentationStrings.InvalidEventHandler.Format(eventInfo.Name, eventHandlerName));

            var eventHandlerDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, eventHandlerMethod);
            eventInfo.AddEventHandler(instance, eventHandlerDelegate);
        }

        // State values.
        private readonly EventInfo eventInfo;
        private readonly UvmlNode eventHandler;
    }
}
