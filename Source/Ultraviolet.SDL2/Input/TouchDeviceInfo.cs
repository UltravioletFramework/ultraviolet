using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Manages the Ultraviolet context's connected touch devices.
    /// </summary>
    internal sealed class TouchDeviceInfo : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchDeviceInfo"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public TouchDeviceInfo(UltravioletContext uv)
            : base(uv)
        {
            var count = SDL_GetNumTouchDevices();
            devices = new SDL2TouchDevice[count];

            for (int i = 0; i < count; i++)
            {
                devices[i] = new SDL2TouchDevice(uv, i);
            }
        }

        /// <summary>
        /// Resets the states of the connected devices in preparation for the next frame.
        /// </summary>
        public void ResetDeviceStates()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var device in devices)
            {
                device.ResetDeviceState();
            }
        }

        /// <summary>
        /// Updates the states of the connected touch devices.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            foreach (var device in devices)
            {
                device.Update(time);
            }
        }

        /// <summary>
        /// Gets the touch device with the specified device index.
        /// </summary>
        /// <param name="index">The index of the device to retrieve.</param>
        /// <returns>The touch device with the specified device index, or <see langword="null"/> if no such device exists.</returns>
        public TouchDevice GetTouchDeviceByIndex(Int32 index)
        {
            return devices[index];
        }

        /// <summary>
        /// Gets the number of available touch devices.
        /// </summary>
        public Int32 Count
        {
            get { return devices.Length; }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                foreach (var device in devices)
                {
                    SafeDispose.Dispose(device);
                }
            }
            base.Dispose(disposing);
        }

        // Connected touch devices.
        private SDL2TouchDevice[] devices;
    }
}
