using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletDisplayInfo interface.
    /// </summary>
    public sealed unsafe class OpenGLUltravioletDisplayInfo : IUltravioletDisplayInfo
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletDisplayInfo class.
        /// </summary>
        public OpenGLUltravioletDisplayInfo()
        {
            this.displays = Enumerable.Range(0, SDL.GetNumVideoDisplays())
                .Select(x => new OpenGLUltravioletDisplay(x))
                .ToList<IUltravioletDisplay>();
        }

        /// <summary>
        /// Gets the collection's enumerator.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        public List<IUltravioletDisplay>.Enumerator GetEnumerator()
        {
            return displays.GetEnumerator();
        }

        /// <summary>
        /// Gets the collection's enumerator.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the collection's enumerator.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        IEnumerator<IUltravioletDisplay> IEnumerable<IUltravioletDisplay>.GetEnumerator()
        {
            return GetEnumerator();
        }

        // The list of display devices.  SDL2 never updates its device info, even if
        // devices are added or removed, so we only need to create this once.
        private readonly List<IUltravioletDisplay> displays;
    }
}
