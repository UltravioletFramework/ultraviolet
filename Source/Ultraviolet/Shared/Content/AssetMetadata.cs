using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the metadata for an asset which is being loaded by the Ultraviolet content pipeline.
    /// </summary>
    internal sealed class AssetMetadata : IContentImporterMetadata, IContentProcessorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetMetadata"/> class.
        /// </summary>
        /// <param name="overrideDirectory">The override directory from which the asset was loaded.</param>
        /// <param name="assetPath">The asset path of the asset being loaded.</param>
        /// <param name="assetFilePath">The path to the file that contains the asset being loaded.</param>
        /// <param name="importerMetadata">The asset's importer metadata.</param>
        /// <param name="processorMetadata">The asset's processor metadata.</param>
        /// <param name="isFile">A value indicating whether the asset was loaded from a file.</param>
        /// <param name="isStream">A value indicating whether the asset was loaded from a stream.</param>
        /// <param name="isJson">A value indicating whether the asset metadata is JSON.</param>
        /// <param name="isLoadedFromSln">A value indicating whether the asset is being loaded from the 
        /// application's solution rather than the binaries folder.</param>
        /// <param name="assetDensity">The screen density for which the asset is being loaded.</param>
        public AssetMetadata(String overrideDirectory, String assetPath, String assetFilePath, 
            Object importerMetadata, Object processorMetadata, Boolean isFile, Boolean isStream, Boolean isJson, Boolean isLoadedFromSln, ScreenDensityBucket assetDensity)
        {
            this.OverrideDirectory = overrideDirectory;
            this.AssetPath = assetPath;
            this.AssetFilePath = assetFilePath;
            this.AssetFileName = (assetFilePath == null) ? null : System.IO.Path.GetFileName(assetFilePath);
            this.Extension = (assetFilePath == null) ? null : System.IO.Path.GetExtension(assetFilePath);
            this.ImporterMetadata = importerMetadata;
            this.ProcessorMetadata = processorMetadata;
            this.IsFile = isFile;
            this.IsStream = isStream;
            this.IsJson = isJson;
            this.IsLoadedFromSolution = isLoadedFromSln;
            this.AssetDensity = assetDensity;
        }
        
        /// <summary>
        /// Creates an empty asset metadata object for assets loaded from streams.
        /// </summary>
        /// <param name="density">The density bucket for which the asset is being loaded.</param>
        /// <param name="importerMetadata">The content importer metadata object.</param>
        /// <param name="processorMetadata">The content processor metadata object.</param>
        /// <returns>The <see cref="AssetMetadata"/> which was created.</returns>
        public static AssetMetadata CreateStreamMetadata(ScreenDensityBucket density, Object importerMetadata = null, Object processorMetadata = null) =>
            new AssetMetadata(null, null, null, importerMetadata, processorMetadata, false, true, false, false, density);

        /// <summary>
        /// Creates an empty asset metadata object for assets loaded in-memory.
        /// </summary>
        /// <param name="density">The density bucket for which the asset is being loaded.</param>
        /// <param name="importerMetadata">The content importer metadata object.</param>
        /// <param name="processorMetadata">The content processor metadata object.</param>
        /// <returns>The <see cref="AssetMetadata"/> which was created.</returns>
        public static AssetMetadata CreateInMemoryMetadata(ScreenDensityBucket density, Object importerMetadata = null, Object processorMetadata = null) =>
            new AssetMetadata(null, null, null, importerMetadata, processorMetadata, false, false, false, false, density);

        /// <summary>
        /// Adds the specified asset as a dependency of the asset being loaded.
        /// </summary>
        /// <param name="dependency">The asset path of the dependency.</param>
        public void AddAssetDependency(String dependency)
        {
            Contract.RequireNotEmpty(dependency, nameof(dependency));

            AssetDependencies.Add(dependency);
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
                if (IsJson)
                {
                    var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);                    
                    return ((JObject)ImporterMetadata).ToObject<T>(serializer);
                }
                else
                {
                    return ObjectLoader.LoadObject<T>((XElement)ImporterMetadata, true);
                }
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
                if (IsJson)
                {
                    var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
                    return ((JObject)ProcessorMetadata).ToObject<T>(serializer);
                }
                else
                {
                    return ObjectLoader.LoadObject<T>((XElement)ProcessorMetadata, true);
                }
            }
            return new T();
        }
        
        /// <summary>
        /// Gets the override directory from which the asset was loaded.
        /// </summary>
        public String OverrideDirectory
        {
            get;
            private set;
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
        public Object ImporterMetadata
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's processor metadata.
        /// </summary>
        public Object ProcessorMetadata
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
        /// Gets a value indicating whether the asset metadata is defined as JSON.
        /// </summary>
        public Boolean IsJson
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this asset was loaded from an override directory.
        /// </summary>
        public Boolean IsOverridden
        {
            get { return !String.IsNullOrEmpty(OverrideDirectory); }
        }
        
        /// <summary>
        /// Gets a value indicating whether the asset is being loaded from the solution,
        /// rather than the binaries folder of the application.
        /// </summary>
        public Boolean IsLoadedFromSolution
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the screen density bucket for which the asset is being loaded.
        /// </summary>
        public ScreenDensityBucket AssetDensity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of dependencies which have been registered for the loaded asset.
        /// </summary>
        public ISet<String> AssetDependencies { get; } = new HashSet<String>();
    }
}
