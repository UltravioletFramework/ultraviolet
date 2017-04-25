using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Content
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
        /// <param name="id">The object's globally-unique identifier.</param>
        public UltravioletDataObject(String key, Guid id)
            : base(key, id)
        {

        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet =>
            UltravioletContext.DemandCurrent();
    }
}
