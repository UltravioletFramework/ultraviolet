using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a collection of curve keys.
    /// </summary>
    [Serializable]
    public sealed class CurveKeyCollection : UltravioletCollection<CurveKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKeyCollection"/> class from the specified collection of keys.
        /// </summary>
        /// <param name="keys">A collection of <see cref="CurveKey"/> objects with which to populate the collection.</param>
        public CurveKeyCollection(IEnumerable<CurveKey> keys)
        {
            if (keys != null)
            {
                Storage.AddRange(keys);
            }
        }
    }
}
