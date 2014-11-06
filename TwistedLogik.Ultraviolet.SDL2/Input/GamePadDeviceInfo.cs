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
            this.devicesByPlayer = new SDL2GamePadDevice[SDL.NumJoysticks()];

            for (int i = 0; i < this.devicesByPlayer.Length; i++)
            {
                if (SDL.IsGameController(i))
                {
                    OnControllerDeviceAdded(i);
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

            foreach (var device in devicesByPlayer)
            {
                if (device != null)
                {
                    device.Update(time);
                }
            }
        }

        /// <summary>
        /// Gets the game pad that belongs to the specified player.
        /// </summary>
        /// <param name="playerIndex">The index of the player for which to retrieve a game pad.</param>
        /// <returns>The game pad that belongs to the specified player, or <c>null</c> if no such game pad exists.</returns>
        public SDL2GamePadDevice GetGamePadForPlayer(Int32 playerIndex)
        {
            if (playerIndex < 0)
                throw new ArgumentOutOfRangeException("index");

            return (playerIndex >= devicesByPlayer.Length) ? null : devicesByPlayer[playerIndex];
        }

        /// <summary>
        /// Gets the first connected game pad device.
        /// </summary>
        /// <returns>The first connected game pad device, or <c>null</c> if no game pads are connected.</returns>
        public SDL2GamePadDevice GetFirstConnectedGamePad()
        {
            for (int i = 0; i < devicesByPlayer.Length; i++)
            {
                if (devicesByPlayer[i] != null)
                {
                    return devicesByPlayer[i];
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
                foreach (var device in devicesByPlayer)
                {
                    if (device != null)
                    {
                        device.Dispose();
                    }
                }
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
        /// <param name="joystickIndex">The index of the device to add.</param>
        private void OnControllerDeviceAdded(Int32 joystickIndex)
        {
            var gamecontroller = SDL.GameControllerOpen(joystickIndex);
            var joystick       = SDL.GameControllerGetJoystick(gamecontroller);
            var joystickID     = SDL.JoystickInstanceID(joystick);

            for (int i = 0; i < devicesByPlayer.Length; i++)
            {
                if (devicesByPlayer[i] != null && devicesByPlayer[i].InstanceID == joystickID)
                {
                    return;
                }
            }

            var playerIndex = GetFirstAvailablePlayerIndex();

            devicesByPlayer[playerIndex] = new SDL2GamePadDevice(Ultraviolet, joystickIndex, playerIndex);
            count++;
        }

        /// <summary>
        /// Called when a controller device is removed.
        /// </summary>
        /// <param name="instanceID">The instance identifier of the device to remove.</param>
        private void OnControllerDeviceRemoved(Int32 instanceID)
        {
            for (int i = 0; i < devicesByPlayer.Length; i++)
            {
                if (devicesByPlayer[i] != null && devicesByPlayer[i].InstanceID == instanceID)
                {
                    devicesByPlayer[i] = null;
                    count--;

                    return;
                }
            }
        }

        /// <summary>
        /// Gets the index of the first player which does not have an associated game pad.
        /// </summary>
        /// <returns>The index of the first player which does not have an associated game pad.</returns>
        private Int32 GetFirstAvailablePlayerIndex()
        {
            for (int i = 0; i < devicesByPlayer.Length; i++)
            {
                if (devicesByPlayer[i] == null)
                {
                    return i;
                }
            }
        
            var devicesOld = devicesByPlayer;
            var devicesNew = new SDL2GamePadDevice[devicesOld.Length + 1];
            Array.Copy(devicesOld, devicesNew, devicesOld.Length);

            devicesByPlayer = devicesNew;

            return devicesByPlayer.Length - 1;
        }

        // State values.
        private SDL2GamePadDevice[] devicesByPlayer;
        private Int32 count;
    }
}
