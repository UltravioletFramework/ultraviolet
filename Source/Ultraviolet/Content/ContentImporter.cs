using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// <para>Represents a content importer.</para>
    /// <para>Content importers take raw file data and transform them into intermediate data structures,
    /// which are then used by content processors to produce game assets.</para>
    /// </summary>
    /// <typeparam name="T">The intermediate asset type which is produced by the content importer.</typeparam>
    public abstract class ContentImporter<T> : IContentImporter
    {
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        Object IContentImporter.Import(IContentImporterMetadata metadata, Stream stream)
        {
            return Import(metadata, stream);
        }

        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public abstract T Import(IContentImporterMetadata metadata, Stream stream);

        /// <summary>
        /// Resolves the asset path of the specified dependency.
        /// </summary>
        /// <param name="metadata">The content processor metadata.</param>
        /// <param name="dependency">The relative path of the dependency to resolve.</param>
        /// <returns>The asset path of the specified dependency.</returns>
        protected static String ResolveDependencyAssetPath(IContentImporterMetadata metadata, String dependency)
        {
            Contract.Require(metadata, nameof(metadata));

            if (dependency == null)
                return null;

            if (metadata.AssetPath == null)
                return dependency;

            return ContentManager.NormalizeAssetPath(Path.Combine(Path.GetDirectoryName(metadata.AssetPath), dependency));
        }

        /// <summary>
        /// Resolves the asset file path of the specified dependency.
        /// </summary>
        /// <param name="metadata">The content processor metadata.</param>
        /// <param name="dependency">The relative path of the dependency to resolve.</param>
        /// <returns>The asset path of the specified dependency.</returns>
        protected static String ResolveDependencyAssetFilePath(IContentImporterMetadata metadata, String dependency)
        {
            Contract.Require(metadata, nameof(metadata));

            if (dependency == null)
                return null;

            if (metadata.AssetFilePath == null)
                return dependency;

            return ContentManager.NormalizeAssetPath(Path.Combine(Path.GetDirectoryName(metadata.AssetFilePath), dependency));
        }
    }
}
