using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives a tap event from a touch device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
    /// <param name="x">The x-coordinate of the touch.</param>
    /// <param name="y">The y-coordinate of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchTapEventHandler(DependencyObject element, TouchDevice device, Int64 fingerID,
        Double x, Double y, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives an event from a touch device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
    /// <param name="x">The x-coordinate of the touch.</param>
    /// <param name="y">The y-coordinate of the touch.</param>
    /// <param name="pressure">The normalized pressure value of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchEventHandler(DependencyObject element, TouchDevice device, Int64 fingerID,
        Double x, Double y, Single pressure, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a UI element receives an event from a touch device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
    /// <param name="x">The x-coordinate of the touch.</param>
    /// <param name="y">The y-coordinate of the touch.</param>
    /// <param name="dx">The amount of motion along the x-axis.</param>
    /// <param name="dy">The amount of motion along the y-axis.</param>
    /// <param name="pressure">The normalized pressure value of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchMotionEventHandler(DependencyObject element, TouchDevice device, Int64 fingerID,
        Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data);

    /// <summary>
    /// Represents a touch device.
    /// </summary>
    [UvmlKnownType]
    public static partial class Touch
    {
        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewTapEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewFingerDownHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewFingerDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewFingerUpHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewFingerUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewFingerMotionHandler(DependencyObject element, UpfTouchMotionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewFingerMotionEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, TapEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddFingerDownHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, FingerDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddFingerUpHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, FingerUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddFingerMotionHandler(DependencyObject element, UpfTouchMotionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, FingerMotionEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewTapEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewFingerDownHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewFingerDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewFingerUpHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewFingerUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewFingerMotionHandler(DependencyObject element, UpfTouchMotionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewFingerMotionEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, TapEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveFingerDownHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, FingerDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveFingerUpHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, FingerUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveFingerMotionHandler(DependencyObject element, UpfTouchMotionEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, FingerMotionEvent, handler);
        }

        /// <summary>
        /// Gets the primary touch input device.
        /// </summary>
        public static TouchDevice PrimaryDevice
        {
            get { return touchState.Value.PrimaryDevice; }
        }

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element is tapped.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTapEvent"/></revtField>
        ///     <revtStylingName>preview-tap</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchTapEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTapEvent = EventManager.RegisterRoutedEvent("PreviewTap", RoutingStrategy.Tunnel,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is pressed against the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewFingerDownEvent"/></revtField>
        ///     <revtStylingName>preview-finger-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewFingerDownEvent = EventManager.RegisterRoutedEvent("PreviewFingerDown", RoutingStrategy.Tunnel,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is released from the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewFingerUpEvent"/></revtField>
        ///     <revtStylingName>preview-finger-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewFingerUpEvent = EventManager.RegisterRoutedEvent("PreviewFingerUp", RoutingStrategy.Tunnel,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is moved over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewFingerMotionEvent"/></revtField>
        ///     <revtStylingName>preview-finger-motion</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchMotionEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewFingerMotionEvent = EventManager.RegisterRoutedEvent("PreviewFingerMotion", RoutingStrategy.Tunnel,
            typeof(UpfTouchMotionEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element is tapped.
        /// </summary>
        /// </AttachedEventComments>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TapEvent"/></revtField>
        ///     <revtStylingName>tap</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchTapEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent("Tap", RoutingStrategy.Bubble,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is pressed against the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="FingerDownEvent"/></revtField>
        ///     <revtStylingName>finger-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent FingerDownEvent = EventManager.RegisterRoutedEvent("FingerDown", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is released from the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="FingerUpEvent"/></revtField>
        ///     <revtStylingName>finger-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent FingerUpEvent = EventManager.RegisterRoutedEvent("FingerUp", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a finger is moved over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="FingerMotionEvent"/></revtField>
        ///     <revtStylingName>finger-motion</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchMotionEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent FingerMotionEvent = EventManager.RegisterRoutedEvent("FingerMotion", RoutingStrategy.Bubble,
            typeof(UpfTouchMotionEventHandler), typeof(Touch));

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewTap"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTap(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchTapEventHandler>(PreviewTapEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewFingerUp(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(PreviewFingerUpEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, pressure, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewFingerDown(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(PreviewFingerDownEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, pressure, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.PreviewFingerMotion"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewFingerMotion(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchMotionEventHandler>(PreviewFingerMotionEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, dx, dy, pressure, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.Tap"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTap(DependencyObject element, TouchDevice device, Int64 fingerID, 
            Double x, Double y, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchTapEventHandler>(TapEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseFingerUp(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(FingerUpEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, pressure, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseFingerDown(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(FingerDownEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, pressure, ref data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TwistedLogik.Ultraviolet.UI.Presentation.Input.Touch.FingerMotion"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseFingerMotion(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfTouchMotionEventHandler>(FingerMotionEvent);
            if (temp != null)
            {
                temp(element, device, fingerID, x, y, dx, dy, pressure, ref data);
            }
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<TouchState> touchState =
            new UltravioletSingleton<TouchState>(uv => new TouchState(uv));
    }
}
