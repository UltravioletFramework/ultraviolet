using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_PowerState;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="PowerManagementService"/> class.
    /// </summary>
    public sealed class SDL2PowerManagementService : PowerManagementService
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
        private unsafe void UpdateCache()
        {
            var now = DateTime.UtcNow;
            if ((now - lastCacheUpdate).TotalSeconds > 1.0)
            {
                int sdlPct;

                this.sdlPowerState = SDL_GetPowerInfo(null, &sdlPct);
                this.pct = sdlPct;

                switch (sdlPowerState)
                {
                    case SDL_POWERSTATE_UNKNOWN:
                    case SDL_POWERSTATE_NO_BATTERY:
                        this.isBatteryPowered = false;
                        this.isPluggedIn = false;
                        break;

                    case SDL_POWERSTATE_ON_BATTERY:
                        this.isBatteryPowered = true;
                        this.isPluggedIn = false;
                        break;

                    case SDL_POWERSTATE_CHARGING:
                    case SDL_POWERSTATE_CHARGED:
                        this.isBatteryPowered = true;
                        this.isPluggedIn = true;
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
