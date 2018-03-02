using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="IUltravioletDisplayInfo"/> interface.
    /// </summary>
    public sealed class SDL2UltravioletDisplayInfo : IUltravioletDisplayInfo
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletDisplayInfo class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SDL2UltravioletDisplayInfo(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            this.displays = Enumerable.Range(0, SDL_GetNumVideoDisplays())
                .Select(x => new SDL2UltravioletDisplay(uv, x))
                .ToList<IUltravioletDisplay>();
        }

        /// <summary>
        /// Updates the state of the application's displays.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            foreach (var display in displays)
                ((SDL2UltravioletDisplay)display).Update(time);
        }

        /// <inheritdoc/>
        public IUltravioletDisplay this[Int32 ix]
        {
            get { return displays[ix]; }
        }

        /// <inheritdoc/>
        public IUltravioletDisplay PrimaryDisplay
        {
            get { return displays.Count == 0 ? null : displays[0]; }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return displays.Count; }
        }

        /// <inheritdoc/>
        public List<IUltravioletDisplay>.Enumerator GetEnumerator()
        {
            return displays.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<IUltravioletDisplay> IEnumerable<IUltravioletDisplay>.GetEnumerator()
        {
            return GetEnumerator();
        }

        // The list of display devices.  SDL2 never updates its device info, even if
        // devices are added or removed, so we only need to create this once.
        private readonly List<IUltravioletDisplay> displays;
    }
}
