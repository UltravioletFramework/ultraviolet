using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class CursorTracker
    {
        /// <summary>
        /// Represents an object which tracks a touch.
        /// </summary>
        public sealed class Touch : CursorTracker
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Touch"/> class.
            /// </summary>
            /// <param name="view">The view which owns this tracker.</param>
            /// <param name="device">The touch device which is being tracked.</param>
            public Touch(PresentationFoundationView view, TouchDevice device)
                : base(view)
            {
                this.device = device;
            }

            /// <summary>
            /// Gets or sets the identifier of the touch associated with this tracker.
            /// </summary>
            public Int64 TouchID
            {
                get { return touchID; }
                set
                {
                    if (touchID == value)
                        return;

                    if (touchID > 0)
                        Update(forceNullPosition: true);

                    touchID = value;

                    if (touchID > 0)
                    {
                        Update();
                    }
                    else
                    {
                        Cleanup();
                    }
                }
            }

            /// <inheritdoc/>
            protected override Point2D? UpdatePosition()
            {
                TouchInfo touchInfo;
                if (!device.TryGetTouch(TouchID, out touchInfo))
                    return null;

                var coords = device.DenormalizeCoordinates(View.Window, touchInfo.CurrentX, touchInfo.CurrentY);
                return View.Display.PixelsToDips(coords);
            }

            /// <inheritdoc/>
            protected override Boolean GetIsCaptureWithin(UIElement element)
            {
                return element.TouchesCapturedWithin.Contains(TouchID);
            }

            /// <inheritdoc/>
            protected override void SetIsCaptureWithin(UIElement element, Boolean isCaptureWithin)
            {
                if (isCaptureWithin)
                {
                    element.TouchesCapturedWithin.Add(TouchID);
                }
                else
                {
                    element.TouchesCapturedWithin.Remove(TouchID);
                }
            }

            /// <inheritdoc/>
            protected override Boolean GetIsCaptured(UIElement element)
            {
                return element.TouchesCaptured.Contains(TouchID);
            }

            /// <inheritdoc/>
            protected override void SetIsCaptured(UIElement element, Boolean isCaptured)
            {
                if (isCaptured)
                {
                    element.TouchesCaptured.Add(TouchID);
                }
                else
                {
                    element.TouchesCaptured.Remove(TouchID);
                }
            }

            /// <inheritdoc/>
            protected override Boolean GetIsOver(UIElement element)
            {
                return element.TouchesOver.Contains(TouchID);
            }

            /// <inheritdoc/>
            protected override void SetIsOver(UIElement element, Boolean isOver)
            {
                if (isOver)
                {
                    element.TouchesOver.Add(TouchID);
                }
                else
                {
                    element.TouchesOver.Remove(TouchID);
                }
            }

            /// <inheritdoc/>
            protected override Boolean GetIsDirectlyOver(UIElement element)
            {
                return element.TouchesDirectlyOver.Contains(TouchID);
            }

            /// <inheritdoc/>
            protected override void SetIsDirectlyOver(UIElement element, Boolean isDirectlyOver)
            {
                if (isDirectlyOver)
                {
                    element.TouchesDirectlyOver.Add(TouchID);
                }
                else
                {
                    element.TouchesDirectlyOver.Remove(TouchID);
                }
            }

            /// <inheritdoc/>
            protected override void RaiseGotCapture(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Touch.RaiseGotTouchCapture(element, device, TouchID, data);
            }

            /// <inheritdoc/>
            protected override void RaiseLostCapture(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Touch.RaiseLostTouchCapture(element, device, TouchID, data);
            }

            /// <inheritdoc/>
            protected override void RaiseEnter(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Touch.RaiseTouchEnter(element, device, TouchID, data);
            }

            /// <inheritdoc/>
            protected override void RaiseLeave(UIElement element)
            {
                var data = RoutedEventData.Retrieve(element);
                Input.Touch.RaiseTouchLeave(element, device, TouchID, data);
            }

            /// <inheritdoc/>
            protected override Boolean OpensToolTips { get; } = false;

            // State values.
            private readonly TouchDevice device;
            private Int64 touchID;
        }
    }
}
