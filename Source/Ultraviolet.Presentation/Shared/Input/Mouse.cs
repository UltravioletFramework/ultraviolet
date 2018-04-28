using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when the mouse cursor enters or leaves an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseEventHandler(DependencyObject element, MouseDevice device, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a button is pressed or released while an interface element is under the mouse.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="button">The mouse button that was pressed or released.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseButtonEventHandler(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data);

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
    public delegate void UpfMouseMoveEventHandler(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when the mouse wheel is scrolled over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
    /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMouseWheelEventHandler(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called to determine which cursor to display.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfQueryCursorEventHandler(DependencyObject element, MouseDevice device, CursorQueryRoutedEventData data);

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
        /// <returns><see langword="true"/> if the mouse was successfully captured; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the mouse was successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean Capture(PresentationFoundationView view, IInputElement element, CaptureMode mode)
        {
            Contract.Require(view, nameof(view));

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
                var positionPixs = PrimaryDevice.GetPositionInWindow(uiElement.View.Window);
                if (positionPixs != null)
                {
                    var visualRoot = VisualTreeHelper.GetRoot(uiElement) as UIElement;
                    if (visualRoot == null)
                        return new Point2D(Double.NaN, Double.NaN);

                    var positionDips = uiElement.View.Display.PixelsToDips(positionPixs.Value);
                    
                    if (visualRoot is PopupRoot)
                    {
                        var popup = visualRoot.Parent as Popup;
                        if (popup == null)
                            return new Point2D(Double.NaN, Double.NaN);

                        positionDips = popup.ScreenToPopup(positionDips);
                    }
                    
                    return visualRoot.TransformToDescendant(uiElement, positionDips);
                }
            }
            return new Point2D(Double.NaN, Double.NaN);
        }

        /// <summary>
        /// Gets the element which has captured the mouse within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which has captured the mouse within the specified view, 
        /// or <see langword="null"/> if no element has captured the mouse.</returns>
        public static IInputElement GetCaptured(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            return view.ElementWithMouseCapture;
        }

        /// <summary>
        /// Gets the element which the mouse cursor is directly over within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which is the mouse cursor is directly over within the specified view,
        /// or <see langword="null"/> if the mouse cursor is not over any element.</returns>
        public static IInputElement GetDirectlyOver(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            return view.ElementUnderMouse;
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.QueryCursor"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddQueryCursorHandler(DependencyObject element, UpfQueryCursorEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, QueryCursorEvent, handler);
        }
        
        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.GotMouseCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.LostMouseCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseWheelEvent, handler);
        }
        
        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseEnter"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseEnterHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseLeave"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMouseLeaveHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.QueryCursor"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveQueryCursorHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, QueryCursorEvent, handler);
        }
        
        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.GotMouseCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, GotMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.LostMouseCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostMouseCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, LostMouseCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseMoveHandler(DependencyObject element, UpfMouseMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseMoveHandler(DependencyObject element, MouseMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseUpHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDownHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseDoubleClickHandler(DependencyObject element, UpfMouseButtonEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseWheelHandler(DependencyObject element, UpfMouseWheelEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseWheelEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseEnter"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseEnterHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseLeave"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMouseLeaveHandler(DependencyObject element, UpfMouseEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.QueryCursor"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.QueryCursor"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when an element is queried to determine which cursor should be displayed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="QueryCursorEvent"/></revtField>
        ///     <revtStylingName>query-cursor</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfQueryCursorEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent QueryCursorEvent = EventManager.RegisterRoutedEvent("QueryCursor", RoutingStrategy.Bubble,
            typeof(UpfQueryCursorEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.GotMouseCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.GotMouseCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element captures the mouse.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="GotMouseCaptureEvent"/></revtField>
        ///     <revtStylingName>got-mouse-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent GotMouseCaptureEvent = EventManager.RegisterRoutedEvent("GotMouseCapture", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.LostMouseCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.LostMouseCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element loses mouse capture.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="LostMouseCaptureEvent"/></revtField>
        ///     <revtStylingName>lost-mouse-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent LostMouseCaptureEvent = EventManager.RegisterRoutedEvent("LostMouseCapture", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse cursor moves over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseMoveEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-move</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseMoveEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseMoveEvent = EventManager.RegisterRoutedEvent("PreviewMouseMove", RoutingStrategy.Tunnel,
            typeof(UpfMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button enters the "down" state while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseDownEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseDown", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button enters the "up" state while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseUpEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseUp", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button is clicked while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseClickEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-click</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseClick", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button is double-clicked while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseDoubleClickEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-double-click</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/>.</description>
        ///     </item>
        /// </list>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Tunnel,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse wheel is scrolled while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMouseWheelEvent"/></revtField>
        ///     <revtStylingName>preview-mouse-wheel</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseWheelEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMouseWheelEvent = EventManager.RegisterRoutedEvent("PreviewMouseWheel", RoutingStrategy.Tunnel,
            typeof(UpfMouseWheelEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse cursor moves over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseMoveEvent"/></revtField>
        ///     <revtStylingName>mouse-move</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseMoveEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent("MouseMove", RoutingStrategy.Bubble,
            typeof(UpfMouseMoveEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button enters the "down" state while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseDownEvent"/></revtField>
        ///     <revtStylingName>mouse-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseDownEvent = EventManager.RegisterRoutedEvent("MouseDown", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button enters the "up" state while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseUpEvent"/></revtField>
        ///     <revtStylingName>mouse-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseUpEvent = EventManager.RegisterRoutedEvent("MouseUp", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button is clicked while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseClickEvent"/></revtField>
        ///     <revtStylingName>mouse-click</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent("MouseClick", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a mouse button is double clicked while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseDoubleClickEvent"/></revtField>
        ///     <revtStylingName>mouse-double-click</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseButtonEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble,
            typeof(UpfMouseButtonEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse wheel is scrolled while the cursor is over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseWheelEvent"/></revtField>
        ///     <revtStylingName>mouse-wheel</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseWheelEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseWheelEvent = EventManager.RegisterRoutedEvent("MouseWheel", RoutingStrategy.Bubble,
            typeof(UpfMouseWheelEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseEnter"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseEnter"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse cursor enters the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseEnterEvent"/></revtField>
        ///     <revtStylingName>mouse-enter</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseEnterEvent = EventManager.RegisterRoutedEvent("MouseEnter", RoutingStrategy.Direct,
            typeof(UpfMouseEventHandler), typeof(Mouse));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseLeave"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseLeave"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the mouse cursor leaves the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MouseLeaveEvent"/></revtField>
        ///     <revtStylingName>mouse-leave</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfMouseEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MouseLeaveEvent = EventManager.RegisterRoutedEvent("MouseLeave", RoutingStrategy.Direct,
            typeof(UpfMouseEventHandler), typeof(Mouse));

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
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.GotMouseCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseGotMouseCapture(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(GotMouseCaptureEvent);
            evt?.Invoke(element, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.LostMouseCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseLostMouseCapture(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(LostMouseCaptureEvent);
            evt?.Invoke(element, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseMove"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseMove(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseMoveEventHandler>(PreviewMouseMoveEvent);
            evt?.Invoke(element, device, x, y, dx, dy, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseDownEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseUp(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseUpEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseClick"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseClick(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseClickEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseDoubleClick"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseDoubleClick(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(PreviewMouseDoubleClickEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewMouseWheel"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseWheelEventHandler>(PreviewMouseWheelEvent);
            evt?.Invoke(element, device, x, y, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseMove"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseMove(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseMoveEventHandler>(MouseMoveEvent);
            evt?.Invoke(element, device, x, y, dx, dy, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseEnter"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseEnter(DependencyObject element, MouseDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseEventHandler>(MouseEnterEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseLeave"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseLeave(DependencyObject element, MouseDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseEventHandler>(MouseLeaveEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDown(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseDownEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseUp(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseUpEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseClick"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseClick(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseClickEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseDoubleClick"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseDoubleClick(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseButtonEventHandler>(MouseDoubleClickEvent);
            evt?.Invoke(element, device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Mouse.MouseWheel"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMouseWheel(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMouseWheelEventHandler>(MouseWheelEvent);
            evt?.Invoke(element, device, x, y, data);
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<MouseState> mouseState = 
            new UltravioletSingleton<MouseState>(uv => new MouseState(uv));
    }
}
