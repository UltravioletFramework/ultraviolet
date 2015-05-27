using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when the mouse cursor enters or leaves an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseEventHandler(DependencyObject element, MouseDevice device, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a button is pressed or released while an interface element is under the mouse.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="button">The mouse button that was pressed or released.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseButtonEventHandler(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when the mouse moves over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="dx">The difference between the x-coordinate of the mouse's 
    /// current position and the x-coordinate of the mouse's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the mouse's 
    /// current position and the y-coordinate of the mouse's previous position.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseMoveEventHandler(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when the mouse wheel is scrolled over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
    /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseWheelEventHandler(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data);

    /// <summary>
    /// Represents the mouse device.
    /// </summary>
    [UvmlKnownType]
    public static partial class Mouse
    {
        /// <summary>
        /// Captures the mouse within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set mouse capture.</param>
        /// <param name="element">The element to capture the mouse.</param>
        /// <returns><c>true</c> if the mouse was successfully captured; otherwise, <c>false</c>.</returns>
        public static Boolean Capture(PresentationFoundationView view, IInputElement element)
        {
            return Capture(view, element, CaptureMode.Element);
        }

        /// <summary>
        /// Captures the mouse within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set mouse capture.</param>
        /// <param name="element">The element to capture the mouse.</param>
        /// <param name="mode">The mouse capture mode.</param>
        /// <returns><c>true</c> if the mouse was successfully captured; otherwise, <c>false</c>.</returns>
        public static Boolean Capture(PresentationFoundationView view, IInputElement element, CaptureMode mode)
        {
            Contract.Require(view, "view");

            if (element != null)
            {
                return view.CaptureMouse(element, mode);
            }
            else
            {
                view.ReleaseMouse();
                return true;
            }
        }

        /// <summary>
        /// Gets the position of the mouse relative to the specified element.
        /// </summary>
        /// <param name="relativeTo">The element for which to retrieve relative mouse coordinates.</param>
        /// <returns>The position of the mouse relative to the specified element.</returns>
        public static Point2D GetPosition(IInputElement relativeTo)
        {
            var uiElement = relativeTo as UIElement;
            if (uiElement != null && uiElement.View != null)
            {
                var device = PrimaryDevice;
                var dipPos = uiElement.View.Display.PixelsToDips((Point2D)device.Position);
                var relPos = dipPos - uiElement.AbsolutePosition;

                return relPos;
            }
            return Point2D.Zero;
        }

        /// <summary>
        /// Gets the element which has captured the mouse within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which has captured the mouse within the specified view, 
        /// or <c>null</c> if no element has captured the mouse.</returns>
        public static IInputElement GetCaptured(PresentationFoundationView view)
        {
            Contract.Require(view, "view");

            return view.ElementWithMouseCapture;
        }

        /// <summary>
        /// Gets the element which the mouse cursor is directly over within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which is the mouse cursor is directly over within the specified view,
        /// or <c>null</c> if the mouse cursor is not over any element.</returns>
        public static IInputElement GetDirectlyOver(PresentationFoundationView view)
        {
            Contract.Require(view, "view");

            return view.ElementUnderMouse;
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseMove attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseDoubleClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewMouseWheel attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the GotMouseCapture attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the LostMouseCapture attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseMove attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseEnter attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseEnterHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseLeave attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseLeaveHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseDoubleClick attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the MouseWheel attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, MouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseMove attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseDoubleClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewMouseWheel attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GotMouseCapture attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the LostMouseCapture attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseMove attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseMoveHandler(DependencyObject element, MouseMoveEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseEnter attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseEnterHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseLeave attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseLeaveHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseDoubleClick attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the MouseWheel attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to Remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, MouseWheelEvent, handler);
        }

        /// <summary>
        /// Identifies the GotMouseCapture routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is got-mouse-capture.</remarks>
        public static readonly RoutedEvent GotMouseCaptureEvent = EventManager.RegisterRoutedEvent("GotMouseCapture", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the LostMouseCapture routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is lost-mouse-capture.</remarks>
        public static readonly RoutedEvent LostMouseCaptureEvent = EventManager.RegisterRoutedEvent("LostMouseCapture", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseMotion routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-move.</remarks>
        public static readonly RoutedEvent PreviewMouseMoveEvent = EventManager.RegisterRoutedEvent("PreviewMouseMove", RoutingStrategy.Tunnel,
            typeof(UpfMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-down.</remarks>
        public static readonly RoutedEvent PreviewMouseDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseDown", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-up.</remarks>
        public static readonly RoutedEvent PreviewMouseUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseUp", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseClick routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-click.</remarks>
        public static readonly RoutedEvent PreviewMouseClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseClick", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseDoubleClick routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-double.</remarks>
        public static readonly RoutedEvent PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the PreviewMouseWheel routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-mouse-wheel.</remarks>
        public static readonly RoutedEvent PreviewMouseWheelEvent = EventManager.RegisterRoutedEvent("PreviewMouseWheel", RoutingStrategy.Tunnel,
            typeof(UpfMouseWheelEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseMotion routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-move.</remarks>
        public static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent("MouseMove", RoutingStrategy.Bubble,
            typeof(UpfMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseEnter routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-enter.</remarks>
        public static readonly RoutedEvent MouseEnterEvent = EventManager.RegisterRoutedEvent("MouseEnter", RoutingStrategy.Direct,
            typeof(UpfMouseEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseLeave routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-leave.</remarks>
        public static readonly RoutedEvent MouseLeaveEvent = EventManager.RegisterRoutedEvent("MouseLeave", RoutingStrategy.Direct,
            typeof(UpfMouseEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseDown routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-down.</remarks>
        public static readonly RoutedEvent MouseDownEvent = EventManager.RegisterRoutedEvent("MouseDown", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseUp routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-up.</remarks>
        public static readonly RoutedEvent MouseUpEvent = EventManager.RegisterRoutedEvent("MouseUp", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseClick routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-click.</remarks>
        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent("MouseClick", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseDoubleClick routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-double-click.</remarks>
        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the MouseWheel routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is mouse-wheel.</remarks>
        public static readonly RoutedEvent MouseWheelEvent = EventManager.RegisterRoutedEvent("MouseWheel", RoutingStrategy.Bubble,
            typeof(UpfMouseWheelEventHandler), typeof(Mouse));

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
        internal static void RaiseGotMouseCapture(DependencyObject element, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(GotMouseCaptureEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }

        /// <summary>
        /// Raises the LostMouseCapture attached event for the specified element.
        /// </summary>
        internal static void RaiseLostMouseCapture(DependencyObject element, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(LostMouseCaptureEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseMove attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseMove(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseMoveEventHandler>(PreviewMouseMoveEvent);
            if (temp != null)
            {
                temp(element, device, x, y, dx, dy, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseDownEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseUp(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseClick attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseClick(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseDoubleClick attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDoubleClick(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseDoubleClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewMouseWheel attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseWheelEventHandler>(PreviewMouseWheelEvent);
            if (temp != null)
            {
                temp(element, device, x, y, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseMove attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseMove(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseMoveEventHandler>(MouseMoveEvent);
            if (temp != null)
            {
                temp(element, device, x, y, dx, dy, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseEnter attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseEnter(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseEventHandler>(MouseEnterEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseLeave attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseLeave(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseEventHandler>(MouseLeaveEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseDown attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDown(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseDownEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseUp attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseUp(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseUpEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseClick(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseDoubleClick attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDoubleClick(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseDoubleClickEvent);
            if (temp != null)
            {
                temp(element, device, button, ref data);
            }
        }

        /// <summary>
        /// Raises the MouseWheel attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfMouseWheelEventHandler>(MouseWheelEvent);
            if (temp != null)
            {
                temp(element, device, x, y, ref data);
            }
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<MouseState> mouseState = 
            new UltravioletSingleton<MouseState>((uv) => new MouseState(uv));
    }
}
