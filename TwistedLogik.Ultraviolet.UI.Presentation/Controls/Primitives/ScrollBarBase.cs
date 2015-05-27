using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the method that handles a <see cref="ScrollBarBase.Scroll"/> event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="type">The scroll event type.</param>
    /// <param name="data">The routed event data.</param>
    public delegate void UpfScrollEventHandler(DependencyObject element, ScrollEventType type, ref RoutedEventData data);

    /// <summary>
    /// Represents the base class for scroll bars.
    /// </summary>
    public abstract class ScrollBarBase : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollBarBase(UltravioletContext uv, String name)
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
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue<Double>(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Occurs when the scroll bar's content is scrolled as a result of the user moving the thumb.
        /// </summary>
        /// <remarks>This event is not raised when the scroll bar's value is changed programatically.</remarks>
        public event UpfScrollEventHandler Scroll
        {
            add { AddHandler(ScrollEvent, value); }
            remove { RemoveHandler(ScrollEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'viewport-size'.</remarks>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(ScrollBarBase),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Scroll"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'scroll'.</remarks>
        public static readonly RoutedEvent ScrollEvent = EventManager.RegisterRoutedEvent("Scroll", RoutingStrategy.Bubble,
            typeof(UpfScrollEventHandler), typeof(ScrollBarBase));

        /// <inheritdoc/>
        protected override void OnMinimumChanged()
        {
            InvalidateMeasure();
            base.OnMinimumChanged();
        }

        /// <inheritdoc/>
        protected override void OnMaximumChanged()
        {
            InvalidateMeasure();
            base.OnMaximumChanged();
        }

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            if (PART_Track != null)
            {
                PART_Track.InvalidateArrange();
            }
            base.OnValueChanged();
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event.
        /// </summary>
        /// <param name="type">The scroll event type.</param>
        protected internal void RaiseScrollEvent(ScrollEventType type)
        {
            var evtData     = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfScrollEventHandler>(ScrollEvent);
            evtDelegate(this, type, ref evtData);
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
