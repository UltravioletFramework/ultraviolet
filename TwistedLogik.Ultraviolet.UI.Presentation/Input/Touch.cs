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
        /// Adds a handler for the PreviewTap attached event to the specified element.
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
        /// Adds a handler for the PreviewFingerDown attached event to the specified element.
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
        /// Adds a handler for the PreviewFingerUp attached event to the specified element.
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
        /// Adds a handler for the PreviewFingerMotion attached event to the specified element.
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
        /// Adds a handler for the Tap attached event to the specified element.
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
        /// Adds a handler for the FingerDown attached event to the specified element.
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
        /// Adds a handler for the FingerUp attached event to the specified element.
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
        /// Adds a handler for the FingerMotion attached event to the specified element.
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
        /// Removes a handler for the PreviewTap attached event to the specified element.
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
        /// Removes a handler for the PreviewFingerDown attached event to the specified element.
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
        /// Removes a handler for the PreviewFingerUp attached event to the specified element.
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
        /// Removes a handler for the PreviewFingerMotion attached event to the specified element.
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
        /// Removes a handler for the Tap attached event from the specified element.
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
        /// Removes a handler for the FingerDown attached event to the specified element.
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
        /// Removes a handler for the FingerUp attached event to the specified element.
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
        /// Removes a handler for the FingerMotion attached event to the specified element.
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
        /// Identifies the PreviewTap attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is tap.</remarks>
        public static readonly RoutedEvent PreviewTapEvent = EventManager.RegisterRoutedEvent("PreviewTap", RoutingStrategy.Tunnel,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the PreviewFingerDown attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-finger-down.</remarks>
        public static readonly RoutedEvent PreviewFingerDownEvent = EventManager.RegisterRoutedEvent("PreviewFingerDown", RoutingStrategy.Tunnel,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the PreviewFingerUp attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-finger-up.</remarks>
        public static readonly RoutedEvent PreviewFingerUpEvent = EventManager.RegisterRoutedEvent("PreviewFingerUp", RoutingStrategy.Tunnel,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the PreviewFingerMotion attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-finger-motion.</remarks>
        public static readonly RoutedEvent PreviewFingerMotionEvent = EventManager.RegisterRoutedEvent("PreviewFingerMotion", RoutingStrategy.Tunnel,
            typeof(UpfTouchMotionEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the Tap attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is tap.</remarks>
        public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent("Tap", RoutingStrategy.Bubble,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the FingerUp attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is finger-up.</remarks>
        public static readonly RoutedEvent FingerUpEvent = EventManager.RegisterRoutedEvent("FingerUp", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the FingerDown attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is finger-down.</remarks>
        public static readonly RoutedEvent FingerDownEvent = EventManager.RegisterRoutedEvent("FingerDown", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the FingerMotion attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is finger-motion.</remarks>
        public static readonly RoutedEvent FingerMotionEvent = EventManager.RegisterRoutedEvent("FingerMotion", RoutingStrategy.Bubble,
            typeof(UpfTouchMotionEventHandler), typeof(Touch));

        /// <summary>
        /// Raises the PreviewTap attached event for the specified element.
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
        /// Raises the PreviewFingerUp attached event for the specified element.
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
        /// Raises the PreviewFingerDown attached event for the specified element.
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
        /// Raises the PreviewFingerMotion attached event for the specified element.
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
        /// Raises the Tap attached event for the specified element.
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
        /// Raises the FingerUp attached event for the specified element.
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
        /// Raises the FingerDown attached event for the specified element.
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
        /// Raises the FingerMotion attached event for the specified element.
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
            new UltravioletSingleton<TouchState>((uv) => new TouchState(uv));
    }
}
