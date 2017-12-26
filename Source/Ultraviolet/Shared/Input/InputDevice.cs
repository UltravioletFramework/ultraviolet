using System;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents any input device.
    /// </summary>
    public abstract class InputDevice : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        internal InputDevice(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Updates the device's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Gets a value indicating whether the device has been registered with the context
        /// as a result of receiving user input.
        /// </summary>
        public abstract Boolean IsRegistered { get; }
    }
}
