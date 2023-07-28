using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a touch enters or leaves an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchEventHandler(DependencyObject element, TouchDevice device, 
        Int64 id, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a touch moves over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="dx">The difference between the x-coordinate of the touch's 
    /// current position and the x-coordinate of the touch's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the touch's 
    /// current position and the y-coordinate of the touch's previous position.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchMoveEventHandler(DependencyObject element, TouchDevice device, 
        Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a touch begins over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchDownEventHandler(DependencyObject element, TouchDevice device,
        Int64 id, Double x, Double y, Single pressure, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a touch ends over an interface element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchUpEventHandler(DependencyObject element, TouchDevice device,
        Int64 id, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when an element is tapped.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchTapEventHandler(DependencyObject element, TouchDevice device, 
        Int64 id, Double x, Double y, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when an element is long pressed.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="id">The unique identifier of the touch.</param>
    /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfTouchLongPressEventHandler(DependencyObject element, TouchDevice device,
        Int64 id, Double x, Double y, Single pressure, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a multi-finger gesture is performed.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="x">The x-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
    /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
    /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
    /// <param name="fingers">The number of fingers involved in the gesture.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfMultiGestureEventHandler(DependencyObject element, TouchDevice device,
        Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a $1 gesture is performed.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The touch device.</param>
    /// <param name="gestureID">The unique identifier of the gesture which was performed.</param>
    /// <param name="x">The x-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
    /// <param name="error">The difference between the gesture template and the actual performed gesture; lower is better.</param>
    /// <param name="fingers">The number of fingers used to perform the gesture.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfDollarGestureEventHandler(DependencyObject element, TouchDevice device,
        Int64 gestureID, Double x, Double y, Single error, Int32 fingers, RoutedEventData data);

    /// <summary>
    /// Represents the touch device.
    /// </summary>
    [UvmlKnownType]
    public static partial class Touch
    {
        /// <summary>
        /// Captures a touch within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture the touch.</param>
        /// <param name="touchID">The unique identifier of the touch to capture.</param>
        /// <returns><see langword="true"/> if the touch was successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean Capture(PresentationFoundationView view, IInputElement element, Int64 touchID)
        {
            return Capture(view, element, touchID, CaptureMode.Element);
        }

        /// <summary>
        /// Captures a touch within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture the touch.</param>
        /// <param name="touchID">The unique identifier of the touch to capture.</param>
        /// <param name="mode">The touch capture mode.</param>
        /// <returns><see langword="true"/> if the touch was successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean Capture(PresentationFoundationView view, IInputElement element, Int64 touchID, CaptureMode mode)
        {
            Contract.Require(view, nameof(view));

            if (element != null)
            {
                return view.CaptureTouch(element, touchID, mode);
            }
            else
            {
                view.ReleaseTouch(touchID);
                return true;
            }
        }

        /// <summary>
        /// Captures all active touches within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture touches.</param>
        /// <returns><see langword="true"/> if the touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean CaptureAll(PresentationFoundationView view, IInputElement element)
        {
            return CaptureAll(view, element, CaptureMode.Element);
        }

        /// <summary>
        /// Captures all active touches within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture touches.</param>
        /// <param name="mode">The touch capture mode.</param>
        /// <returns><see langword="true"/> if the touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean CaptureAll(PresentationFoundationView view, IInputElement element, CaptureMode mode)
        {
            Contract.Require(view, nameof(view));

            if (element != null)
            {
                return view.CaptureAllTouches(element, mode);
            }
            else
            {
                view.ReleaseAllTouches();
                return true;
            }
        }

        /// <summary>
        /// Captures new touches within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture new touches.</param>
        /// <returns><see langword="true"/> if new touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean CaptureNew(PresentationFoundationView view, IInputElement element)
        {
            return CaptureNew(view, element, CaptureMode.Element);
        }

        /// <summary>
        /// Captures new touches within the specified input element.
        /// </summary>
        /// <param name="view">The view for which to set touch capture.</param>
        /// <param name="element">The element to capture new touches.</param>
        /// <param name="mode">The touch capture mode.</param>
        /// <returns><see langword="true"/> if new touches were successfully captured; otherwise, <see langword="false"/>.</returns>
        public static Boolean CaptureNew(PresentationFoundationView view, IInputElement element, CaptureMode mode)
        {
            Contract.Require(view, nameof(view));

            if (element != null)
            {
                return view.CaptureNewTouches(element, mode);
            }
            else
            {
                view.ReleaseNewTouches();
                return true;
            }
        }

        /// <summary>
        /// Gets the position of a touch relative to the specified element.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <param name="relativeTo">The element for which to retrieve relative touch coordinates.</param>
        /// <returns>The position of the touch relative to the specified element.</returns>
        public static Point2D GetPosition(Int64 touchID, IInputElement relativeTo)
        {
            var uiElement = relativeTo as UIElement;
            if (uiElement != null && uiElement.View != null)
            {
                TouchInfo touchInfo;
                if (!PrimaryDevice.TryGetTouch(touchID, out touchInfo))
                    return new Point2D(Double.NaN, Double.NaN);

                var visualRoot = VisualTreeHelper.GetRoot(uiElement) as UIElement;
                if (visualRoot == null)
                    return new Point2D(Double.NaN, Double.NaN);

                var positionPixs = PrimaryDevice.DenormalizeCoordinates(new Point2F(touchInfo.CurrentX, touchInfo.CurrentY));
                var positionDips = uiElement.View.Display.PixelsToDips(positionPixs);

                if (visualRoot is PopupRoot)
                {
                    var popup = visualRoot.Parent as Popup;
                    if (popup == null)
                        return new Point2D(Double.NaN, Double.NaN);

                    positionDips = popup.ScreenToPopup(positionDips);
                }

                return visualRoot.TransformToDescendant(uiElement, positionDips);
            }
            return new Point2D(Double.NaN, Double.NaN);
        }

        /// <summary>
        /// Gets the element which has captured a touch within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns>The element which has captured the touch within the specified view, 
        /// or <see langword="null"/> if no element has captured the touch.</returns>
        public static IInputElement GetCaptured(PresentationFoundationView view, Int64 touchID)
        {
            Contract.Require(view, nameof(view));

            return view.GetElementWithTouchCapture(touchID);
        }

        /// <summary>
        /// Gets the element which has captured new touches within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which has captured new touches within the specified view, 
        /// or <see langword="null"/> if no element has captured new touches.</returns>
        public static IInputElement GetCapturedNew(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            return view.ElementWithNewTouchCapture;
        }

        /// <summary>
        /// Gets the element which a touch is directly over within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns>The element which is the touch is directly over within the specified view,
        /// or <see langword="null"/> if the touch is not over any element.</returns>
        public static IInputElement GetDirectlyOver(PresentationFoundationView view, Int64 touchID)
        {
            Contract.Require(view, nameof(view));

            return view.GetElementUnderTouch(touchID);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotTouchCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotTouchCaptureHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, GotTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotNewTouchCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotNewTouchCaptureHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, GotNewTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostTouchCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostTouchCaptureHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, LostTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostNewTouchCapture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostNewTouchCaptureHandler(DependencyObject element, UpfTouchEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, LostNewTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchMove"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTouchMoveHandler(DependencyObject element, UpfTouchMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTouchMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTouchDownHandler(DependencyObject element, UpfTouchDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTouchDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTouchUpHandler(DependencyObject element, UpfTouchUpEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTouchUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTouchTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTouchTapEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongPress"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTouchLongPressHandler(DependencyObject element, UpfTouchLongPressEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTouchTapEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewMultiGestureHandler(DependencyObject element, UpfMultiGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewMultiGestureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewDollarGestureHandler(DependencyObject element, UpfDollarGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewDollarGestureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTouchMoveHandler(DependencyObject element, UpfTouchMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TouchMoveEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTouchDownHandler(DependencyObject element, UpfTouchDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TouchDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTouchUpHandler(DependencyObject element, UpfTouchUpEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TouchUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTouchTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TouchTapEvent, handler);
        }
        
        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPressEvent"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTouchLongPressHandler(DependencyObject element, UpfMultiGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TouchLongPressEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddMultiGestureHandler(DependencyObject element, UpfMultiGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, MultiGestureEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddDollarGestureHandler(DependencyObject element, UpfDollarGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, DollarGestureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotTouchCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotTouchCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, GotTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotNewTouchCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotNewTouchCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, GotNewTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostTouchCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostTouchCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, LostTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostNewTouchCapture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element to which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostNewTouchCaptureHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, LostNewTouchCaptureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchMove"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTouchMoveHandler(DependencyObject element, UpfTouchMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTouchMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTouchDownHandler(DependencyObject element, UpfTouchDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTouchDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTouchUpHandler(DependencyObject element, UpfTouchUpEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTouchUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTouchTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTouchTapEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongPress"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTouchLongPressHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTouchLongPressEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewMultiGestureHandler(DependencyObject element, UpfMultiGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewMultiGestureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewDollarGestureHandler(DependencyObject element, UpfDollarGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewDollarGestureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTouchMoveHandler(DependencyObject element, UpfTouchMoveEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TouchMoveEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTouchDownHandler(DependencyObject element, UpfTouchDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TouchDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTouchUpHandler(DependencyObject element, UpfTouchUpEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TouchUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTouchTapHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TouchTapEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPress"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTouchLongPressHandler(DependencyObject element, UpfTouchTapEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TouchLongPressEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveMultiGestureHandler(DependencyObject element, UpfMultiGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, MultiGestureEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveDollarGestureHandler(DependencyObject element, UpfDollarGestureEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, DollarGestureEvent, handler);
        }

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotTouchCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotTouchCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element captures a touch.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="GotTouchCaptureEvent"/></revtField>
        ///     <revtStylingName>got-touch-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent GotTouchCaptureEvent = EventManager.RegisterRoutedEvent("GotTouchCapture", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotNewTouchCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotNewTouchCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element captures new touches.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="GotNewTouchCaptureEvent"/></revtField>
        ///     <revtStylingName>got-new-touch-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent GotNewTouchCaptureEvent = EventManager.RegisterRoutedEvent("GotNewTouchCapture", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostTouchCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostTouchCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element loses touch capture.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="LostTouchCaptureEvent"/></revtField>
        ///     <revtStylingName>lost-touch-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent LostTouchCaptureEvent = EventManager.RegisterRoutedEvent("LostTouchCapture", RoutingStrategy.Bubble,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostNewTouchCapture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostNewTouchCapture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element loses new touch capture.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="LostNewTouchCaptureEvent"/></revtField>
        ///     <revtStylingName>lost-new-touch-capture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent LostNewTouchCaptureEvent = EventManager.RegisterRoutedEvent("LostNewTouchCapture", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchMove"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Mouse.PreviewTouchMove"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch moves over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTouchMoveEvent"/></revtField>
        ///     <revtStylingName>preview-touch-move</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchMoveEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTouchMoveEvent = EventManager.RegisterRoutedEvent("PreviewTouchMove", RoutingStrategy.Tunnel,
            typeof(UpfTouchMoveEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch begins over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTouchDownEvent"/></revtField>
        ///     <revtStylingName>preview-touch-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTouchDownEvent = EventManager.RegisterRoutedEvent("PreviewTouchDown", RoutingStrategy.Tunnel,
            typeof(UpfTouchDownEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch ends over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTouchUpEvent"/></revtField>
        ///     <revtStylingName>preview-touch-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTouchUpEvent = EventManager.RegisterRoutedEvent("PreviewTouchUp", RoutingStrategy.Tunnel,
            typeof(UpfTouchUpEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs the element is tapped.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTouchTapEvent"/></revtField>
        ///     <revtStylingName>preview-touch-tap</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchTapEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTouchTapEvent = EventManager.RegisterRoutedEvent("PreviewTouchTap", RoutingStrategy.Tunnel,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongTap"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongTap"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs the element is long pressed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTouchLongPressEvent"/></revtField>
        ///     <revtStylingName>preview-touch-long-press</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchLongPressEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPress"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTouchLongPressEvent = EventManager.RegisterRoutedEvent("PreviewTouchLongPress", RoutingStrategy.Tunnel,
            typeof(UpfTouchLongPressEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a multiple-finger gesture is performed over an element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewMultiGestureEvent"/></revtField>
        ///     <revtStylingName>preview-multi-gesture</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMultiGestureEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewMultiGestureEvent = EventManager.RegisterRoutedEvent("PreviewMultiGesture", RoutingStrategy.Tunnel,
            typeof(UpfMultiGestureEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a $1 gesture is performed over an element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewDollarGestureEvent"/></revtField>
        ///     <revtStylingName>preview-dollar-gesture</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfDollarGestureEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewDollarGestureEvent = EventManager.RegisterRoutedEvent("PreviewDollarGesture", RoutingStrategy.Tunnel,
            typeof(UpfDollarGestureEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch moves over the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchMoveEvent"/></revtField>
        ///     <revtStylingName>touch-move</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchMoveEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchMove"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchMoveEvent = EventManager.RegisterRoutedEvent("TouchMove", RoutingStrategy.Bubble,
            typeof(UpfTouchMoveEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch begins over an element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchDownEvent"/></revtField>
        ///     <revtStylingName>touch-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchDownEvent = EventManager.RegisterRoutedEvent("TouchDown", RoutingStrategy.Bubble,
            typeof(UpfTouchDownEventHandler), typeof(Touch));
        
        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch ends over an element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchUpEvent"/></revtField>
        ///     <revtStylingName>touch-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchUpEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchUpEvent = EventManager.RegisterRoutedEvent("TouchUp", RoutingStrategy.Bubble,
            typeof(UpfTouchUpEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element is tapped.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchTapEvent"/></revtField>
        ///     <revtStylingName>touch-tap</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchTapEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchTapEvent = EventManager.RegisterRoutedEvent("TouchTap", RoutingStrategy.Bubble,
            typeof(UpfTouchTapEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPress"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPress"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element is long pressed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchLongPressEvent"/></revtField>
        ///     <revtStylingName>touch-long-press</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchLongPressEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongPress"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchLongPressEvent = EventManager.RegisterRoutedEvent("TouchLongPress", RoutingStrategy.Bubble,
            typeof(UpfTouchLongPressEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a multiple-finger gesture is performed over an element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="MultiGestureEvent"/></revtField>
        ///     <revtStylingName>multi-gesture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfMultiGestureEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent MultiGestureEvent = EventManager.RegisterRoutedEvent("MultiGesture", RoutingStrategy.Bubble,
            typeof(UpfMultiGestureEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a $1 gesture is performed over a view.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="DollarGestureEvent"/></revtField>
        ///     <revtStylingName>dollar-gesture</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfDollarGestureEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent DollarGestureEvent = EventManager.RegisterRoutedEvent("DollarGesture", RoutingStrategy.Bubble,
            typeof(UpfDollarGestureEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchEnter"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchEnter"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch enters the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchEnterEvent"/></revtField>
        ///     <revtStylingName>touch-enter</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchEnterEvent = EventManager.RegisterRoutedEvent("TouchEnter", RoutingStrategy.Direct,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLeave"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLeave"/> 
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a touch leaves the element.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TouchLeaveEvent"/></revtField>
        ///     <revtStylingName>touch-leave</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfTouchEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TouchLeaveEvent = EventManager.RegisterRoutedEvent("TouchLeave", RoutingStrategy.Direct,
            typeof(UpfTouchEventHandler), typeof(Touch));

        /// <summary>
        /// Gets the primary touch input device.
        /// </summary>
        public static TouchDevice PrimaryDevice
        {
            get
            {
                var uv = UltravioletContext.DemandCurrent();
                return uv.GetInput().GetPrimaryTouchDevice();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotTouchCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseGotTouchCapture(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(GotTouchCaptureEvent);
            evt?.Invoke(element, device, id, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.GotNewTouchCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseGotNewTouchCapture(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(GotNewTouchCaptureEvent);
            evt?.Invoke(element, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostTouchCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseLostTouchCapture(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(LostTouchCaptureEvent);
            evt?.Invoke(element, device, id, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.LostNewTouchCapture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseLostNewTouchCapture(DependencyObject element, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(LostNewTouchCaptureEvent);
            evt?.Invoke(element, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchMove"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTouchMove(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchMoveEventHandler>(PreviewTouchMoveEvent);
            evt?.Invoke(element, device, id, x, y, dx, dy, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTouchDown(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchDownEventHandler>(PreviewTouchDownEvent);
            evt?.Invoke(element, device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTouchUp(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchUpEventHandler>(PreviewTouchUpEvent);
            evt?.Invoke(element, device, id, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchTap"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTouchTap(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchTapEventHandler>(PreviewTouchTapEvent);
            evt?.Invoke(element, device, id, x, y, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewTouchLongPress"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTouchLongPress(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchLongPressEventHandler>(PreviewTouchLongPressEvent);
            evt?.Invoke(element, device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewMultiGesture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewMultiGesture(DependencyObject element, TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMultiGestureEventHandler>(PreviewMultiGestureEvent);
            evt?.Invoke(element, device, x, y, theta, distance, fingers, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.PreviewDollarGesture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewDollarGesture(DependencyObject element, TouchDevice device,
            Int64 gestureID, Double x, Double y, Single error, Int32 fingers, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfDollarGestureEventHandler>(PreviewDollarGestureEvent);
            evt?.Invoke(element, device, gestureID, x, y, error, fingers, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchMove"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchMove(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchMoveEventHandler>(TouchMoveEvent);
            evt?.Invoke(element, device, id, x, y, dx, dy, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchDown(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchDownEventHandler>(TouchDownEvent);
            evt?.Invoke(element, device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchUp"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchUp(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchUpEventHandler>(TouchUpEvent);
            evt?.Invoke(element, device, id, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchTap"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchTap(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchTapEventHandler>(TouchTapEvent);
            evt?.Invoke(element, device, id, x, y, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLongPress"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchLongPress(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchLongPressEventHandler>(TouchLongPressEvent);
            evt?.Invoke(element, device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.MultiGesture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseMultiGesture(DependencyObject element, TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfMultiGestureEventHandler>(MultiGestureEvent);
            evt?.Invoke(element, device, x, y, theta, distance, fingers, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.DollarGesture"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseDollarGesture(DependencyObject element, TouchDevice device,
            Int64 gestureID, Double x, Double y, Single error, Int32 fingers, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfDollarGestureEventHandler>(DollarGestureEvent);
            evt?.Invoke(element, device, gestureID, x, y, error, fingers, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchEnter"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchEnter(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(TouchEnterEvent);
            evt?.Invoke(element, device, id, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Touch.TouchLeave"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTouchLeave(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfTouchEventHandler>(TouchLeaveEvent);
            evt?.Invoke(element, device, id, data);
        }
    }
}
