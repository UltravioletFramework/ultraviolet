using System;
using System.Collections.Generic;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Provides access to information concerning the system's attached display devices.
    /// </summary>
    public interface IUltravioletDisplayInfo : IEnumerable<IUltravioletDisplay>
    {
        /// <summary>
        /// Gets the display with the specified index.
        /// </summary>
        /// <param name="index">The index of the display to retrieve.</param>
        /// <returns>The display with the specified index.</returns>
        IUltravioletDisplay this[Int32 index]
        {
            get;
        }

        /// <summary>
        /// Gets the application's primary display device.
        /// </summary>
        IUltravioletDisplay PrimaryDisplay
        {
            get;
        }

        /// <summary>
        /// Gets the number of connected displays.
        /// </summary>
        Int32 Count
        {
            get;
        }
    }
}
