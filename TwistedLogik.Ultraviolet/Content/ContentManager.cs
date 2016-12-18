﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a collection of related content assets.
    /// </summary>
    public sealed partial class ContentManager : UltravioletResource, IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes the <see cref="ContentManager"/> type.
        /// </summary>
        static ContentManager()
        {
            ScreenDensityBuckets = ((ScreenDensityBucket[])Enum.GetValues(typeof(ScreenDensityBucket))).OrderByDescending(x => x);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class with the specified root directory.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="rootDirectory">The content manager's root directory.</param>
        private ContentManager(UltravioletContext uv, String rootDirectory)
            : base(uv)
        {
            this.rootDirectory = rootDirectory;
            this.fileSystemService = FileSystemService.Create();

            uv.Messages.Subscribe(this, UltravioletMessages.LowMemory);
        }

        /// <summary>
        /// Creates a new <see cref="ContentManager"/> with the specified root directory.
        /// </summary>
        /// <param name="rootDirectory">The content manager's root directory.</param>
        /// <returns>The <see cref="ContentManager"/> that was created.</returns>
        public static ContentManager Create(String rootDirectory = null)
        {
            var uv = UltravioletContext.DemandCurrent();
            return new ContentManager(uv, rootDirectory);
        }

        /// <summary>
        /// Creates a new <see cref="ContentManager"/> with the specified root directory.
        /// </summary>
        /// <param name="rootDirectoryPaths">An array containing the parts of the path to the content manager's root directory.</param>
        /// <returns>The <see cref="ContentManager"/> that was created.</returns>
        public static ContentManager Create(params String[] rootDirectoryPaths)
        {
            return Create(Path.Combine(rootDirectoryPaths));
        }

        /// <summary>
        /// Converts the specified asset path to a normalized asset path.
        /// </summary>
        /// <param name="path">The asset path to normalize.</param>
        /// <returns>The normalized asset path.</returns>
        public static String NormalizeAssetPath(String path)
        {
            // Make sure the path isn't rooted.
            if (Path.IsPathRooted(path))
                throw new ArgumentException(UltravioletStrings.AssetPathMustBeRelative);

            // Remove any directory traversal operators.
            var stack = new Stack<String>();
            var parts = path.Split('/', '\\');
            foreach (var part in parts)
            {
                if (part == ".") { continue; }
                if (part == "..")
                {
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException(UltravioletStrings.AssetPathCannotTraverseDirectories);
                    }
                    stack.Pop();
                    continue;
                }
                stack.Push(part);
            }

            // Recombine the path parts into a single string.
            var sb = new StringBuilder();
            foreach (var item in stack)
            {
                if (sb.Length > 0)
                {
                    sb.Insert(0, Path.DirectorySeparatorChar);
                }
                sb.Insert(0, item);
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.LowMemory)
            {
                PurgeCache(true);
            }
        }

        /// <summary>
        /// Purges the content manager's internal cache, removing all references to previously loaded objects
        /// so that they can be collected.
        /// </summary>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void PurgeCache(Boolean lowMemory)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
            {
                foreach (var kvp in assetFlags)
                {
                    var asset = kvp.Key;
                    var flags = kvp.Value;

                    var preserve = (flags & AssetFlags.PreserveThroughLowMemory) == AssetFlags.PreserveThroughLowMemory;
                    if (preserve)
                        continue;

                    assetCache.Remove(asset);
                }
            }
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <remarks>Please note that, for performance reasons, the content manager's internal cache does not normalize asset paths.
        /// This means that if you reference the same asset by two different but equivalent paths (i.e. "foo/bar" and "foo\\bar"), 
        /// each of those paths will represent a <b>separate entry in the cache</b> with <b>separate asset flags</b>.</remarks>
        /// <param name="asset">The asset path of the asset for which to set flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> values to associate with the specified asset.</param>
        public void SetAssetFlags(String asset, AssetFlags flags)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
                assetFlags[asset] = flags;
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to set flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> values to associate with the specified asset.</param>
        public void SetAssetFlags(AssetID asset, AssetFlags flags)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
                assetFlags[AssetID.GetAssetPath(asset)] = flags;
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <remarks>Please note that, for performance reasons, the content manager's internal cache does not normalize asset paths.
        /// This means that if you reference the same asset by two different but equivalent paths (i.e. "foo/bar" and "foo\\bar"), 
        /// each of those paths will represent a <b>separate entry in the cache</b> with <b>separate asset flags</b>.</remarks>
        /// <param name="asset">The asset path of the asset for which to retrieve flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> value associated with the specified asset.</param>
        /// <returns><see langword="true"/> if the specified asset has flags defined within this 
        /// content manager; otherwise, <see langword="false"/>.</returns>
        public Boolean GetAssetFlags(String asset, out AssetFlags flags)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
                return assetFlags.TryGetValue(asset, out flags);
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to retrieve flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> value associated with the specified asset.</param>
        /// <returns><see langword="true"/> if the specified asset has flags defined within this 
        /// content manager; otherwise, <see langword="false"/>.</returns>
        public Boolean GetAssetFlags(AssetID asset, out AssetFlags flags)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
                return assetFlags.TryGetValue(AssetID.GetAssetPath(asset), out flags);
        }

        /// <summary>
        /// Loads all of the assets in the specified <see cref="ContentManifest"/> into the content manager's asset cache.
        /// </summary>
        /// <param name="manifest">The <see cref="ContentManifest"/> to load.</param>
        public void Load(ContentManifest manifest)
        {
            Contract.Require(manifest, nameof(manifest));
            Contract.EnsureNotDisposed(this, Disposed);

            Object result;
            foreach (var group in manifest)
            {
                foreach (var asset in group)
                {
                    LoadInternal(asset.AbsolutePath, asset.Type, true, false, false, out result);
                }
            }
        }

        /// <summary>
        /// Loads the specified asset file.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling Load() multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="asset">The path to the asset to load.</param>
        /// <param name="cache">A value indicating whether to add the asset to the manager's cache.</param>
        /// <returns>The asset that was loaded from the specified file.</returns>
        public TOutput Load<TOutput>(String asset, Boolean cache = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var cachedInstance = default(Object);
            var cacheMiss = false;

            lock (cacheSyncObject)
                cacheMiss = !assetCache.TryGetValue(asset, out cachedInstance);

            if (cacheMiss)
            {
                Object result;
                LoadInternal(asset, typeof(TOutput), cache, false, false, out result);
                return (TOutput)result;
            }

            return (TOutput)cachedInstance;
        }

        /// <summary>
        /// Loads the specified asset file.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling Load() multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="asset">The path to the asset to load.</param>
        /// <param name="cache">A value indicating whether to add the asset to the manager's cache.</param>
        /// <returns>The asset that was loaded from the specified file.</returns>
        public TOutput Load<TOutput>(AssetID asset, Boolean cache = true)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            return Load<TOutput>(AssetID.GetAssetPath(asset), cache);
        }

        /// <summary>
        /// Loads the specified asset stream.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> that contains the asset to load.</param>
        /// <param name="extension">The file extension to use to search for a content importer.</param>
        /// <returns>The asset that was loaded from the specified stream.</returns>
        public TOutput LoadFromStream<TOutput>(Stream stream, String extension)
        {
            Contract.Require(stream, nameof(stream));
            Contract.RequireNotEmpty(extension, nameof(extension));
            Contract.EnsureNotDisposed(this, Disposed);

            return (TOutput)LoadInternalFromStream(typeof(TOutput), stream, extension);
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="paths">An array of parts of the path to the asset to import.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(params String[] paths)
        {
            return Import<TOutput>(Path.Combine(paths));
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(String asset)
        {
            Type outputType;
            return Import<TOutput>(asset, out outputType);
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <param name="outputType">The output type of the content importer which was used.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(String asset, out Type outputType)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var metadata = GetAssetMetadata(asset, false);
            var importer = FindContentImporter(metadata.AssetFilePath, out outputType);

            using (var stream = fileSystemService.OpenRead(metadata.AssetFilePath))
            {
                return (TOutput)importer.Import(metadata, stream);
            }
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(AssetID asset)
        {
            Type outputType;
            return Import<TOutput>(asset, out outputType);
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <param name="outputType">The output type of the content importer which was used.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(AssetID asset, out Type outputType)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            return Import<TOutput>(AssetID.GetAssetPath(asset), out outputType);
        }

        /// <summary>
        /// Imports the specified asset from the specified stream, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> that contains the asset data.</param>
        /// <param name="extension">The file extension to use to search for a content importer.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput ImportFromStream<TOutput>(Stream stream, String extension)
        {
            Contract.Require(stream, nameof(stream));
            Contract.EnsureNotDisposed(this, Disposed);

            var path = String.Format("__STREAM.{0}", extension);
            var importerOutputType = default(Type);
            var importer = FindContentImporter(path, out importerOutputType);

            return (TOutput)importer.Import(AssetMetadata.StreamMetadata, stream);
        }

        /// <summary>
        /// Processes an intermediate object into an asset object.
        /// </summary>
        /// <typeparam name="TOutput">The type of the asset object produced by the content processor.</typeparam>
        /// <param name="intermediate">The intermediate object to process.</param>
        /// <param name="metadata">The processor metadata, if any.</param>
        /// <returns>The processed asset.</returns>
        public TOutput Process<TOutput>(Object intermediate, XElement metadata = null)
        {
            Contract.Require(intermediate, nameof(intermediate));
            Contract.EnsureNotDisposed(this, Disposed);

            var processor = FindContentProcessor("unknown", intermediate.GetType(), typeof(TOutput));
            var assetmeta = (metadata == null) ? AssetMetadata.InMemoryMetadata : new AssetMetadata(null, null, null, metadata, false, false);
            var result = processor.Process(this, assetmeta, intermediate);

            return (TOutput)result;
        }

        /// <summary>
        /// Processes an intermediate object into an asset object.
        /// </summary>
        /// <typeparam name="TInput">The type of the intermediate object being processed.</typeparam>
        /// <typeparam name="TOutput">The type of the asset object produced by the content processor.</typeparam>
        /// <param name="intermediate">The intermediate object to process.</param>
        /// <param name="metadata">The processor metadata, if any.</param>
        /// <returns>The processed asset.</returns>
        public TOutput Process<TInput, TOutput>(TInput intermediate, XElement metadata = null) where TInput : class
        {
            Contract.Require(intermediate, nameof(intermediate));
            Contract.EnsureNotDisposed(this, Disposed);

            var processor = FindContentProcessor("unknown", typeof(TInput), typeof(TOutput));
            var assetmeta = (metadata == null) ? AssetMetadata.InMemoryMetadata : new AssetMetadata(null, null, null, metadata, false, false);
            var result = processor.Process(this, assetmeta, intermediate);

            return (TOutput)result;
        }

        /// <summary>
        /// Preprocesses the assets in the specified content manifest.
        /// </summary>
        /// <param name="manifest">The content manifest to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original files after preprocessing them.</param>
        public void Preprocess(ContentManifest manifest, Boolean delete = false)
        {
            Contract.Require(manifest, nameof(manifest));
            Contract.EnsureNotDisposed(this, Disposed);

            PreprocessInternal(new[] { manifest }, delete);
        }

        /// <summary>
        /// Preprocesses the assets in the specified content manifests.
        /// </summary>
        /// <param name="manifests">A collection containing the content manifests to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original files after preprocessing them.</param>
        public void Preprocess(IEnumerable<ContentManifest> manifests, Boolean delete = false)
        {
            Contract.Require(manifests, nameof(manifests));
            Contract.EnsureNotDisposed(this, Disposed);

            PreprocessInternal(manifests, delete);
        }

        /// <summary>
        /// Preprocesses the specified asset by saving it in a binary format designed for fast deserialization.
        /// If the asset's content importer does not support a binary data format, this method has no effect.
        /// </summary>
        /// <typeparam name="TOutput">The type of asset to preprocess.</typeparam>
        /// <param name="asset">The asset to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original file after preprocessing it.</param>
        /// <returns><see langword="true"/> if the asset was preprocessed; otherwise, <see langword="false"/>.</returns>
        public Boolean Preprocess<TOutput>(String asset, Boolean delete = false)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            return PreprocessInternal(typeof(TOutput), asset, delete);
        }

        /// <summary>
        /// Preprocesses the specified asset by saving it in a binary format designed for fast deserialization.
        /// If the asset's content importer does not support a binary data format, this method has no effect.
        /// </summary>
        /// <typeparam name="TOutput">The type of asset to preprocess.</typeparam>
        /// <param name="asset">The asset to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original file after preprocessing it.</param>
        /// <returns><see langword="true"/> if the asset was preprocessed; otherwise, <see langword="false"/>.</returns>
        public Boolean Preprocess<TOutput>(AssetID asset, Boolean delete = false)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            return PreprocessInternal(typeof(TOutput), AssetID.GetAssetPath(asset), delete);
        }

        /// <summary>
        /// Flushes the file deletion buffer.
        /// </summary>
        public void FlushDeletedFiles()
        {
            Contract.Ensure(batchDeletedFiles, UltravioletStrings.ContentManagerNotBatchingDeletes);

            foreach (var file in filesPendingDeletion)
            {
                File.Delete(file);
            }
            filesPendingDeletion.Clear();
        }

        /// <summary>
        /// Resolves the full path to the file that represents the specified asset.
        /// </summary>
        /// <param name="asset">The asset path for which to resolve a file path.</param>
        /// <returns>The full path to the file that represents the specified asset.</returns>
        public String ResolveAssetFilePath(String asset)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var metadata = GetAssetMetadata(NormalizeAssetPath(asset), true, false);
            if (metadata == null)
                throw new FileNotFoundException(asset);

            return fileSystemService.GetFullPath(metadata.AssetFilePath);
        }

        /// <summary>
        /// Resolves the full path to the file that represents the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier for which to resolve a file path.</param>
        /// <returns>The full path to the file that represents the specified asset.</returns>
        public String ResolveAssetFilePath(AssetID asset)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var assetPath = AssetID.GetAssetPath(asset);

            var metadata = GetAssetMetadata(NormalizeAssetPath(assetPath), true, false);
            if (metadata == null)
                throw new FileNotFoundException(assetPath);

            return fileSystemService.GetFullPath(metadata.AssetFilePath);
        }

        /// <summary>
        /// Gets a list of assets in the specified asset directory.
        /// </summary>
        /// <param name="path">The directory to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of files in the path.</param>
        /// <returns>The list of assets in the specified asset directory.</returns>
        public IEnumerable<String> GetAssetsInDirectory(String path, String searchPattern = "*")
        {
            Contract.RequireNotEmpty(path, nameof(path));
            Contract.RequireNotEmpty(searchPattern, nameof(searchPattern));
            Contract.EnsureNotDisposed(this, Disposed);

            return GetAssetsInDirectoryInternal(NormalizeAssetPath(path), searchPattern).Keys;
        }

        /// <summary>
        /// Gets a list of resolved asset file paths in the specified asset directory.
        /// </summary>
        /// <param name="path">The directory to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of files in the path.</param>
        /// <returns>The list of resolved asset file paths in the specified asset directory.</returns>
        public IEnumerable<String> GetAssetFilePathsInDirectory(String path, String searchPattern = "*")
        {
            Contract.RequireNotEmpty(path, nameof(path));
            Contract.RequireNotEmpty(searchPattern, nameof(searchPattern));
            Contract.EnsureNotDisposed(this, Disposed);

            return GetAssetsInDirectoryInternal(NormalizeAssetPath(path), searchPattern).Values;
        }

        /// <summary>
        /// Gets a list of subdirectories in the specified asset directory.
        /// </summary>
        /// <param name="path">The directory to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in the path.</param>
        /// <returns>The list of subdirectories in the specified asset directory.</returns>
        public IEnumerable<String> GetSubdirectories(String path, String searchPattern = "*")
        {
            Contract.Require(path, nameof(path));
            Contract.EnsureNotDisposed(this, Disposed);

            var results = new Dictionary<String, String>();

            GetSubdirectories(RootDirectory, path, searchPattern, results);

            if (!Path.IsPathRooted(path))
            {
                foreach (var dir in overrideDirectories)
                {
                    GetSubdirectories(dir, path, searchPattern, results);
                }
            }

            return results.Keys;
        }

        /// <summary>
        /// Gets the content manager's root directory.
        /// </summary>
        public String RootDirectory
        {
            get { return rootDirectory; }
        }

        /// <summary>
        /// Gets the content manager's collection of override directories.
        /// </summary>
        /// <remarks>Override directories are alternative paths where the content manager will search for asset files.
        /// If multiple override directories contain an asset with the same path, directories which have higher indices 
        /// within this collection will take priority over directories with lower indices, "overriding" the other values
        /// of the asset in question. All override directories take precedence over the content manager's root directory.</remarks>
        public ContentOverrideDirectoryCollection OverrideDirectories
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return overrideDirectories;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content manager should batch file deletions.
        /// </summary>
        /// <remarks>When this property is set to <see langword="true"/>, <see cref="ContentManager"/> will not delete files
        /// immediately. Instead, it will buffer deletions until the batch is ended. This is useful when, for example,
        /// preprocessing a large number of files which depend on the same raw resources; batching deletes ensures that
        /// those raw resources remain on disk until all of the assets are preprocessed.</remarks>
        public Boolean BatchDeletedFiles
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return batchDeletedFiles;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureNot(batchDeletedFilesGuarantee, UltravioletStrings.ContentManagerRequiresBatch);

                if (batchDeletedFiles)
                {
                    FlushDeletedFiles();
                }
                batchDeletedFiles = value;
            }
        }

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Ultraviolet.Messages.Unsubscribe(this);

                foreach (var instance in assetCache)
                {
                    var disposable = instance.Value as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            lock (cacheSyncObject)
            {
                assetCache.Clear();
                assetFlags.Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Lists the assets which can serve as substitutions for the specified asset.
        /// </summary>
        /// <param name="path">The file path of the asset for which to list substitutions.</param>
        /// <param name="maxDensityBucket">The maximum density bucket to consider.</param>
        /// <returns>A collection containing the specified asset's possible substitution assets.</returns>
        private IEnumerable<String> ListPossibleSubstitutions(String path, ScreenDensityBucket maxDensityBucket)
        {
            var directory = Path.GetDirectoryName(path) ?? String.Empty;
            var filename = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            var substitutions =
                from bucket in ScreenDensityBuckets.OrderByDescending(x => x)
                where bucket <= maxDensityBucket
                let bucketname = ScreenDensityService.GetDensityBucketName(bucket)
                let bucketfile = String.Format("{0}-{1}{2}", filename, bucketname, extension)
                let bucketpath = Path.Combine(directory, bucketfile)
                where fileSystemService.FileExists(bucketpath)
                select fileSystemService.GetRelativePath(rootDirectory, bucketpath);

            return substitutions;
        }

        /// <summary>
        /// Finds the importer for the specified asset.
        /// </summary>
        /// <param name="asset">The asset for which to find an importer.</param>
        /// <param name="outputType">The importer's output type.</param>
        /// <returns>The importer for the specified asset.</returns>
        private IContentImporter FindContentImporter(String asset, out Type outputType)
        {
            var extension = Path.GetExtension(asset);
            if (String.IsNullOrEmpty(extension))
            {
                throw new InvalidOperationException(UltravioletStrings.ImporterNeedsExtension.Format(asset));
            }

            var importer = Ultraviolet.GetContent().Importers.FindImporter(extension, out outputType);
            if (importer == null)
            {
                throw new InvalidOperationException(UltravioletStrings.NoValidImporter.Format(asset));
            }

            return importer;
        }

        /// <summary>
        /// Finds the processor for the specified type.
        /// </summary>
        /// <param name="asset">The asset for which to find a processor.</param>
        /// <param name="inputType">The processor's input type.</param>
        /// <param name="outputType">The processor's output type.</param>
        /// <returns>The processor for the specified type.</returns>
        private IContentProcessor FindContentProcessor(String asset, Type inputType, Type outputType)
        {
            var processor = Ultraviolet.GetContent().Processors.FindProcessor(inputType, outputType);
            if (processor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidProcessor.Format(asset));

            return processor;
        }

        /// <summary>
        /// Loads or preprocesses the specified content file.
        /// </summary>
        /// <param name="asset">The asset to load.</param>
        /// <param name="type">The type of asset to load.</param>
        /// <param name="cache">A value indicating whether to add the loaded asset to the asset cache.</param>
        /// <param name="preprocess">A value indicating whether to preprocess the loaded asset.</param>
        /// <param name="delete">A value indicating whether to delete the original file after preprocessing it.</param>
        /// <param name="result">The asset that was loaded.</param>
        /// <returns><see langword="true"/> if the asset was loaded or preprocessed successfully; otherwise, <see langword="false"/>.</returns>
        private Boolean LoadInternal(String asset, Type type, Boolean cache, Boolean preprocess, Boolean delete, out Object result)
        {
            result = null;

            var normalizedAsset = NormalizeAssetPath(asset);

            var metadata = GetAssetMetadata(normalizedAsset, !preprocess);
            var preprocessed = IsPreprocessedFile(metadata.AssetFilePath);
            if (preprocess && preprocessed)
                return true;

            if (preprocess)
            {
                var intermediateObjectType = default(Type);
                var intermediate = Import<Object>(normalizedAsset, out intermediateObjectType);
                var processor = FindContentProcessor(normalizedAsset, intermediateObjectType, type);
                var preprocessSucceeded = PreprocessInternal(normalizedAsset, metadata, processor, intermediate, delete);
                if (preprocessSucceeded && delete)
                {
                    if (batchDeletedFiles)
                    {
                        filesPendingDeletion.Add(metadata.AssetFilePath);
                    }
                    else
                    {
                        File.Delete(metadata.AssetFilePath);
                    }
                }
                return preprocessSucceeded;
            }
            else
            {
                var importer = default(IContentImporter);
                var processor = default(IContentProcessor);
                var instance = preprocessed ?
                    LoadInternalPreprocessed(type, normalizedAsset, metadata.AssetFilePath, out importer, out processor) :
                    LoadInternalRaw(type, normalizedAsset, metadata, out importer, out processor);

                if (cache)
                {
                    lock (cacheSyncObject)
                    {
                        assetCache[asset] = instance;

                        if (!assetFlags.ContainsKey(asset))
                            assetFlags[asset] = AssetFlags.None;
                    }
                }

                result = instance;
                return true;
            }
        }

        /// <summary>
        /// Loads a raw asset from a stream.
        /// </summary>
        /// <param name="type">The type of asset to load.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the asset to load.</param>
        /// <param name="extension">The file extension to use to search for a content importer.</param>
        /// <returns>The asset that was loaded.</returns>
        private Object LoadInternalFromStream(Type type, Stream stream, String extension)
        {
            var filename = String.Format("__STREAM.{0}", extension);
            var importerOutputType = default(Type);
            var importer = FindContentImporter(filename, out importerOutputType);

            var intermediate = importer.Import(AssetMetadata.StreamMetadata, stream);
            try
            {
                if (intermediate == null)
                    throw new InvalidOperationException(UltravioletStrings.ImporterOutputInvalid.Format(filename));

                var processor = FindContentProcessor(filename, importerOutputType, type);

                return processor.Process(this, AssetMetadata.StreamMetadata, intermediate);
            }
            finally
            {
                var disposable = intermediate as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads a raw asset.
        /// </summary>
        /// <param name="type">The type of asset to load.</param>
        /// <param name="asset">The name of the asset being loaded.</param>
        /// <param name="metadata">The asset metadata.</param>
        /// <param name="importer">The content importer for this asset.</param>
        /// <param name="processor">The content processor for this asset.</param>
        /// <returns>The asset that was loaded.</returns>
        private Object LoadInternalRaw(Type type, String asset, AssetMetadata metadata, out IContentImporter importer, out IContentProcessor processor)
        {
            var importerOutputType = default(Type);
            importer = FindContentImporter(metadata.AssetFilePath, out importerOutputType);

            var intermediate = default(Object);
            try
            {
                using (var stream = fileSystemService.OpenRead(metadata.AssetFilePath))
                {
                    intermediate = importer.Import(metadata, stream);
                    if (intermediate == null)
                        throw new InvalidOperationException(UltravioletStrings.ImporterOutputInvalid.Format(asset));
                }

                processor = FindContentProcessor(metadata.AssetFilePath, importerOutputType, type);

                return processor.Process(this, metadata, intermediate);
            }
            finally
            {
                var disposable = intermediate as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads a preprocessed asset.
        /// </summary>
        /// <param name="type">The type of asset to load.</param>
        /// <param name="asset">The name of the asset being loaded.</param>
        /// <param name="path">The path to the asset file.</param>
        /// <param name="importer">The content importer for the asset.</param>
        /// <param name="processor">The content processor for the asset.</param>
        /// <returns>The asset that was loaded.</returns>
        private Object LoadInternalPreprocessed(Type type, String asset, String path, out IContentImporter importer, out IContentProcessor processor)
        {
            importer = null;
            using (var stream = fileSystemService.OpenRead(path))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var uvcHeader = reader.ReadString();
                    if (uvcHeader != "UVC0")
                        throw new InvalidDataException();

                    var uvcProcessorTypeName = reader.ReadString();
                    var uvcProcessorType = Type.GetType(uvcProcessorTypeName);

                    for (var currentType = uvcProcessorType; currentType != null; currentType = currentType.BaseType)
                    {
                        if (currentType.IsGenericType)
                        {
                            var genericTypeDef = currentType.GetGenericTypeDefinition();
                            if (genericTypeDef == typeof(ContentProcessor<,>))
                            {
                                if (!type.IsAssignableFrom(currentType.GetGenericArguments()[1]))
                                    throw new InvalidOperationException(UltravioletStrings.PreprocessedAssetTypeMismatch.Format(path, type.Name));

                                break;
                            }
                        }

                        if (currentType.BaseType == null)
                            throw new InvalidOperationException(UltravioletStrings.ProcessorInvalidBaseClass.Format(uvcProcessorType.FullName));
                    }


                    processor = (IContentProcessor)Activator.CreateInstance(uvcProcessorType);

                    var metadata = new AssetMetadata(asset, fileSystemService.GetFullPath(path), null, null, true, false);
                    return processor.ImportPreprocessed(this, metadata, reader);
                }
            }
        }

        /// <summary>
        /// Preprocesses an asset.
        /// </summary>
        /// <param name="asset">The name of the asset to preprocess.</param>
        /// <param name="metadata">The asset metadata.</param>
        /// <param name="processor">The content processor for the asset.</param>
        /// <param name="intermediate">The intermediate form of the asset to preprocess.</param>
        /// <param name="delete">A value indicating whether the original file will be deleted after preprocessing is complete.</param>
        /// <returns><see langword="true"/> if the asset was preprocessed; otherwise, <see langword="false"/>.</returns>
        private Boolean PreprocessInternal(String asset, AssetMetadata metadata, IContentProcessor processor, Object intermediate, Boolean delete)
        {
            if (!processor.SupportsPreprocessing)
                return false;

            using (var stream = File.Open(Path.ChangeExtension(metadata.AssetFilePath, PreprocessedFileExtension), FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var processorType = processor.GetType();
                    var processorTypeName = String.Format("{0}, {1}", processorType.FullName, processorType.Assembly.GetName().Name);

                    writer.Write("UVC0");
                    writer.Write(processorTypeName);

                    processor.ExportPreprocessed(this, metadata, writer, intermediate, delete);
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified file is preprocessed.
        /// </summary>
        /// <param name="filename">The filename to evaluate.</param>
        /// <returns><see langword="true"/> if the specified file is preprocessed; otherwise, <see langword="false"/>.</returns>
        private Boolean IsPreprocessedFile(String filename)
        {
            return Path.GetExtension(filename) == PreprocessedFileExtension;
        }

        /// <summary>
        /// Gets a value indicating whether the specified file contains asset metadata.
        /// </summary>
        /// <param name="filename">The filename to evaluate.</param>
        /// <param name="extension">The file extension of the metadata file.</param>
        /// <returns><see langword="true"/> if the specified file contains asset metadata; otherwise, <see langword="false"/>.</returns>
        private Boolean IsMetadataFile(String filename, out String extension)
        {
            extension = Path.GetExtension(filename);
            return
                extension == MetadataFileExtensionXml ||
                extension == MetadataFileExtensionJson;
        }

        /// <summary>
        /// Gets the path of the specified asset relative to the specified root directory.
        /// </summary>
        /// <param name="root">The root directory.</param>
        /// <param name="asset">The asset name.</param>
        /// <param name="extension">The required file extension, if any; otherwise, <see langword="null"/>.</param>
        /// <param name="flags">A collection of <see cref="AssetResolutionFlags"/> values indicating how to resolve the asset path.</param>
        /// <returns>The path of the specified asset relative to the specified root directory.</returns>
        private String GetAssetPathFromDirectory(String root, String asset, ref String extension, AssetResolutionFlags flags = AssetResolutionFlags.Default)
        {
            var includePreprocessed = (flags & AssetResolutionFlags.IncludePreprocessed) == AssetResolutionFlags.IncludePreprocessed;

            var assetPath = Path.GetDirectoryName(Path.Combine(root, asset));
            if (!fileSystemService.DirectoryExists(assetPath))
            {
                return null;
            }

            var assetNoExtension = Path.GetFileNameWithoutExtension(asset);
            var assetMatches =
                from file in fileSystemService.ListFiles(assetPath, assetNoExtension + ".*")
                let fileExtension = Path.GetExtension(file)
                where
                    includePreprocessed || !fileExtension.Equals(PreprocessedFileExtension, StringComparison.InvariantCultureIgnoreCase)
                select file;

            var filteredExtension = extension;
            var filteredMatches =
                from assetMatch in assetMatches
                let assetExtension = Path.GetExtension(assetMatch)
                where
                    filteredExtension == null || assetExtension.Equals(filteredExtension, StringComparison.InvariantCultureIgnoreCase)
                select assetMatch;

            if (filteredMatches.Count() > 1)
                throw new FileNotFoundException(UltravioletStrings.FileAmbiguous.Format(asset));

            var singleMatch = filteredMatches.SingleOrDefault();
            if (singleMatch != null)
            {
                extension = Path.GetExtension(singleMatch);
                return singleMatch;
            }
            return null;
        }

        /// <summary>
        /// Gets the path to the specified asset.
        /// </summary>
        /// <param name="asset">The asset name.</param>
        /// <param name="extension">The extension for which to search, or <see langword="null"/> to search for any extension.</param>
        /// <param name="directory">The directory in which the asset was found.</param>
        /// <param name="flags">A collection of <see cref="AssetResolutionFlags"/> values indicating how to resolve the asset path.</param>
        /// <returns>The path of the specified asset.</returns>
        private String GetAssetPath(String asset, String extension, out String directory, AssetResolutionFlags flags = AssetResolutionFlags.Default)
        {
            var specifiedExtension = Path.GetExtension(asset);
            if (extension == null && !String.IsNullOrWhiteSpace(specifiedExtension))
                extension = specifiedExtension;

            var path = GetAssetPathFromDirectory(RootDirectory, asset, ref extension, flags);
            directory = RootDirectory;

            foreach (var dir in OverrideDirectories)
            {
                var dirPath = GetAssetPathFromDirectory(dir, asset, ref extension, flags);
                if (dirPath != null)
                {
                    directory = dir;
                    path = dirPath;
                }
            }

            var performSubstitution = (flags & AssetResolutionFlags.PerformSubstitution) == AssetResolutionFlags.PerformSubstitution;
            if (performSubstitution && path != null && !Path.HasExtension(asset))
            {
                var primaryDisplay = Ultraviolet.GetPlatform().Displays.FirstOrDefault();
                if (primaryDisplay != null)
                {
                    var substitution = ListPossibleSubstitutions(path, primaryDisplay.DensityBucket)
                        .Take(1).SingleOrDefault();

                    if (substitution != null)
                    {
                        flags &= ~AssetResolutionFlags.PerformSubstitution;
                        return GetAssetPath(substitution, extension, out directory, flags);
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Gets the metadata for the specified asset.
        /// </summary>
        /// <param name="asset">The asset for which to find metadata.</param>
        /// <param name="includePreprocessedFiles">A value indicating whether to include preprocessed files in the search.</param>
        /// <param name="includeDetailedMetadata">A value indicating whether to include detailed metadata loaded from .uvmeta files.</param>
        /// <returns>The metadata for the specified asset.</returns>
        private AssetMetadata GetAssetMetadata(String asset, Boolean includePreprocessedFiles, Boolean includeDetailedMetadata = true)
        {
            // If we're given a full filename with extension, return that.
            var assetDirectory = String.Empty;
            var assetExtension = Path.GetExtension(asset);
            if (!String.IsNullOrEmpty(assetExtension))
            {
                var assetPath = GetAssetPath(asset, assetExtension, out assetDirectory);
                if (assetPath != null)
                {
                    if (includePreprocessedFiles || !IsPreprocessedFile(asset))
                    {
                        return CreateMetadataFromFile(asset, assetPath, assetDirectory, includeDetailedMetadata);
                    }
                }
                throw new FileNotFoundException(UltravioletStrings.FileNotFound.Format(asset));
            }

            // Find the highest-ranking preprocessed file, if one exists.
            if (includePreprocessedFiles)
            {
                var assetPathPreprocessed = GetAssetPath(asset, PreprocessedFileExtension, out assetDirectory);
                if (assetPathPreprocessed != null)
                    return CreateMetadataFromFile(asset, assetPathPreprocessed, assetDirectory, includeDetailedMetadata);
            }

            // Find the highest-ranking metadata file, if one exists.
            var assetPathMetadata = GetAssetPath(asset, MetadataFileExtensionXml, out assetDirectory, AssetResolutionFlags.PerformSubstitution);
            if (assetPathMetadata != null)
                return CreateMetadataFromFile(asset, assetPathMetadata, assetDirectory, includeDetailedMetadata);

            // Find the highest-ranking raw file.
            var assetPathRaw = GetAssetPath(asset, null, out assetDirectory, AssetResolutionFlags.PerformSubstitution);
            if (assetPathRaw != null)
                return CreateMetadataFromFile(asset, assetPathRaw, assetDirectory, includeDetailedMetadata);

            // If we still have no matches, we can't find the file.
            throw new FileNotFoundException(UltravioletStrings.FileNotFound.Format(asset));
        }

        /// <summary>
        /// Creates an asset metadata object from the specified asset file.
        /// </summary>
        /// <param name="asset">The normalized asset path.</param>
        /// <param name="filename">The filename of the file from which to create asset metadata.</param>
        /// <param name="rootdir">The root directory from which the file is being loaded.</param>
        /// <param name="includeDetailedMetadata">A value indicating whether to include detailed metadata loaded from .uvmeta files.</param>
        /// <returns>The asset metadata for the specified asset file.</returns>
        private AssetMetadata CreateMetadataFromFile(String asset, String filename, String rootdir, Boolean includeDetailedMetadata)
        {
            String extension;
            if (IsMetadataFile(filename, out extension) && includeDetailedMetadata)
            {
                var isJson = false;

                var wrappedFilename = default(String);
                var wrappedAssetPath = default(String);

                var importerMetadata = default(Object);
                var processorMetadata = default(Object);

                if (extension == MetadataFileExtensionXml)
                {
                    var xml = XDocument.Load(filename);

                    wrappedFilename = xml.Root.Elements().Where(x => x.Name.LocalName == "Asset").Single()?.Value;
                    importerMetadata = xml.Root.Elements().Where(x => x.Name.LocalName == "ImporterMetadata").SingleOrDefault();
                    processorMetadata = xml.Root.Elements().Where(x => x.Name.LocalName == "ProcessorMetadata").SingleOrDefault();
                }
                else
                {
                    using (var sreader = File.OpenText(filename))
                    using (var jreader = new JsonTextReader(sreader))
                    {
                        var json = (JObject)JToken.ReadFrom(jreader);
                        isJson = true;

                        wrappedFilename = json["asset"].Value<String>();
                        importerMetadata = (JObject)json["importerMetadata"];
                        processorMetadata = (JObject)json["processorMetadata"];
                    }
                }

                if (String.IsNullOrWhiteSpace(wrappedFilename) || String.IsNullOrWhiteSpace(Path.GetExtension(wrappedFilename)))
                    throw new InvalidDataException(UltravioletStrings.AssetMetadataHasInvalidFilename);

                var directory = Path.GetDirectoryName(filename);
                var relative = fileSystemService.GetRelativePath(rootDirectory, Path.Combine(directory, wrappedFilename));

                var wrappedAssetDirectory = String.Empty;
                wrappedAssetPath = GetAssetPath(relative, Path.GetExtension(relative), out wrappedAssetDirectory);

                if (!fileSystemService.FileExists(wrappedAssetPath))
                    throw new InvalidDataException(UltravioletStrings.AssetMetadataFileNotFound);

                return new AssetMetadata(asset, wrappedAssetPath, importerMetadata, processorMetadata, true, false, isJson);
            }
            return new AssetMetadata(asset, filename, null, null, true, false);
        }

        /// <summary>
        /// Gets a list of assets in the specified asset directory.
        /// </summary>
        /// <param name="path">The directory to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of files in the path.</param>
        /// <returns>The list of assets in the specified asset directory.</returns>
        private Dictionary<String, String> GetAssetsInDirectoryInternal(String path, String searchPattern)
        {
            var results = new Dictionary<String, String>();

            GetAssetsInDirectory(RootDirectory, path, searchPattern, results);

            if (!Path.IsPathRooted(path))
            {
                foreach (var dir in overrideDirectories)
                {
                    GetAssetsInDirectory(dir, path, searchPattern, results);
                }
            }

            return results;
        }

        /// <summary>
        /// Updates the specified result set with the list of assets at the specified path.
        /// </summary>
        /// <param name="directory">The root directory being examined.</param>
        /// <param name="path">The path relative to the root directory being examined.</param>
        /// <param name="searchPattern">The search string to match against the names of files in the path.</param>
        /// <param name="results">The result set to update.</param>
        private void GetAssetsInDirectory(String directory, String path, String searchPattern, Dictionary<String, String> results)
        {
            var root = String.IsNullOrEmpty(directory) ? fileSystemService.GetCurrentDirectory() : fileSystemService.GetFullPath(directory);

            var assetRoot = fileSystemService.GetRelativePath(fileSystemService.GetCurrentDirectory(), root);
            var assetDirectory = Path.Combine(assetRoot, path ?? String.Empty);
            if (!fileSystemService.DirectoryExists(assetDirectory))
                return;

            var assets =
                from f in fileSystemService.ListFiles(assetDirectory, searchPattern)
                let relative = fileSystemService.GetRelativePath(root, f)
                let absolute = fileSystemService.GetFullPath(Path.Combine(root, relative))
                select new { RelativePath = relative, AbsolutePath = absolute };

            foreach (var asset in assets)
            {
                results[asset.RelativePath] = asset.AbsolutePath;
            }
        }

        /// <summary>
        /// Updates the specified result set with the list of directories at the specified path.
        /// </summary>
        /// <param name="directory">The root directory being examined.</param>
        /// <param name="path">The path relative to the root directory being examined.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in the path.</param>
        /// <param name="results">The result set to update.</param>
        private void GetSubdirectories(String directory, String path, String searchPattern, Dictionary<String, String> results)
        {
            var root = String.IsNullOrEmpty(directory) ? fileSystemService.GetCurrentDirectory() : fileSystemService.GetFullPath(directory);

            var assetRoot = fileSystemService.GetRelativePath(fileSystemService.GetCurrentDirectory(), root);
            var assetDirectory = Path.Combine(assetRoot, path ?? String.Empty);
            if (!fileSystemService.DirectoryExists(assetDirectory))
                return;

            var assets =
                from d in fileSystemService.ListDirectories(assetDirectory, searchPattern)
                let relative = fileSystemService.GetRelativePath(root, d)
                let absolute = fileSystemService.GetFullPath(Path.Combine(root, relative))
                select new { RelativePath = relative, AbsolutePath = absolute };

            foreach (var asset in assets)
            {
                results[asset.RelativePath] = asset.AbsolutePath;
            }
        }

        /// <summary>
        /// Preprocesses the assets in the specified content manifests.
        /// </summary>
        /// <param name="manifests">A collection containing the content manifests to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original files after preprocessing them.</param>
        private void PreprocessInternal(IEnumerable<ContentManifest> manifests, Boolean delete)
        {
            var batch = false;
            if (!BatchDeletedFiles)
            {
                BatchDeletedFiles = true;
                batch = true;
            }
            batchDeletedFilesGuarantee = true;

            foreach (var manifest in manifests)
            {
                foreach (var group in manifest)
                {
                    foreach (var asset in group)
                    {
                        PreprocessInternal(asset.Type, asset.AbsolutePath, delete);
                    }
                }
            }

            if (batch)
            {
                batchDeletedFilesGuarantee = false;
                BatchDeletedFiles = false;
            }
        }

        /// <summary>
        /// Preprocesses the specified asset by saving it in a binary format designed for fast deserialization.
        /// If the asset's content importer does not support a binary data format, this method has no effect.
        /// </summary>
        /// <param name="type">The type of asset to preprocess.</param>
        /// <param name="asset">The asset to preprocess.</param>
        /// <param name="delete">A value indicating whether to delete the original file after preprocessing it.</param>
        /// <returns><see langword="true"/> if the asset was preprocessed; otherwise, <see langword="false"/>.</returns>
        private Boolean PreprocessInternal(Type type, String asset, Boolean delete)
        {
            Object result;

            var assetDirectory = String.Empty;
            var assetPath = GetAssetPath(asset, null, out assetDirectory, AssetResolutionFlags.None);

            if (IsPreprocessedFile(assetPath))
                return true;

            var substitutions = ListPossibleSubstitutions(assetPath, ScreenDensityBucket.ExtraExtraExtraHigh);
            foreach (var substitution in substitutions)
            {
                if (!LoadInternal(substitution, type, false, true, delete, out result))
                {
                    return false;
                }
            }

            return LoadInternal(asset, type, false, true, delete, out result);
        }

        // Property values.
        private readonly String rootDirectory;

        // State values.
        private readonly ContentOverrideDirectoryCollection overrideDirectories = new ContentOverrideDirectoryCollection();
        private readonly Dictionary<String, Object> assetCache = new Dictionary<String, Object>();
        private readonly Dictionary<String, AssetFlags> assetFlags = new Dictionary<String, AssetFlags>();
        private readonly FileSystemService fileSystemService;
        private readonly Object cacheSyncObject = new Object();

        // The file extensions associated with preprocessed binary data and asset metadata files.
        private const String PreprocessedFileExtension = ".uvc";
        private const String MetadataFileExtensionXml = ".uvmeta";
        private const String MetadataFileExtensionJson = ".jsmeta";

        // Files waiting to be deleted as part of a batch.
        private readonly List<String> filesPendingDeletion = new List<String>();
        private Boolean batchDeletedFiles;
        private Boolean batchDeletedFilesGuarantee;

        // The supported screen density buckets.
        private static readonly IEnumerable<ScreenDensityBucket> ScreenDensityBuckets;
    }
}
