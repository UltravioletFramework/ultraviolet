using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an instance of <see cref="DataObject"/> which is designed for use with the Ultraviolet Framework.
    /// </summary>
    /// <inheritdoc/>
    [CLSCompliant(false)]
    public abstract class UltravioletDataObject : DataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletDataObject"/> class.
        /// </summary>
        /// <param name="key">The object's uniquely identifying key.</param>
        /// <param name="globalID">The object's globally-unique identifier.</param>
        public UltravioletDataObject(String key, Guid globalID)
            : base(key, globalID)
        {

        }
    }
}
