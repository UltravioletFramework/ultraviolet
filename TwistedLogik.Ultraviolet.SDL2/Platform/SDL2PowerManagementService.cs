using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="PowerManagementService"/> class.
    /// </summary>
    public sealed unsafe class SDL2PowerManagementService : PowerManagementService
    {
        /// <inheritdoc/>
        public override Single PercentBatteryRemaining
        {
            get 
            {
                UpdateCache();
                return MathUtil.Clamp(pct / 100.0f, 0f, 1f);
            }
        }

        /// <inheritdoc/>
        public override Boolean IsBatteryPowered
        {
            get 
            {
                UpdateCache();
                return isBatteryPowered;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsPluggedIn
        {
            get 
            {
                UpdateCache();
                return isPluggedIn;
            }
        }

        /// <summary>
        /// Updates the service's cached state values.
        /// </summary>
        private void UpdateCache()
        {
            var now = DateTime.UtcNow;
            if ((now - lastCacheUpdate).TotalSeconds > 1.0)
            {
                int sdlPct;

				this.sdlPowerState = SDL.GetPowerInfo(null, &sdlPct);
                this.pct           = sdlPct;

                switch (sdlPowerState)
                {
                    case SDL_PowerState.UNKNOWN:
                    case SDL_PowerState.NO_BATTERY:
                        this.isBatteryPowered = false;
                        this.isPluggedIn      = false;
                        break;

                    case SDL_PowerState.ON_BATTERY:
                        this.isBatteryPowered = true;
                        this.isPluggedIn      = false;
                        break;

                    case SDL_PowerState.CHARGING:
                    case SDL_PowerState.CHARGED:
                        this.isBatteryPowered = true;
                        this.isPluggedIn      = true;
                        break;
                }

                this.lastCacheUpdate = now;
            }
        }

        // Cached property values.
        private SDL_PowerState sdlPowerState;
        private Int32 pct;
        private Boolean isBatteryPowered;
        private Boolean isPluggedIn;
        private DateTime lastCacheUpdate;
    }
}
