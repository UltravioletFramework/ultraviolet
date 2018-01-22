using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation
{
    partial class CursorTracker
    {
        /// <summary>
        /// Represents an object which tracks the mouse cursor.
        /// </summary>
        public sealed class Mouse : CursorTracker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CursorTracker.Mouse"/> class.
            /// </summary>
            /// <param name="view">The view which owns this tracker.</param>
            /// <param name="device">The mouse device which is being tracked.</param>
            public Mouse(PresentationFoundationView view, MouseDevice device)
                : base(view)
            {
                this.device = device;
            }

            /// <inheritdoc/>
            protected override Point2D? UpdatePosition()
            {
                var posWin = device.GetPositionInWindow(View.Window);
                return posWin.HasValue ? View.Display.PixelsToDips(posWin.Value) : (Point2D?)null;
            }

            /// <inheritdoc/>
            protected override Boolean GetIsCaptureWithin(UIElement element)
            {
                return element.IsMouseCaptureWithin;
            }

            /// <inheritdoc/>
            protected override void SetIsCaptureWithin(UIElement element, Boolean isCaptureWithin)
            {
                element.IsMouseCaptureWithin = isCaptureWithin;
            }

            /// <inheritdoc/>
            protected override Boolean GetIsCaptured(UIElement element)
            {
                return element.IsMouseCaptured;
            }

            /// <inheritdoc/>
            protected override void SetIsCaptured(UIElement element, Boolean isCaptured)
            {
                element.IsMouseCaptured = isCaptured;
            }

            /// <inheritdoc/>
            protected override Boolean GetIsOver(UIElement element)
            {
                return element.IsMouseOver;
            }

            /// <inheritdoc/>
            protected override void SetIsOver(UIElement element, Boolean isOver)
            {
                element.IsMouseOver = isOver;
            }

            /// <inheritdoc/>
            protected override Boolean GetIsDirectlyOver(UIElement element)
            {
                return element.IsMouseDirectlyOver;
            }

            /// <inheritdoc/>
            protected override void SetIsDirectlyOver(UIElement element, Boolean isDirectlyOver)
            {
                element.IsMouseDirectlyOver = isDirectlyOver;
            }

            /// <inheritdoc/>
            protected override void RaiseGotCapture(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Mouse.RaiseGotMouseCapture(element, data);
            }

            /// <inheritdoc/>
            protected override void RaiseLostCapture(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Mouse.RaiseLostMouseCapture(element, data);
            }

            /// <inheritdoc/>
            protected override void RaiseEnter(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Mouse.RaiseMouseEnter(element, Input.Mouse.PrimaryDevice, data);
            }

            /// <inheritdoc/>
            protected override void RaiseLeave(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Mouse.RaiseMouseLeave(element, Input.Mouse.PrimaryDevice, data);
            }

            /// <inheritdoc/>
            protected override Boolean IsEnabled => device.IsRegistered || device.Ultraviolet.GetInput().EmulateMouseWithTouchInput;

            /// <inheritdoc/>
            protected override Boolean OpensToolTips { get; } = true;

            // State values.
            private readonly MouseDevice device;
        }
    }
}
