using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for sliders.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public abstract class SliderBase : RangeBase
    {
        /// <summary>
        /// Initializes the <see cref="SliderBase"/> class.
        /// </summary>
        static SliderBase()
        {
            EventManager.RegisterClassHandler(typeof(SliderBase), Mouse.PreviewMouseDownEvent, new UpfMouseButtonEventHandler(HandlePreviewMouseDown));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public SliderBase(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

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
        /// Handles the <see cref="Mouse.PreviewMouseDownEvent"/>.
        /// </summary>
        private static void HandlePreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                ((SliderBase)element).Focus();
            }
        }

        // Component references.
        private readonly Track PART_Track = null;
    }
}
