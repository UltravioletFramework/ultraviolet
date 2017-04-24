using System;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents an object which is dynamically constructed from data files.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class DataObject : IGloballyIdentified
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObject"/> class.
        /// </summary>
        /// <param name="key">The object's uniquely identifying key.</param>
        /// <param name="id">The object's globally-unique identifier.</param>
        public DataObject(String key, Guid id)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            Key = key;
            GlobalID = id;
        }

        /// <summary>
        /// Gets the object's uniquely identifying key.
        /// </summary>
        public String Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object's globally-unique identifier.
        /// </summary>
        /// <remarks>A data object's global identifier allows it to be addressed in a way which does not change
        /// across instances of the application.</remarks>
        public Guid GlobalID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object's application-local identifier.
        /// </summary>
        /// <remarks>A data object's local identifier allows it to be conveniently and efficiently addressed
        /// within a single process. Local identifiers may change across instances of an application.</remarks>
        public UInt16 LocalID
        {
            get;
            internal set;
        }
    }
}
