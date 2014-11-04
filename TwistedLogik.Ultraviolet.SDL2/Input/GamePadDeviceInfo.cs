using System;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Manages the Ultraviolet context's connected game pad devices.
    /// </summary>
    internal sealed class GamePadDeviceInfo : UltravioletResource,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadDeviceInfo"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public GamePadDeviceInfo(UltravioletContext uv)
            : base(uv)
        {
            this.devices = new SDL2GamePadDevice[SDL.NumJoysticks()];

            for (int i = 0; i < this.devices.Length; i++)
            {
                if (SDL.IsGameController(i))
                {
                    this.devices[i] = new SDL2GamePadDevice(uv, i);
                }
            }

            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type != SDL2UltravioletMessages.SDLEvent)
                return;

            var evt = ((SDL2EventMessageData)data).Event;
            switch (evt.type)
            {
                case SDL_EventType.CONTROLLERDEVICEADDED:
                    OnControllerDeviceAdded(evt.cdevice.which);
                    break;

                case SDL_EventType.CONTROLLERDEVICEREMOVED:
                    OnControllerDeviceRemoved(evt.cdevice.which);
                    break;
            }
        }

        /// <summary>
        /// Updates the state of the attached game pad devices.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var device in devices)
            {
                if (device != null)
                {
                    device.Update(time);
                }
            }
        }

        /// <summary>
        /// Gets the game pad device with the specified index.
        /// </summary>
        /// <param name="index">The index of the device to retrieve.</param>
        /// <returns>The game pad device with the specified index, or <c>null</c> if no such device exists.</returns>
        public SDL2GamePadDevice GetDeviceByIndex(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            return (index >= devices.Length) ? null : devices[index];
        }

        /// <summary>
        /// Gets the first available game pad device.
        /// </summary>
        /// <returns>The first available game pad device, or null if no game pads are available.</returns>
        public SDL2GamePadDevice GetFirstGamePad()
        {
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i] != null)
                {
                    return devices[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the number of attached game pads.
        /// </summary>
        public Int32 Count
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return count; 
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                foreach (var device in devices)
                    device.Dispose();

                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.Messages.Unsubscribe(this);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when a controller device is added.
        /// </summary>
        /// <param name="index">The index of the device.</param>
        private void OnControllerDeviceAdded(Int32 index)
        {
            if (this.devices.Length <= index)
            {
                var temp = this.devices;
                this.devices = new SDL2GamePadDevice[index + 1];
                Array.Copy(temp, this.devices, temp.Length);
            }

            var existing = this.devices[index];
            if (existing != null)
            {
                return;
            }
            this.devices[index] = new SDL2GamePadDevice(Ultraviolet, index);

            count++;
        }

        /// <summary>
        /// Called when a controller device is removed.
        /// </summary>
        /// <param name="index">The index of the device.</param>
        private void OnControllerDeviceRemoved(Int32 index)
        {
            var device = this.devices[index];
            if (device != null)
            {
                this.devices[index].Dispose();
                this.devices[index] = null;
            }

            count--;
        }

        // State values.
        private SDL2GamePadDevice[] devices;
        private Int32 count;
    }
}
