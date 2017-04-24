using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation
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
            /// Called when the tracker is retrieved from its pool.
            /// </summary>
            /// <param name="touchID">The unique identifier of the touch to track.</param>
            /// <param name="captureElement">The element which is capturing the touch.</param>
            /// <param name="captureMode">The capture mode for the touch.</param>
            public void OnRetrieveFromPool(Int64 touchID, IInputElement captureElement, CaptureMode captureMode)
            {
                if (this.touchID > 0)
                    Update(forceNullPosition: true);

                this.touchID = touchID;

                if (this.touchID > 0)
                {
                    if (captureMode != CaptureMode.None)
                        Capture(captureElement, captureMode);

                    Update();
                }
                else
                {
                    Cleanup();
                }
            }

            /// <summary>
            /// Called when the tracker is released back into its pool.
            /// </summary>
            public void OnReleaseIntoPool()
            {
                Release();

                if (this.touchID > 0)
                    Update(forceNullPosition: true);

                this.touchID = 0;
                Cleanup();
            }
            
            /// <summary>
            /// Gets the identifier of the touch associated with this tracker.
            /// </summary>
            public Int64 TouchID
            {
                get { return touchID; }
            }

            /// <inheritdoc/>
            protected override Point2D? UpdatePosition()
            {
                TouchInfo touchInfo;
                if (!device.TryGetTouch(TouchID, out touchInfo))
                    return null;

                var coords = device.DenormalizeCoordinates(touchInfo.CurrentX, touchInfo.CurrentY);
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
            protected override Boolean IsEnabled => device.IsRegistered;

            /// <inheritdoc/>
            protected override Boolean OpensToolTips { get; } = false;

            // State values.
            private readonly TouchDevice device;
            private Int64 touchID;
        }
    }
}
