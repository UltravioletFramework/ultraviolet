using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents the metadata for an asset which is being loaded by the Ultraviolet content pipeline.
    /// </summary>
    internal sealed class AssetMetadata : IContentImporterMetadata, IContentProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetMetadata"/> class.
        /// </summary>
        /// <param name="assetPath">The asset path of the asset being loaded.</param>
        /// <param name="assetFilePath">The path to the file that contains the asset being loaded.</param>
        /// <param name="importerMetadata">The asset's importer metadata.</param>
        /// <param name="processorMetadata">The asset's processor metadata.</param>
        /// <param name="isFile">A value indicating whether the asset was loaded from a file.</param>
        /// <param name="isStream">A value indicating whether the asset was loaded from a stream.</param>
        public AssetMetadata(String assetPath, String assetFilePath, XElement importerMetadata, XElement processorMetadata, Boolean isFile, Boolean isStream)
        {
            this.AssetPath = assetPath;
            this.AssetFilePath = assetFilePath;
            this.AssetFileName = (assetFilePath == null) ? null : System.IO.Path.GetFileName(assetFilePath);
            this.Extension = (assetFilePath == null) ? null : System.IO.Path.GetExtension(assetFilePath);
            this.ImporterMetadata = importerMetadata;
            this.ProcessorMetadata = processorMetadata;
            this.IsFile = isFile;
            this.IsStream = isStream;
        }

        /// <summary>
        /// Creates an instance of the specified metadata type based on the metadata in this object.
        /// </summary>
        /// <typeparam name="T">The metadata type to create.</typeparam>
        /// <returns>A new instance of the specified metadata type.</returns>
        T IContentImporterMetadata.As<T>()
        {
            if (ImporterMetadata != null)
            {
                return ObjectLoader.LoadObject<T>(ImporterMetadata, true);
            }
            return new T();
        }

        /// <summary>
        /// Creates an instance of the specified metadata type based on the metadata in this object.
        /// </summary>
        /// <typeparam name="T">The metadata type to create.</typeparam>
        /// <returns>A new instance of the specified metadata type.</returns>
        T IContentProcessorMetadata.As<T>()
        {
            if (ProcessorMetadata != null)
            {
                return ObjectLoader.LoadObject<T>(ProcessorMetadata, true);
            }
            return new T();
        }

        /// <summary>
        /// Gets the asset path of the asset being loaded.
        /// </summary>
        public String AssetPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to the file that contains the asset data.
        /// </summary>
        public String AssetFilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the file that contains the asset data.
        /// </summary>
        public String AssetFileName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the file extension of the file being loaded.
        /// </summary>
        public String Extension
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's importer metadata.
        /// </summary>
        public XElement ImporterMetadata
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's processor metadata.
        /// </summary>
        public XElement ProcessorMetadata
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the asset was loaded from a file.
        /// </summary>
        public Boolean IsFile
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the asset was loaded from a stream.
        /// </summary>
        public Boolean IsStream
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents an empty asset metadata object for assets loaded from streams.
        /// </summary>
        public static readonly AssetMetadata StreamMetadata = new AssetMetadata(null, null, null, null, false, true);

        /// <summary>
        /// Represents an empty asset metadata object for assets loaded in-memory.
        /// </summary>
        public static readonly AssetMetadata InMemoryMetadata = new AssetMetadata(null, null, null, null, false, false);
    }
}
