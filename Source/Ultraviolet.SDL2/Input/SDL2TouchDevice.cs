using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="TouchDevice"/> class.
    /// </summary>
    public sealed class SDL2TouchDevice : TouchDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2TouchDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="index">The index of the SDL2 touch device represented by this object.</param>
        public SDL2TouchDevice(UltravioletContext uv, Int32 index)
            : base(uv)
        {
            // HACK: Working around an Android emulator glitch here -- it will
            // return a touch ID of 0 even after saying that the device exists,
            // and yet not produce any kind of SDL error... I hate emulators.            
            var id = SDL_GetTouchDevice(index);
            if (id == 0 && !String.IsNullOrEmpty(SDL_GetError()))
                throw new SDL2Exception();

            this.sdlTouchID = id;

            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            var evt = ((SDL2EventMessageData)data).Event;
            switch (evt.type)
            {
                case SDL_FINGERDOWN:
                    if (evt.tfinger.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        BeginTouch(ref evt);
                    }
                    break;

                case SDL_FINGERUP:
                    if (evt.tfinger.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        EndTouch(ref evt);
                    }
                    break;

                case SDL_FINGERMOTION:
                    if (evt.tfinger.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        UpdateTouch(ref evt);
                    }
                    break;

                case SDL_MULTIGESTURE:
                    if (evt.mgesture.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        OnMultiGesture(evt.mgesture.x, evt.mgesture.y, 
                            evt.mgesture.dTheta, evt.mgesture.dDist, evt.mgesture.numFingers);
                    }
                    break;

                case SDL_DOLLARRECORD:
                    if (evt.dgesture.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        isRecordingDollarGesture = false;

                        if (dollarGestureTaskCompletionSource != null)
                        {
                            dollarGestureTaskCompletionSource.SetResult(evt.dgesture.gestureId);
                            dollarGestureTaskCompletionSource = null;
                        }

                        OnDollarGestureRecorded(evt.dgesture.gestureId);
                    }
                    break;

                case SDL_DOLLARGESTURE:
                    if (evt.dgesture.touchId == sdlTouchID)
                    {
                        if (!isRegistered)
                            Register();

                        OnDollarGesture(evt.dgesture.gestureId, 
                            evt.dgesture.x, evt.dgesture.y, evt.dgesture.error, (Int32)evt.dgesture.numFingers);
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Resets the device's state in preparation for the next frame.
        /// </summary>
        public void ResetDeviceState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            taps.Clear();
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            timestamp = time.TotalTime.Ticks;

            var window = BoundWindow;
            if (window == null)
                return;

            var longPressDelaySpan = TimeSpan.FromMilliseconds(LongPressDelay);
            var longPressDistanceDips = LongPressMaximumDistance;
            var longPressDistancePixs = window.Display.DipsToPixels(longPressDistanceDips);

            for (int i = 0; i < touches.Count; i++)
            {
                var touchInfo = touches[i];
                if (touchInfo.IsLongPress)
                    continue;
                
                var touchDistancePixs = touchInfo.Distance;
                var touchDistanceDips = window.Display.PixelsToDips(touchDistancePixs);

                var touchLifetime = TimeSpan.FromTicks(timestamp - touchInfo.Timestamp);
                if (touchLifetime > longPressDelaySpan && touchDistanceDips <= longPressDistancePixs)
                {
                    SetTouchIsLongPress(ref touchInfo, true);

                    touches[i] = touchInfo;

                    OnLongPress(touchInfo.TouchID, touchInfo.FingerID,
                        touchInfo.CurrentX, touchInfo.CurrentY, touchInfo.Pressure);
                }
            }
        }

        /// <inheritdoc/>
        public override void BindToWindow(IUltravioletWindow window)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            boundWindow = window;
        }

        /// <inheritdoc/>
        public override Point2F NormalizeCoordinates(Point2 coordinates)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var window = BoundWindow;
            if (window == null)
                throw new InvalidOperationException(UltravioletStrings.TouchDeviceNotBoundToWindow);

            return new Point2F(
                coordinates.X / (Single)window.ClientSize.Width,
                coordinates.Y / (Single)window.ClientSize.Height);
        }

        /// <inheritdoc/>
        public override Point2F NormalizeCoordinates(Int32 x, Int32 y)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var window = BoundWindow;
            if (window == null)
                throw new InvalidOperationException(UltravioletStrings.TouchDeviceNotBoundToWindow);

            return new Point2F(
                x / (Single)window.ClientSize.Width,
                y / (Single)window.ClientSize.Height);
        }

        /// <inheritdoc/>
        public override Point2 DenormalizeCoordinates(Point2F coordinates)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var window = BoundWindow;
            if (window == null)
                throw new InvalidOperationException(UltravioletStrings.TouchDeviceNotBoundToWindow);

            return new Point2(
                (Int32)(coordinates.X * window.ClientSize.Width),
                (Int32)(coordinates.Y * window.ClientSize.Height));
        }

        /// <inheritdoc/>
        public override Point2 DenormalizeCoordinates(Single x, Single y)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var window = BoundWindow;
            if (window == null)
                throw new InvalidOperationException(UltravioletStrings.TouchDeviceNotBoundToWindow);

            return new Point2(
                (Int32)(x * window.ClientSize.Width),
                (Int32)(y * window.ClientSize.Height));
        }

        /// <inheritdoc/>
        public override Boolean IsActive(Int64 touchID)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            for (int i = 0; i < touches.Count; i++)
            {
                if (touches[i].TouchID == touchID)
                    return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override Boolean TryGetTouch(Int64 touchID, out TouchInfo touchInfo)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var touch in touches)
            {
                if (touch.TouchID == touchID)
                {
                    touchInfo = touch;
                    return true;
                }
            }

            touchInfo = default(TouchInfo);
            return false;
        }

        /// <inheritdoc/>
        public override TouchInfo GetTouch(Int64 touchID)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var touch in touches)
            {
                if (touch.TouchID == touchID)
                    return touch;
            }

            throw new ArgumentException(nameof(touchID));
        }

        /// <inheritdoc/>
        public override TouchInfo GetTouchByIndex(Int32 index)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return touches[index];
        }

        /// <inheritdoc/>
        public override Boolean WasTapped()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return taps.Count > 0;
        }

        /// <inheritdoc/>
        public override Boolean WasTappedWithin(RectangleF area)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var tap in taps)
            {
                if (area.Contains(tap.X, tap.Y))
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean IsTouchWithin(RectangleF area)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var touch in touches)
            {
                if (area.Contains(touch.CurrentX, touch.CurrentY))
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean IsTouchWithin(Int64 touchID, RectangleF area)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var touch in touches)
            {
                if (touch.TouchID == touchID)
                    return area.Contains(touch.CurrentX, touch.CurrentY);
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean IsFirstTouchInGesture(Int64 touchID)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (touches.Count == 0)
                return false;
            
            var first = touches[0];
            return first.TouchID == touchID && first.TouchIndex == 0;
        }

        /// <inheritdoc/>
        public override Int32 GetTouchIndex(Int64 touchID)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            TouchInfo touchInfo;
            if (!TryGetTouch(touchID, out touchInfo))
                return -1;

            return touchInfo.TouchIndex;
        }

        /// <inheritdoc/>
        public override Task<Int64> RecordDollarGestureAsync()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (dollarGestureTaskCompletionSource != null)
                return dollarGestureTaskCompletionSource.Task;

            dollarGestureTaskCompletionSource = new TaskCompletionSource<Int64>();

            if (!IsRecordingDollarGesture)
            {
                if (SDL_RecordGesture(sdlTouchID) == 0)
                    throw new SDL2Exception();

                isRecordingDollarGesture = true;
            }

            return dollarGestureTaskCompletionSource.Task;
        }

        /// <inheritdoc/>
        public override Boolean RecordDollarGesture()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsRecordingDollarGesture)
                return false;

            if (SDL_RecordGesture(sdlTouchID) == 0)
                throw new SDL2Exception();

            isRecordingDollarGesture = true;
            return true;
        }

        /// <inheritdoc/>
        public override void LoadDollarGestures(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));

            using (var streamWrapper = new SDL2StreamWrapper(stream))
            {
                if (SDL_LoadDollarTemplates(sdlTouchID, streamWrapper.ToIntPtr()) == 0)
                    throw new SDL2Exception();
            }
        }

        /// <inheritdoc/>
        public override void SaveDollarGestures(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));

            using (var streamWrapper = new SDL2StreamWrapper(stream))
            {
                if (SDL_SaveAllDollarTemplates(streamWrapper.ToIntPtr()) == 0)
                    throw new SDL2Exception();
            }
        }

        /// <inheritdoc/>
        public override IUltravioletWindow BoundWindow => boundWindow ?? Ultraviolet.GetPlatform().Windows.GetPrimary();

        /// <inheritdoc/>
        public override Boolean IsRecordingDollarGesture => isRecordingDollarGesture;

        /// <inheritdoc/>
        public override Int32 TouchCount => touches.Count;

        /// <inheritdoc/>
        public override Int32 TapCount => taps.Count;

        /// <inheritdoc/>
        public override Boolean IsRegistered => isRegistered;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.Messages.Unsubscribe(this);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Begins a new touch input.
        /// </summary>
        private void BeginTouch(ref SDL_Event evt)
        {
            var touchID = nextTouchID++;
            var touchIndex = touches.Count == 0 ? 0 : touches[touches.Count - 1].TouchIndex + 1;
            var touchInfo = new TouchInfo(timestamp, touchID, touchIndex, evt.tfinger.fingerId, 
                evt.tfinger.x, evt.tfinger.y, evt.tfinger.x, evt.tfinger.y, evt.tfinger.pressure, false);

            touches.Add(touchInfo);

            OnTouchDown(touchID, touchInfo.FingerID, touchInfo.CurrentX, touchInfo.CurrentY, touchInfo.Pressure);        
        }

        /// <summary>
        /// Ends an active touch input.
        /// </summary>
        private void EndTouch(ref SDL_Event evt)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                var touch = touches[i];
                if (touch.FingerID == evt.tfinger.fingerId)
                {
                    if (timestamp - touch.Timestamp <= TimeSpan.FromMilliseconds(TapDelay).Ticks)
                    {
                        var window = BoundWindow;
                        if (window != null)
                        {
                            var tapDistanceDips = TapMaximumDistance;
                            var tapDistancePixs = window.Display.DipsToPixels(tapDistanceDips);
                            if (tapDistancePixs >= touch.Distance)
                            {
                                EndTap(touch.TouchID, touch.FingerID, touch.OriginX, touch.OriginY);
                            }
                        }
                    }

                    OnTouchUp(touch.TouchID, touch.FingerID);

                    touches.RemoveAt(i);

                    break;
                }
            }
        }

        /// <summary>
        /// Ends a tap.
        /// </summary>
        private void EndTap(Int64 touchID, Int64 fingerID, Single x, Single y)
        {
            var tapInfo = new TouchTapInfo(touchID, fingerID, x, y);
            taps.Add(tapInfo);

            OnTap(touchID, fingerID, x, y);
        }

        /// <summary>
        /// Updates an active touch input.
        /// </summary>
        private void UpdateTouch(ref SDL_Event evt)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                var touch = touches[i];
                if (touch.FingerID == evt.tfinger.fingerId)
                {
                    Single dx, dy;
                    SetTouchPosition(ref touch, evt.tfinger.x, evt.tfinger.y, out dx, out dy, evt.tfinger.pressure);

                    touches[i] = touch;

                    OnTouchMotion(touch.TouchID, touch.FingerID, 
                        evt.tfinger.x, evt.tfinger.y, dx, dy, evt.tfinger.pressure);

                    break;
                }
            }
        }

        /// <summary>
        /// Flags the device as registered.
        /// </summary>
        private void Register()
        {
            var input = (SDL2UltravioletInput)Ultraviolet.GetInput();
            if (input.RegisterTouchDevice(this))
                isRegistered = true;
        }

        // State values.
        private readonly List<TouchInfo> touches = new List<TouchInfo>(5);
        private readonly List<TouchTapInfo> taps = new List<TouchTapInfo>(5);
        private readonly Int64 sdlTouchID;
        private Int64 nextTouchID = 1;
        private Int64 timestamp;
        private Boolean isRegistered;
        private TaskCompletionSource<Int64> dollarGestureTaskCompletionSource;

        // Property values.
        private IUltravioletWindow boundWindow;
        private Boolean isRecordingDollarGesture;
    }
}
