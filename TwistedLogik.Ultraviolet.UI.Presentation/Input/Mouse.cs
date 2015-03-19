using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when the mouse cursor enters or leaves a UI element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementMouseEventHandler(UIElement element, MouseDevice device, ref Boolean handled);

    /// <summary>
    /// Represents the method that is called when a button is pressed or released while an element is under the mouse.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="button">The mouse button that was pressed or released.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementMouseButtonEventHandler(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled);

    /// <summary>
    /// Represents the method that is called when the mouse moves over a control.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="dx">The difference between the x-coordinate of the mouse's 
    /// current position and the x-coordinate of the mouse's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the mouse's 
    /// current position and the y-coordinate of the mouse's previous position.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementMouseMoveEventHandler(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled);

    /// <summary>
    /// Represents the method that is called when the mouse wheel is scrolled over a control.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
    /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementMouseWheelEventHandler(UIElement element, MouseDevice device, Double x, Double y, ref Boolean handled);

    /// <summary>
    /// Represents the mouse device.
    /// </summary>
    [UvmlKnownType]
    public static partial class Mouse
    {
        /// <summary>
        /// Adds a handler for the PreviewMouseMove attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseMoveHandler(UIElement element, UIElementMouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDownHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseUpHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseDoubleClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDoubleClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseWheel attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseWheelHandler(UIElement element, UIElementMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the GotMouseCapture attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotMouseCaptureHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the LostMouseCapture attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostMouseCaptureHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseMove attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseMoveHandler(UIElement element, MouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseEnter attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseEnterHandler(UIElement element, UIElementMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseEnterEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseLeave attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseLeaveHandler(UIElement element, UIElementMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseUpHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDownHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseDoubleClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDoubleClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseWheel attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseWheelHandler(UIElement element, UIElementMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(MouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseMove attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseMoveHandler(UIElement element, UIElementMouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDownHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseUpHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseDoubleClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDoubleClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseWheel attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseWheelHandler(UIElement element, UIElementMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GotMouseCapture attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotMouseCaptureHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the LostMouseCapture attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostMouseCaptureHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseMove attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseMoveHandler(UIElement element, MouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseEnter attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseEnterHandler(UIElement element, UIElementMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseEnterEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseLeave attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseLeaveHandler(UIElement element, UIElementMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseUpHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDownHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseDoubleClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDoubleClickHandler(UIElement element, UIElementMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseWheel attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseWheelHandler(UIElement element, UIElementMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(MouseWheelEvent, handler);
        }

        /// <summary>
        /// Identifies the GotMouseCapture routed event.
        /// </summary>
        public static readonly RoutedEvent GotMouseCaptureEvent = RoutedEvent.Register("GotMouseCapture", RoutingStrategy.Direct,
            typeof(UIElementRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the LostMouseCapture routed event.
        /// </summary>
        public static readonly RoutedEvent LostMouseCaptureEvent = RoutedEvent.Register("LostMouseCapture", RoutingStrategy.Direct,
            typeof(UIElementRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseMotion routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseMoveEvent = RoutedEvent.Register("PreviewMouseMove", RoutingStrategy.Tunnel,
            typeof(UIElementMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseDown routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseDownEvent = RoutedEvent.Register("PreviewMouseDown", RoutingStrategy.Tunnel,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseUp routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseUpEvent = RoutedEvent.Register("PreviewMouseUp", RoutingStrategy.Tunnel,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseClick routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseClickEvent = RoutedEvent.Register("PreviewMouseClick", RoutingStrategy.Tunnel,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseDoubleClick routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseDoubleClickEvent = RoutedEvent.Register("PreviewMouseDoubleClick", RoutingStrategy.Tunnel,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseWheel routed event.
        /// </summary>
        public static readonly RoutedEvent PreviewMouseWheelEvent = RoutedEvent.Register("PreviewMouseWheel", RoutingStrategy.Tunnel,
            typeof(UIElementMouseWheelEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseMotion routed event.
        /// </summary>
        public static readonly RoutedEvent MouseMoveEvent = RoutedEvent.Register("MouseMove", RoutingStrategy.Bubble,
            typeof(UIElementMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseEnter routed event.
        /// </summary>
        public static readonly RoutedEvent MouseEnterEvent = RoutedEvent.Register("MouseEnter", RoutingStrategy.Direct,
            typeof(UIElementMouseEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseLeave routed event.
        /// </summary>
        public static readonly RoutedEvent MouseLeaveEvent = RoutedEvent.Register("MouseLeave", RoutingStrategy.Direct,
            typeof(UIElementMouseEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseDown routed event.
        /// </summary>
        public static readonly RoutedEvent MouseDownEvent = RoutedEvent.Register("MouseDown", RoutingStrategy.Bubble,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseUp routed event.
        /// </summary>
        public static readonly RoutedEvent MouseUpEvent = RoutedEvent.Register("MouseUp", RoutingStrategy.Bubble,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseClick routed event.
        /// </summary>
        public static readonly RoutedEvent MouseClickEvent = RoutedEvent.Register("MouseClick", RoutingStrategy.Bubble,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseDoubleClick routed event.
        /// </summary>
        public static readonly RoutedEvent MouseDoubleClickEvent = RoutedEvent.Register("MouseDoubleClick", RoutingStrategy.Bubble,
            typeof(UIElementMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseWheel routed event.
        /// </summary>
        public static readonly RoutedEvent MouseWheelEvent = RoutedEvent.Register("MouseWheel", RoutingStrategy.Bubble,
            typeof(UIElementMouseWheelEventHandler), typeof(Mouse));

        /// <summary>
        /// Gets the primary mouse input device.
        /// </summary>
        public static MouseDevice PrimaryDevice
        {
            get { return mouseState.Value.PrimaryDevice; }
        }

        /// <summary>
        /// Gets the state of the mouse's left button.
        /// </summary>
        public static MouseButtonState LeftButton
        {
            get { return PrimaryDevice.IsButtonDown(MouseButton.Left) ? MouseButtonState.Pressed : MouseButtonState.Released; }
        }

        /// <summary>
        /// Gets the state of the mouse's middle button.
        /// </summary>
        public static MouseButtonState MiddleButton
        {
            get { return PrimaryDevice.IsButtonDown(MouseButton.Middle) ? MouseButtonState.Pressed : MouseButtonState.Released; }
        }

        /// <summary>
        /// Gets the state of the mouse's right button.
        /// </summary>
        public static MouseButtonState RightButton
        {
            get { return PrimaryDevice.IsButtonDown(MouseButton.Right) ? MouseButtonState.Pressed : MouseButtonState.Released; }
        }

        /// <summary>
        /// Gets the state of the mouse's first extended button.
        /// </summary>
        public static MouseButtonState XButton1
        {
            get { return PrimaryDevice.IsButtonDown(MouseButton.XButton1) ? MouseButtonState.Pressed : MouseButtonState.Released; }
        }

        /// <summary>
        /// Gets the state of the mouse's second extended button.
        /// </summary>
        public static MouseButtonState XButton2
        {
            get { return PrimaryDevice.IsButtonDown(MouseButton.XButton2) ? MouseButtonState.Pressed : MouseButtonState.Released; }
        }

        /// <summary>
        /// Raises the GotMouseCapture attached event for the specified element.
        /// </summary>
        internal static void RaiseGotMouseCapture(UIElement element, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(GotMouseCaptureEvent);
            if (temp != null)
            {
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the LostMouseCapture attached event for the specified element.
        /// </summary>
        internal static void RaiseLostMouseCapture(UIElement element, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(LostMouseCaptureEvent);
            if (temp != null)
            {
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseMove attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseMove(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseMoveEventHandler>(PreviewMouseMoveEvent);
            if (temp != null)
            {
                temp(element, device, x, y, dx, dy, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDown(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(PreviewMouseDownEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseUp(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(PreviewMouseUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseClick attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseClick(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(PreviewMouseClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseDoubleClick attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDoubleClick(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(PreviewMouseDoubleClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseWheel attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseWheel(UIElement element, MouseDevice device, Double x, Double y, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseWheelEventHandler>(PreviewMouseWheelEvent);
            if (temp != null)
            {
                temp(element, device, x, y, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseMove attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseMove(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseMoveEventHandler>(MouseMoveEvent);
            if (temp != null)
            {
                temp(element, device, x, y, dx, dy, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseEnter attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseEnter(UIElement element, MouseDevice device, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseEventHandler>(MouseEnterEvent);
            if (temp != null)
            {
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseLeave attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseLeave(UIElement element, MouseDevice device, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseEventHandler>(MouseLeaveEvent);
            if (temp != null)
            {
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseDown attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDown(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseDownEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseUp attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseUp(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseClick(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseDoubleClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDoubleClick(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseDoubleClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseWheel attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseWheel(UIElement element, MouseDevice device, Double x, Double y, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseWheelEventHandler>(MouseWheelEvent);
            if (temp != null)
            {
                temp(element, device, x, y, ref handled);
            }
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<MouseState> mouseState = 
            new UltravioletSingleton<MouseState>((uv) => new MouseState(uv));
    }
}
