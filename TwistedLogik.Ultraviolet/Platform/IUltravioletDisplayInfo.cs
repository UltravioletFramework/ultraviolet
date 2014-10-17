using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Provides access to information concerning the system's attached display devices.
    /// </summary>
    public interface IUltravioletDisplayInfo : IEnumerable<IUltravioletDisplay>
    {
        /// <summary>
        /// Gets the collection's enumerator.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        new List<IUltravioletDisplay>.Enumerator GetEnumerator();
    }
}
