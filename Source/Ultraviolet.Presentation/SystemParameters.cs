using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains properties which expose system settings.
    /// </summary>
    public static class SystemParameters
    {
        /// <summary>
        /// Contains native system functions.
        /// </summary>
        private static class Native
        {
            #region Native Methods

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SystemParametersInfo(uint action, uint param, ref uint vparam, uint init);

            #endregion
        }

        /// <summary>
        /// Gets the initial delay in milliseconds, between a key being pressed and the first repeated key press event.
        /// </summary>
        public static Double KeyboardDelay
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    const UInt32 SPI_GETKEYBOARDDELAY = 0x0016;

                    uint delay = 0;
                    Native.SystemParametersInfo(SPI_GETKEYBOARDDELAY, 0, ref delay, 0);

                    return (1 + delay) * 250.0;
                }
                return 500.0;
            }
        }

        /// <summary>
        /// Gets the delay in milliseconds between repeated key press events.
        /// </summary>
        public static Double KeyboardSpeed
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    const UInt32 SPI_GETKEYBOARDSPEED = 0x000A;

                    uint speed = 0;
                    Native.SystemParametersInfo(SPI_GETKEYBOARDSPEED, 0, ref speed, 0);

                    return 33.0 + ((31 - speed) * (367.0 / 31));
                }
                return 33.0;
            }
        }

        /// <summary>
        /// Gets the number of milliseconds that the mouse must stay still before it is considered to be hovering.
        /// </summary>
        public static Double MouseHoverTime
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    const UInt32 SPI_GETMOUSEHOVERTIME = 0x0066;

                    uint speed = 0;
                    Native.SystemParametersInfo(SPI_GETMOUSEHOVERTIME, 0, ref speed, 0);

                    return speed;
                }
                return 400.0;
            }
        }
    }
}
