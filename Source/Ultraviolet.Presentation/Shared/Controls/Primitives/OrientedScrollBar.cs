using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for horizontal and vertical scroll bars.
    /// </summary>
    [UvmlKnownType]
    public abstract class OrientedScrollBar : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="OrientedScrollBar"/> type.
        /// </summary>
        static OrientedScrollBar()
        {
            FocusableProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

            EventManager.RegisterClassHandler(typeof(OrientedScrollBar), Thumb.DragStartedEvent, new UpfDragStartedEventHandler(HandleThumbDragStarted));
            EventManager.RegisterClassHandler(typeof(OrientedScrollBar), Thumb.DragDeltaEvent, new UpfDragDeltaEventHandler(HandleThumbDragDelta));
            EventManager.RegisterClassHandler(typeof(OrientedScrollBar), Thumb.DragCompletedEvent, new UpfDragCompletedEventHandler(HandleThumbDragCompleted));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrientedScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public OrientedScrollBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the scroll bar's <see cref="Track"/> component.
        /// </summary>
        public Track Track
        {
            get { return PART_Track; }
        }

        /// <summary>
        /// Gets or sets the amount of scrollable content that is currently visible.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the amount of scrollable content that is currently
        /// visible in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="ViewportSizeProperty"/></dpropField>
        ///		<dpropStylingName>viewport-size</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Occurs when the scroll bar's content is scrolled as a result of the user moving the thumb.
        /// </summary>
        /// <remarks>
        /// <para>This event is not raised when the scroll bar's value is changed programatically.</para>
        /// <revt>
        ///		<revtField><see cref="ScrollEvent"/></revtField>
        ///		<revtStylingName>scroll</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfScrollEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfScrollEventHandler Scroll
        {
            add { AddHandler(ScrollEvent, value); }
            remove { RemoveHandler(ScrollEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ViewportSize"/> dependency property.</value>
        public static readonly DependencyProperty ViewportSizeProperty = ScrollBar.ViewportSizeProperty.AddOwner(typeof(OrientedScrollBar));

        /// <summary>
        /// Identifies the <see cref="Scroll"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Scroll"/> routed event.</value>
        public static readonly RoutedEvent ScrollEvent = ScrollBar.ScrollEvent.AddOwner(typeof(OrientedScrollBar));

        /// <summary>
        /// Occurs when the scroll bar receives a <see cref="Thumb.DragDelta"/> event.
        /// </summary>
        /// <param name="hchange">The distance that the thumb moved horizontally.</param>
        /// <param name="vchange">The distance that the thumb moved vertically.</param>
        protected virtual void OnThumbDragDelta(Double hchange, Double vchange)
        {
            if (hchange == 0 && vchange == 0)
                return;

            var valueDelta = Track.ValueFromDistance(hchange, vchange);
            if (!Double.IsNaN(valueDelta) && valueDelta != 0.0)
            {
                var valueAfterChange = Math.Max(Minimum, Math.Min(Value + valueDelta, Maximum));

                const Double MaximumPerpendicularDistance = 150.0;

                var perpendicularDistance = Math.Abs((Track.Orientation == Orientation.Horizontal) ? vchange : hchange);
                if (perpendicularDistance > MaximumPerpendicularDistance)
                    valueAfterChange = valueAtStartOfDrag;

                if (IsPartOfScrollViewer)
                {
                    var command = (Track.Orientation == Orientation.Horizontal) ? ScrollBar.DeferScrollToHorizontalOffsetCommand : ScrollBar.DeferScrollToVerticalOffsetCommand;
                    var commandParam = new PrimitiveUnion(valueAfterChange);
                    var commandTarget = (TemplatedParent as IInputElement) ?? this;
                    if (command.CanExecute(View, null, commandParam, commandTarget))
                    {
                        command.Execute(View, null, commandParam, commandTarget);
                    }
                    else
                    {
                        command = (Track.Orientation == Orientation.Horizontal) ? ScrollBar.ScrollToHorizontalOffsetCommand : ScrollBar.ScrollToVerticalOffsetCommand;
                        if (command.CanExecute(View, null, commandParam, commandTarget))
                        {
                            command.Execute(View, null, commandParam, commandTarget);
                        }
                    }
                }
                else
                {
                    Value = valueAfterChange;
                }
                RaiseScrollEvent(ScrollEventType.ThumbTrack);
            }
        }

        /// <inheritdoc/>
        protected override void OnMinimumChanged()
        {
            InvalidateMeasure();

            if (PART_Track != null)
                PART_Track.InvalidateArrange();

            base.OnMinimumChanged();
        }

        /// <inheritdoc/>
        protected override void OnMaximumChanged()
        {
            InvalidateMeasure();

            if (PART_Track != null)
                PART_Track.InvalidateArrange();

            base.OnMaximumChanged();
        }

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (PART_Track != null)
                PART_Track.InvalidateArrange();

            base.OnValueChanged();
        }
        
        /// <summary>
        /// Gets a value indicating whether the scroll bar is part of a <see cref="ScrollViewer"/> control.
        /// </summary>
        protected Boolean IsPartOfScrollViewer
        {
            get { return TemplatedParent is ScrollViewer; }
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event.
        /// </summary>
        /// <param name="type">The scroll event type.</param>
        protected internal void RaiseScrollEvent(ScrollEventType type)
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfScrollEventHandler>(ScrollEvent);
            evtDelegate(this, type, evtData);
        }
        
        /// <summary>
        /// Occurs when the user starts a thumb drag operation.
        /// </summary>
        private static void HandleThumbDragStarted(DependencyObject element, Double hoffset, Double voffset, RoutedEventData data)
        {
            var scrollBar = element as OrientedScrollBar;
            if (scrollBar == null)
                return;

            scrollBar.valueAtStartOfDrag = scrollBar.Value;
        }

        /// <summary>
        /// Occurs when the user moves the thumb during a drag operation.
        /// </summary>
        private static void HandleThumbDragDelta(DependencyObject element, Double hchange, Double vchange, RoutedEventData data)
        {
            var scrollBar = (OrientedScrollBar)element;
            scrollBar.OnThumbDragDelta(hchange, vchange);
        }

        /// <summary>
        /// Occurs when the user completes a thumb drag operation.
        /// </summary>
        private static void HandleThumbDragCompleted(DependencyObject element, Double hchange, Double vchange, RoutedEventData data)
        {
            var scrollBar = (OrientedScrollBar)element;
            scrollBar.RaiseScrollEvent(ScrollEventType.EndScroll);
        }

        // State values.
        private const Double MaximumPerpendularDistance = 150.0;
        private Double valueAtStartOfDrag;

        // Component references.
        private readonly Track PART_Track = null;
    }
}
