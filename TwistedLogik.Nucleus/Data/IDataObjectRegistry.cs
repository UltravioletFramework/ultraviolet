using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a registry of data-driven objects of a particular type.
    /// </summary>
    [CLSCompliant(false)]
    public interface IDataObjectRegistry
    {
        /// <summary>
        /// Removes all objects from the registry and returns it to its default state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Loads the registry's object keys.
        /// </summary>
        void LoadKeys();

        /// <summary>
        /// Loads the registry's objects.
        /// </summary>
        void LoadObjects();

        /// <summary>
        /// Resolves an object key to a global identifier.
        /// </summary>
        /// <param name="key">The object key to resolve.</param>
        /// <returns>The global identifier associated with the specified object key, or <c>null</c> if no such key exists.</returns>
        Guid? ResolveObjectKey(String key);
        
        /// <summary>
        /// Gets the data object's reference resolution name, which is used to resolve
        /// references with the format @ReferenceResolutionName:KEY
        /// </summary>
        String ReferenceResolutionName
        {
            get;
        }
    }
}
