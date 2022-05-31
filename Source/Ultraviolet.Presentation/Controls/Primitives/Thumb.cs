using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the method that is called when the user begins dragging a <see cref="Thumb"/> element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="hoffset">The horizontal offset at which the thumb was clicked, in relative coordinates.</param>
    /// <param name="voffset">The vertical offset at which the thumb was clicked, in relative coordinates.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfDragStartedEventHandler(DependencyObject element, Double hoffset, Double voffset, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when the position of a <see cref="Thumb"/> element changes during a drag operation.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="hchange">The distance that the thumb moved horizontally.</param>
    /// <param name="vchange">The distance that the thumb moved vertically.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfDragDeltaEventHandler(DependencyObject element, Double hchange, Double vchange, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when the user finishes dragging a <see cref="Thumb"/> element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="hchange">The total distance that the thumb moved horizontally.</param>
    /// <param name="vchange">The total distance that the thumb moved vertically.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfDragCompletedEventHandler(DependencyObject element, Double hchange, Double vchange, RoutedEventData data);

    /// <summary>
    /// Represents the thumb used to drag the position of a track.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Primitives.Templates.Thumb.xml")]
    public class Thumb : Control
    {
        /// <summary>
        /// Initializes the <see cref="Thumb"/> type.
        /// </summary>
        static Thumb()
        {
            FocusableProperty.OverrideMetadata(typeof(Thumb), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));
            IsEnabledProperty.OverrideMetadata(typeof(Thumb), new PropertyMetadata<Boolean>(HandleIsEnabledChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thumb"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Thumb(UltravioletContext uv, String name)
            : base(uv, name)
        {
            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <summary>
        /// Cancels the current dragging operation.
        /// </summary>
        public void CancelDrag()
        {
            if (!IsDragging)
                return;

            if (dragCursorID == 0)
            {
                ReleaseMouseCapture();
            }
            else
            {
                ReleaseTouchCapture(dragCursorID);
            }
            dragCursorID = 0;

            IsDragging = false;
            RaiseDragCompleted(dragPosLast.X - dragPosOriginAbs.X, dragPosLast.Y - dragPosOriginAbs.Y);
        }

        /// <summary>
        /// Gets a value indicating whether the thumb is currently being dragged.
        /// </summary>
        public Boolean IsDragging
        {
            get { return GetValue<Boolean>(IsDraggingProperty); }
            protected set { SetValue(IsDraggingPropertyKey, value); }
        }

        /// <summary>
        /// Occurs when a drag operation starts.
        /// </summary>
        public event UpfDragStartedEventHandler DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }

        /// <summary>
        /// Occurs when the thumb moves as the result of a drag operation.
        /// </summary>
        public event UpfDragDeltaEventHandler DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }

        /// <summary>
        /// Occurs when a drag operation is completed.
        /// </summary>
        public event UpfDragCompletedEventHandler DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="IsDragging"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(Boolean), typeof(Thumb),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleIsDraggingChanged));

        /// <summary>
        /// Identifies the <see cref="IsDragging"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDragging"/> dependency property.</value>
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="DragStarted"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="DragStarted"/> routed event.</value>
        public static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble,
            typeof(UpfDragStartedEventHandler), typeof(Thumb));

        /// <summary>
        /// Identifies the <see cref="DragDelta"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="DragDelta"/> routed event.</value>
        public static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble,
            typeof(UpfDragDeltaEventHandler), typeof(Thumb));

        /// <summary>
        /// Identifies the <see cref="DragCompleted"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="DragCompleted"/> routed event.</value>
        public static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble,
            typeof(UpfDragCompletedEventHandler), typeof(Thumb));

        /// <summary>
        /// Occurs when the value of the <see cref="IsDragging"/> property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="IsDragging"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsDragging"/> property.</param>
        protected virtual void OnDraggingChanged(Boolean oldValue, Boolean newValue)
        {

        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(RoutedEventData data)
        {
            HandleCursorUp(0);

            base.OnLostMouseCapture(data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                Focus();
                CaptureMouse();

                HandleCursorDown(0);

                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left && IsMouseCaptured && IsDragging)
            {
                HandleCursorUp(0);
                ReleaseMouseCapture();

                data.Handled = true;
            }
            base.OnMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (IsDragging)
            {
                HandleCursorMove(0);

                data.Handled = true;
            }
            base.OnMouseMove(device, x, y, dx, dy, data);
        }
        
        /// <inheritdoc/>
        protected override void OnLostTouchCapture(TouchDevice device, Int64 id, RoutedEventData data)
        {
            HandleCursorUp(id);

            base.OnLostTouchCapture(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchDown(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            Focus();
            CaptureTouch(id);

            HandleCursorDown(id);

            data.Handled = true;

            base.OnTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchUp(TouchDevice device, Int64 id, RoutedEventData data)
        {
            if (TouchesCaptured.Contains(id) && IsDragging)
            {
                HandleCursorUp(id);
                ReleaseTouchCapture(id);

                data.Handled = true;
            }
            base.OnTouchUp(device, id, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchMove(TouchDevice device, Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            if (IsDragging)
            {
                HandleCursorMove(0);

                data.Handled = true;
            }
            base.OnTouchMove(device, id, x, y, dx, dy, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateCommonState();
            base.OnIsMouseOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsDragging"/> dependency property changes.
        /// </summary>
        private static void HandleIsDraggingChanged(DependencyObject element, Boolean oldValue, Boolean newValue)
        {
            var thumb = (Thumb)element;
            thumb.OnDraggingChanged(oldValue, newValue);
            thumb.UpdateCommonState();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UIElement.IsEnabled"/> dependency property changes.
        /// </summary>
        private static void HandleIsEnabledChanged(DependencyObject element, Boolean oldValue, Boolean newValue)
        {
            var thumb = (Thumb)element;
            thumb.UpdateCommonState();
        }

        /// <summary>
        /// Handles <see cref="Mouse.MouseMoveEvent"/> and <see cref="Touch.TouchMoveEvent"/> events for the thumb.
        /// </summary>
        private void HandleCursorMove(Int64 cursorID)
        {
            UpdateLayout();

            var relPos = (cursorID == 0) ? Mouse.GetPosition(this) : Touch.GetPosition(cursorID, this);
            var oldPos = dragPosLast;
            var newPos = TransformToAncestor(View.LayoutRoot, relPos);
            if (newPos != oldPos)
            {
                dragPosLast = newPos;
                RaiseDragDelta(relPos.X - dragPosOriginRel.X, relPos.Y - dragPosOriginRel.Y);
            }
        }

        /// <summary>
        /// Handles <see cref="Mouse.MouseDownEvent"/> and <see cref="Touch.TouchDownEvent"/> events for the thumb.
        /// </summary>
        private void HandleCursorDown(Int64 cursorID)
        {
            if (IsDragging)
                CancelDrag();

            UpdateLayout();

            dragCursorID = cursorID;
            dragPosOriginRel = (cursorID == 0) ? Mouse.GetPosition(this) : Touch.GetPosition(cursorID, this);
            dragPosOriginAbs = TransformToAncestor(View.LayoutRoot, dragPosOriginRel);
            dragPosLast = dragPosOriginAbs;

            IsDragging = true;
            RaiseDragStarted(dragPosOriginRel.X, dragPosOriginRel.Y);
        }

        /// <summary>
        /// Handles <see cref="Mouse.MouseUpEvent"/> and <see cref="Touch.TouchUpEvent"/> events for the thumb.
        /// </summary>
        private void HandleCursorUp(Int64 cursorID)
        {
            if (!IsDragging || dragCursorID != cursorID)
                return;

            CancelDrag();
        }

        /// <summary>
        /// Raises the <see cref="DragStarted"/> event.
        /// </summary>
        private void RaiseDragStarted(Double hoffset, Double voffset)
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfDragStartedEventHandler>(DragStartedEvent);
            evtDelegate(this, hoffset, voffset, evtData);
        }

        /// <summary>
        /// Raises the <see cref="DragDelta"/> event.
        /// </summary>
        private void RaiseDragDelta(Double hchange, Double vchange)
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfDragDeltaEventHandler>(DragDeltaEvent);
            evtDelegate(this, hchange, vchange, evtData);
        }

        /// <summary>
        /// Raises the <see cref="DragCompleted"/> event.
        /// </summary>
        private void RaiseDragCompleted(Double hchange, Double vchange)
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfDragCompletedEventHandler>(DragCompletedEvent);
            evtDelegate(this, hchange, vchange, evtData);
        }
        
        /// <summary>
        /// Transitions the thumb into the appropriate common state.
        /// </summary>
        private void UpdateCommonState()
        {
            if (IsEnabled)
            {
                if (IsDragging)
                {
                    VisualStateGroups.GoToState("common", "pressed");
                }
                else
                {
                    if (IsMouseOver)
                    {
                        VisualStateGroups.GoToState("common", "hover");
                    }
                    else
                    {
                        VisualStateGroups.GoToState("common", "normal");
                    }
                }
            }
            else
            {
                VisualStateGroups.GoToState("common", "disabled");
            }
        }
        
        // State values.
        private Int64 dragCursorID;
        private Point2D dragPosOriginRel;
        private Point2D dragPosOriginAbs;
        private Point2D dragPosLast;
    }
}
