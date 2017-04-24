using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which tracks the position of a cursor (mouse or touch) within a particular view.
    /// </summary>
    internal abstract partial class CursorTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursorTracker"/> class.
        /// </summary>
        /// <param name="view">The view which owns this tracker.</param>
        protected CursorTracker(PresentationFoundationView view)
        {
            this.View = view;
        }

        /// <summary>
        /// Creates a new <see cref="CursorTracker"/> which tracks the mouse cursor.
        /// </summary>
        /// <param name="view">The view for which to create the tracker.</param>
        /// <returns>The <see cref="CursorTracker"/> instance which was created.</returns>
        public static Mouse ForMouse(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            return new Mouse(view, Input.Mouse.PrimaryDevice);
        }

        /// <summary>
        /// Creates a new <see cref="CursorTracker"/> which tracks touches.
        /// </summary>
        /// <param name="view">The view for which to create the tracker.</param>
        /// <returns>The <see cref="CursorTracker"/> instance which was created.</returns>
        public static Touch ForTouch(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));
            
            return new Touch(view, Input.Touch.PrimaryDevice);
        }

        /// <summary>
        /// Cleans up the cursor's internal references.
        /// </summary>
        public void Cleanup()
        {
            underCursorPrev = null;
            underCursor = null;

            underCursorBeforeValidityCheckPrev = null;
            underCursorBeforeValidityCheck = null;

            withCapture = null;
            captureMode = CaptureMode.None;

            underCursorPopupPrev = null;
            underCursorPopup = null;
        }

        /// <summary>
        /// Updates the cursor's relationship to the view's elements.
        /// </summary>
        /// <param name="forceNullPosition">A value indicating whether to force the tracker's position to <see langword="null"/>,
        /// regardless of the physical device state.</param>
        public void Update(Boolean forceNullPosition = false)
        {
            Position = (!IsEnabled || forceNullPosition) ? null : UpdatePosition();

            underCursorPopupPrev = underCursorPopup;
            underCursorPrev = underCursor;

            underCursor = HasPosition ? View.HitTestInternal(Position.Value, out underCursorPopup) as UIElement : null;
            underCursor = RedirectInput(underCursor);

            underCursorBeforeValidityCheckPrev = underCursorBeforeValidityCheck;
            underCursorBeforeValidityCheck = underCursor;

            if (!(underCursor?.IsValidForInput() ?? false))
                underCursor = null;

            if (!(withCapture?.IsValidForInput() ?? false))
                Release();

            // Handle tooltips.
            if (OpensToolTips && underCursorBeforeValidityCheck != underCursorBeforeValidityCheckPrev)
                View.UpdateToolTipElement(underCursorBeforeValidityCheck);

            // Handle cursor movement.
            if (underCursor != underCursorPrev)
            {
                UpdateIsOver(underCursorPrev as UIElement, underCursorPopup != underCursorPopupPrev);

                if (underCursorPrev != null)
                {
                    var uiElement = underCursorPrev as UIElement;
                    if (uiElement != null)
                        SetIsDirectlyOver(uiElement, false);
                }

                if (underCursor != null)
                {
                    var uiElement = underCursor as UIElement;
                    if (uiElement != null)
                        SetIsDirectlyOver(uiElement, true);
                }

                UpdateIsOver(underCursor as UIElement);
            }
        }

        /// <summary>
        /// Captures the cursor to the specified element.
        /// </summary>
        /// <param name="element">The element to which to assign capture.</param>
        /// <param name="mode">The capture mode to apply.</param>
        /// <returns><see langword="true"/> if the cursor was captured; otherwise, <see langword="false"/>.</returns>
        public Boolean Capture(IInputElement element, CaptureMode mode)
        {
            if ((element != null && mode == CaptureMode.None) || (element == null && mode != CaptureMode.None))
                throw new ArgumentException(nameof(mode));

            if (withCapture == element)
                return true;

            if (!(element?.IsValidForInput() ?? false))
                return false;

            if (withCapture != null)
                Release();

            withCapture = element;
            captureMode = mode;

            var uiElement = withCapture as UIElement;
            if (uiElement != null)
            {
                SetIsCaptured(uiElement, true);

                UpdateIsCaptureWithin(uiElement, true);
                UpdateIsOver(uiElement);

                RaiseGotCapture(uiElement);
            }

            return true;
        }

        /// <summary>
        /// Releases capture.
        /// </summary>
        /// <returns><see langword="true"/> if capture was released; otherwise, <see langword="false"/>.</returns>
        public Boolean Release()
        {
            if (withCapture == null)
                return false;

            var hadCapture = withCapture;
            withCapture = null;
            captureMode = CaptureMode.None;

            var uiElement = hadCapture as UIElement;
            if (uiElement != null)
            {
                SetIsCaptured(uiElement, false);

                UpdateIsCaptureWithin(uiElement, false);
                UpdateIsOver(uiElement);

                RaiseLostCapture(uiElement);
            }

            return true;
        }

        /// <summary>
        /// Gets the view which owns this tracker.
        /// </summary>
        public PresentationFoundationView View { get; }

        /// <summary>
        /// Gets the cursor's current position within the view.
        /// </summary>
        public Point2D? Position { get; private set; }

        /// <summary>
        /// Gets the cursor's x-coordinate in device-independent pixels.
        /// </summary>
        public Double? X => Position?.X;

        /// <summary>
        /// Gets the cursor's y-coordinate in device-independent pixels.
        /// </summary>
        public Double? Y => Position?.Y;

        /// <summary>
        /// Gets a value indicating whether the cursor has a valid position.
        /// </summary>
        public Boolean HasPosition => Position.HasValue;

        /// <summary>
        /// Gets the element that is currently under this cursor.
        /// </summary>
        public IInputElement ElementUnderCursor => underCursor;

        /// <summary>
        /// Gets the element that is currently under this cursor, ignoring checks to determine whether
        /// that element is a valid input target.
        /// </summary>
        public IInputElement ElementUnderCursorBeforeValidityCheck => underCursorBeforeValidityCheck;

        /// <summary>
        /// Gets the element that has captured this cursor.
        /// </summary>
        public IInputElement ElementWithCapture => withCapture;

        /// <summary>
        /// Updates the cursor's position from the physical device state.
        /// </summary>
        /// <returns>The cursor's position within the view.</returns>
        protected abstract Point2D? UpdatePosition();

        /// <summary>
        /// Gets the property value on the specified element which indicates whether capture
        /// for this cursor is within the element.
        /// </summary>
        protected abstract Boolean GetIsCaptureWithin(UIElement element);

        /// <summary>
        /// Sets the property value on the specified element which indicates that capture for
        /// this cursor is within the element.
        /// </summary>
        protected abstract void SetIsCaptureWithin(UIElement element, Boolean isCaptureWithin);

        /// <summary>
        /// Gets the property value on the specified element which indicates whether the cursor is captured by the element.
        /// </summary>
        protected abstract Boolean GetIsCaptured(UIElement element);

        /// <summary>
        /// Sets the property value on the specified element which indicates that the cursor is captured by the element.
        /// </summary>
        protected abstract void SetIsCaptured(UIElement element, Boolean isCaptured);

        /// <summary>
        /// Gets the property value on the specified element which indicates whether the cursor is over the element.
        /// </summary>
        protected abstract Boolean GetIsOver(UIElement element);

        /// <summary>
        /// Sets the property value on the specified element which indicates that the cursor is over the element.
        /// </summary>
        protected abstract void SetIsOver(UIElement element, Boolean isOver);

        /// <summary>
        /// Gets the property value on the specified element which indicates whether the cursor is directly over the element.
        /// </summary>
        protected abstract Boolean GetIsDirectlyOver(UIElement element);

        /// <summary>
        /// Sets the property value on the specified element which indicates that the cursor is directly over the element.
        /// </summary>
        protected abstract void SetIsDirectlyOver(UIElement element, Boolean isDirectlyOver);

        /// <summary>
        /// Raises an event indicating that the specified element has captured the cursor.
        /// </summary>
        protected abstract void RaiseGotCapture(UIElement element);

        /// <summary>
        /// Raises an event indicating that the specified element has lost capture.
        /// </summary>
        protected abstract void RaiseLostCapture(UIElement element);

        /// <summary>
        /// Raises an event indicating that the cursor has entered the specified element.
        /// </summary>
        /// <param name="element"></param>
        protected abstract void RaiseEnter(UIElement element);

        /// <summary>
        /// Raises an event indicating that the cursor has left the specified element.
        /// </summary>
        protected abstract void RaiseLeave(UIElement element);

        /// <summary>
        /// Gets a value indicating whether the cursor is currently enabled.
        /// </summary>
        protected abstract Boolean IsEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this cursor causes tool tips to open.
        /// </summary>
        protected abstract Boolean OpensToolTips { get; }

        /// <summary>
        /// Redirects input to the element with capture, if necessary.
        /// </summary>
        private IInputElement RedirectInput(IInputElement recipient)
        {
            if (captureMode == CaptureMode.None || !IsEnabled)
                return recipient;

            return IsCapturedBy(recipient) ? recipient : withCapture;
        }

        /// <summary>
        /// Gets a value indicating whether this cursor has been captured by the specified element.
        /// </summary>
        private Boolean IsCapturedBy(IInputElement element)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return false;

            if (captureMode == CaptureMode.None)
                return false;

            if (captureMode == CaptureMode.Element)
                return uiElement == withCapture;

            var current = (DependencyObject)element;
            while (current != null)
            {
                if (current == withCapture)
                    return true;

                current = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current);
            }
            return false;
        }

        /// <summary>
        /// Updates the relevant properties on the ancestors of the specified element to indicating whether capture
        /// for this cursor lies within those elements.
        /// </summary>
        private void UpdateIsCaptureWithin(UIElement root, Boolean value)
        {
            if (root == null)
                return;

            var current = root as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                    SetIsCaptureWithin(uiElement, value);

                current = VisualTreeHelper.GetParent(current);
            }
        }

        /// <summary>
        /// Updates the relevant properties on the ancestors of the specified element to indicate
        /// whether this cursor is over those elements.
        /// </summary>
        private void UpdateIsOver(UIElement root, Boolean unset = false)
        {
            if (root == null)
                return;

            isOverSet.Clear();

            var cursorElement = HasPosition ? (DependencyObject)View.HitTest(Position.Value) : null;
            cursorElement = RedirectInput(cursorElement as IInputElement) as DependencyObject;

            while (cursorElement != null)
            {
                isOverSet.Add(cursorElement);
                cursorElement = VisualTreeHelper.GetParent(cursorElement);
            }

            var current = root as DependencyObject;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                {
                    var oldValue = GetIsOver(uiElement);
                    var newValue = isOverSet.Contains(uiElement) && !unset;
                    if (oldValue != newValue)
                    {
                        SetIsOver(uiElement, newValue);
                        if (newValue)
                        {
                            RaiseEnter(uiElement);
                        }
                        else
                        {
                            RaiseLeave(uiElement);
                        }
                    }
                }
                current = VisualTreeHelper.GetParent(current);
            }

            isOverSet.Clear();
        }

        // Tracked elements
        private readonly HashSet<DependencyObject> isOverSet = new HashSet<DependencyObject>();
        private IInputElement underCursorPrev;
        private IInputElement underCursor;
        private IInputElement underCursorBeforeValidityCheckPrev;
        private IInputElement underCursorBeforeValidityCheck;
        private IInputElement withCapture;
        private Popup underCursorPopupPrev;
        private Popup underCursorPopup;

        // State values.
        private CaptureMode captureMode;
    }
}