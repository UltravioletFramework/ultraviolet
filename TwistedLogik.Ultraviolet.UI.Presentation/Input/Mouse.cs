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
    public static class Mouse
    {
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
        /// Raises the GotMouseCapture attached event for the specified element.
        /// </summary>
        internal static void RaiseGotMouseCapture(UIElement element)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(GotMouseCaptureEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the LostMouseCapture attached event for the specified element.
        /// </summary>
        internal static void RaiseLostMouseCapture(UIElement element)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(LostMouseCaptureEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseMove attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseMove(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseMoveEventHandler>(MouseMoveEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, x, y, dx, dy, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseEnter attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseEnter(UIElement element, MouseDevice device)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseEventHandler>(MouseEnterEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseLeave attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseLeave(UIElement element, MouseDevice device)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseEventHandler>(MouseLeaveEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseDown attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDown(UIElement element, MouseDevice device, MouseButton button)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseDownEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseUp attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseUp(UIElement element, MouseDevice device, MouseButton button)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseUpEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseClick(UIElement element, MouseDevice device, MouseButton button)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseClickEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseDoubleClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDoubleClick(UIElement element, MouseDevice device, MouseButton button)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseButtonEventHandler>(MouseDoubleClickEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, button, ref handled);
            }
        }

        /// <summary>
        /// Raises the MouseWheel attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseWheel(UIElement element, MouseDevice device, Double x, Double y)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementMouseWheelEventHandler>(MouseWheelEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, x, y, ref handled);
            }
        }
    }
}
