using System;
using System.Collections.Generic;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a registry of data-driven objects of a particular type.
    /// </summary>
    public interface IDataObjectRegistry
    {
        /// <summary>
        /// Indicates that the registry has been added to the global collection of registries.
        /// </summary>
        void Register();

        /// <summary>
        /// Indicates that the registry has been removed from the global collection of registries.
        /// </summary>
        void Unregister();

        /// <summary>
        /// Sets the collection of source files from which the registry reads object definitions.
        /// </summary>
        /// <param name="files">A collection of source files from which to read object definitions.</param>
        void SetSourceFiles(IEnumerable<String> files);

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
        /// <returns>The global identifier associated with the specified object key, or <see langword="null"/> if no such key exists.</returns>
        Guid? ResolveObjectKey(String key);
        
        /// <summary>
        /// Gets the data object's reference resolution name, which is used to resolve
        /// references with the format @ReferenceResolutionName:KEY
        /// </summary>
        String ReferenceResolutionName
        {
            get;
        }

        /// <summary>
        /// The name of the content elements in the definition files loaded by this registry.
        /// </summary>
        String DataElementName
        {
            get;
        }
    }
}
