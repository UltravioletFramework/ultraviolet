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
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewGenericInteractionHandler(DependencyObject element, UpfGenericInteractionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewGenericInteractionEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/>
        /// attached event to the specified element.
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
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/>
        /// attached event to the specified element.
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
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/>
        /// attached event to the specified element.
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
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when an element is tapped or clicked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtFields><see cref="PreviewGenericInteractionEvent"/></revtFields>
        ///     <revtStylingName>preview-generic-interaction</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGenericInteractionEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewGenericInteractionEvent = EventManager.RegisterRoutedEvent("PreviewGenericInteraction", RoutingStrategy.Tunnel,
            typeof(UpfGenericInteractionEventHandler), typeof(Generic));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when an element is tapped or clicked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="GenericInteractionEvent"/></revtField>
        ///     <revtStylingName>generic-interaction</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfGenericInteractionEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent GenericInteractionEvent = EventManager.RegisterRoutedEvent("GenericInteraction", RoutingStrategy.Bubble,
            typeof(UpfGenericInteractionEventHandler), typeof(Generic));

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/> 
        /// attached event for the specified element.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.PreviewGenericInteraction"/> 
        /// attached routed event.</value>
        internal static void RaisePreviewGenericInteraction(DependencyObject element, UltravioletResource device, ref RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGenericInteractionEventHandler>(PreviewGenericInteractionEvent);
            evt?.Invoke(element, device, ref data);
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Generic.GenericInteraction"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseGenericInteraction(DependencyObject element, UltravioletResource device, ref RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfGenericInteractionEventHandler>(GenericInteractionEvent);
            evt?.Invoke(element, device, ref data);
        }
    }
}
