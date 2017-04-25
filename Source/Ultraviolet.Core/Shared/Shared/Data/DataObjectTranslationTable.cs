using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a table which associates sets of application-local identifiers with global identifiers.
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class DataObjectTranslationTable : Dictionary<UInt16, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the DataObjectTranslationTable class from serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo"/> object containing the 
        /// information required to serialize the dictionary.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext"/> structure containing 
        /// the source and destinatoin of the serialized stream associated with the dictionary.</param>
        protected DataObjectTranslationTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        /// <summary>
        /// Gets a value indicating whether the specified local identifier in this translation
        /// table translates to an object that is valid within the current process.
        /// </summary>
        /// <typeparam name="T">The type of data object being evaluated.</typeparam>
        /// <param name="localID">The local identifier of the entry within the translation table to validate.</param>
        /// <returns><see langword="true"/> if the entry is valid; otherwise, <see langword="false"/>.</returns>
        public Boolean IsValid<T>(UInt16 localID) where T : DataObject
        {
            Guid globalID;
            if (!TryGetValue(localID, out globalID))
                return false;

            var registry = DataObjectRegistries.Get<T>();
            var obj = registry.GetObject(globalID);
            return obj != null;
        }

        /// <summary>
        /// Translates the specified local identifier in this translation table
        /// into the corresponding local identifier within the current process.
        /// </summary>
        /// <typeparam name="T">The type of data object being evaluated.</typeparam>
        /// <param name="localID">The local identifier to translate.</param>
        /// <returns>The translated local identifier.</returns>
        public UInt16 Translate<T>(UInt16 localID) where T : DataObject
        {
            Guid globalID;
            if (!TryGetValue(localID, out globalID))
                return 0;

            var registry = DataObjectRegistries.Get<T>();
            var obj = registry.GetObject(globalID);
            return (obj == null) ? (ushort)0 : obj.LocalID;
        }
    }
}
