using System;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer.
    /// </summary>
    public interface IContentImporter
    {
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        Object Import(IContentImporterMetadata metadata, Stream stream);
    }
}
