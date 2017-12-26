using System;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="PowerManagementService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="PowerManagementService"/> that was created.</returns>
    public delegate PowerManagementService PowerManagementServiceFactory();

    /// <summary>
    /// Represents a platform service which retrieves the device's current power management state.
    /// </summary>
    public abstract class PowerManagementService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PowerManagementService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="PowerManagementService"/> that was created.</returns>
        public static PowerManagementService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<PowerManagementServiceFactory>()();
        }

        /// <summary>
        /// Gets the amount of battery remaining, if the system is battery-powered.
        /// </summary>
        public abstract Single PercentBatteryRemaining
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the system is battery-powered.
        /// </summary>
        public abstract Boolean IsBatteryPowered
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the system is currently plugged in.
        /// </summary>
        public abstract Boolean IsPluggedIn
        {
            get;
        }
    }
}
