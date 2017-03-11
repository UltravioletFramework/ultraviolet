using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for horizontal and vertical scroll bars.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public abstract class OrientedScrollBar : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="OrientedScrollBar"/> type.
        /// </summary>
        static OrientedScrollBar()
        {
            ValueProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Double>(HandleValueChanged));
            MinimumProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Double>(HandleMinimumChanged));
            MaximumProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Double>(HandleMaximumChanged));
            SmallChangeProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Double>(HandleSmallChangeChanged));
            LargeChangeProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Double>(HandleLargeChangeChanged));
            FocusableProperty.OverrideMetadata(typeof(OrientedScrollBar), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));
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
        public static readonly DependencyProperty ViewportSizeProperty = ScrollBar.ViewportSizeProperty.AddOwner(typeof(OrientedScrollBar), 
            new PropertyMetadata<Double>(HandleViewportSizeChanged));

        /// <summary>
        /// Identifies the <see cref="Scroll"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Scroll"/> routed event.</value>
        public static readonly RoutedEvent ScrollEvent = ScrollBar.ScrollEvent.AddOwner(typeof(OrientedScrollBar));

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
        /// Occurs when the value of the <see cref="RangeBase.Value"/> dependency property changes.
        /// </summary>
        private static void HandleValueChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildValueChanged(child, newValue);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Minimum"/> dependency property changes.
        /// </summary>
        private static void HandleMinimumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildMinimumChanged(child, newValue);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.Maximum"/> dependency property changes.
        /// </summary>
        private static void HandleMaximumChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildMaximumChanged(child, newValue);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.SmallChange"/> dependency property changes.
        /// </summary>
        private static void HandleSmallChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildSmallChangeChanged(child, newValue);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RangeBase.LargeChange"/> dependency property changes.
        /// </summary>
        private static void HandleLargeChangeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildLargeChangeChanged(child, newValue);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> dependency property changes.
        /// </summary>
        private static void HandleViewportSizeChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var child = (OrientedScrollBar)element;
            var parent = child.TemplatedParent as ScrollBar;
            if (parent != null)
            {
                parent.OnChildViewportSizeChanged(child, newValue);
            }
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
