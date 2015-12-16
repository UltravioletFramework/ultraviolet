using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives a generic interaction event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The input device.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfGenericInteractionEventHandler(DependencyObject element, UltravioletResource device, ref RoutedEventData data);

    /// <summary>
    /// Contains generic input events which are not associated with any particular class of device.
    /// </summary>
    [UvmlKnownType]
    public static class Generic
    {
        /// <summary>
        /// Adds a handler for the PreviewGenericInteraction attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTapHandler(DependencyObject element, UpfGenericInteractionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewGenericInteractionEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the GenericInteraction attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGenericInteractionHandler(DependencyObject element, UpfGenericInteractionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, GenericInteractionEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewGenericInteraction attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewGenericInteractionHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewGenericInteractionEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GenericInteraction attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGenericInteractionHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, GenericInteractionEvent, handler);
        }

        /// <summary>
        /// Gets a value indicating whether a touch device is currently available.
        /// </summary>
        public static Boolean IsTouchDeviceAvailable
        {
            get { return Touch.PrimaryDevice != null; }
        }

        /// <summary>
        /// Identifies the PreviewGenericInteraction routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-generic-interaction.</remarks>
        public static readonly RoutedEvent PreviewGenericInteractionEvent = EventManager.RegisterRoutedEvent("PreviewGenericInteraction", RoutingStrategy.Tunnel,
            typeof(UpfGenericInteractionEventHandler), typeof(Generic));

        /// <summary>
        /// Identifies the GenericInteraction routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is generic-interaction.</remarks>
        public static readonly RoutedEvent GenericInteractionEvent = EventManager.RegisterRoutedEvent("GenericInteraction", RoutingStrategy.Bubble,
            typeof(UpfGenericInteractionEventHandler), typeof(Generic));

        /// <summary>
        /// Raises the PreviewGenericInteraction attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewGenericInteraction(DependencyObject element, UltravioletResource device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGenericInteractionEventHandler>(PreviewGenericInteractionEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }

        /// <summary>
        /// Raises the GenericInteraction attached event for the specified element.
        /// </summary>
        internal static void RaiseGenericInteraction(DependencyObject element, UltravioletResource device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfGenericInteractionEventHandler>(GenericInteractionEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }
    }
}
