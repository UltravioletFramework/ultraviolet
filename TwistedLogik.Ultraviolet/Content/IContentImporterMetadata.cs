using System;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer's asset metadata.
    /// </summary>
    public interface IContentImporterMetadata
    {
        /// <summary>
        /// Creates an instance of the specified metadata type based on the metadata in this object.
        /// </summary>
        /// <typeparam name="T">The metadata type to create.</typeparam>
        /// <returns>A new instance of the specified metadata type.</returns>
        T As<T>() where T : new();

        /// <summary>
        /// Gets the asset path of the asset being loaded.
        /// </summary>
        String AssetPath { get; }

        /// <summary>
        /// Gets the path to the file that contains the asset data.
        /// </summary>
        String AssetFilePath { get; }

        /// <summary>
        /// Gets the name of the file that contains the asset data.
        /// </summary>
        String AssetFileName { get; }
    }
}
