using System;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor's asset metadata.
    /// </summary>
    public interface IContentProcessorMetadata
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

        /// <summary>
        /// Gets the asset's extension.
        /// </summary>
        String Extension { get; }

        /// <summary>
        /// Gets a value indicating whether the asset is being loaded from a file.
        /// </summary>
        Boolean IsFile { get; }

        /// <summary>
        /// Gets a value indicating whether the asset is being loaded from a stream.
        /// </summary>
        Boolean IsStream { get; }
    }
}
