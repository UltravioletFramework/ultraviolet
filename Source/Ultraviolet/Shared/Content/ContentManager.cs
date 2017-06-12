using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Core.Xml;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
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
            this.fullRootDirectory = Path.GetFullPath(rootDirectory);
            this.fileSystemService = FileSystemService.Create();
            this.overrideDirectories = new ContentOverrideDirectoryCollection(this);

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
        /// Purges the specified asset from the content manager's internal cache.
        /// </summary>
        /// <param name="asset">The asset to purge from the cache.</param>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void PurgeCache(String asset, Boolean lowMemory)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
            {
                if (!GetAssetFlags(asset, out var flags))
                    return;

                var preserve = (flags & AssetFlags.PreserveThroughLowMemory) == AssetFlags.PreserveThroughLowMemory;
                if (preserve && lowMemory)
                    return;

                assetCache.Remove(asset);
                assetFlags.Remove(asset);
                ClearAssetDependencies(asset);
            }
        }

        /// <summary>
        /// Purges the specified asset from the content manager's internal cache.
        /// </summary>
        /// <param name="asset">The asset to purge from the cache.</param>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void PurgeCache(AssetID asset, Boolean lowMemory)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            PurgeCache(AssetID.GetAssetPath(asset), lowMemory);
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
                    if (preserve && lowMemory)
                        continue;

                    assetCache.Remove(asset);
                    assetFlags.Remove(asset);
                    ClearAssetDependencies(asset);
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
                    LoadInternal(asset.AbsolutePath, asset.Type, true, false, out result);
                }
            }
        }
        
        /// <summary>
        /// Adds a watcher for the specified asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to add a watcher.</param>
        /// <param name="watcher">The watcher to add for the specified asset.</param>
        public void AddWatcher<TOutput>(String asset, AssetWatcher<TOutput> watcher)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(watcher, nameof(watcher));
            Contract.EnsureNotDisposed(this, Disposed);

            CreateFileSystemWatchers();

            lock (cacheSyncObject)
            {
                if (watchers == null)
                    watchers = new Dictionary<String, IAssetWatcherCollection>();

                var assetPath = asset;
                var assetFilePath = Path.GetFullPath(ResolveAssetFilePath(assetPath, true));

                if (!watchers.TryGetValue(assetFilePath, out var list))
                {
                    list = new AssetWatcherCollection<TOutput>(this, assetPath, assetFilePath);
                    watchers[assetFilePath] = list;
                }

                list.Add(watcher);
            }
        }

        /// <summary>
        /// Adds a watcher for the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to add a watcher.</param>
        /// <param name="watcher">The watcher to add for the specified asset.</param>
        public void AddWatcher<TOutput>(AssetID asset, AssetWatcher<TOutput> watcher)
        {
            Contract.Require(watcher, nameof(watcher));
            Contract.EnsureNotDisposed(this, Disposed);

            AddWatcher(AssetID.GetAssetPath(asset), watcher);
        }

        /// <summary>
        /// Removes a watcher from the specified asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to remove a watcher.</param>
        /// <param name="watcher">The watcher to remove from the specified asset.</param>
        /// <returns><see langword="true"/> if the specified watcher was removed; otherwise, <see langword="false"/>.</returns>
        public Boolean RemoveWatcher<TOutput>(String asset, AssetWatcher<TOutput> watcher)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(watcher, nameof(watcher));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
            {
                if (watchers == null)
                    return false;

                var assetFilePath = ResolveAssetFilePath(asset, true);

                if (!watchers.TryGetValue(asset, out var list))
                    return false;

                return list.Remove(watcher);
            }
        }

        /// <summary>
        /// Removes a watcher from the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to remove a watcher.</param>
        /// <param name="watcher">The watcher to remove from the specified asset.</param>
        /// <returns><see langword="true"/> if the specified watcher was removed; otherwise, <see langword="false"/>.</returns>
        public Boolean RemoveWatcher<TOutput>(AssetID asset, AssetWatcher<TOutput> watcher)
        {
            Contract.Require(watcher, nameof(watcher));
            Contract.EnsureNotDisposed(this, Disposed);

            return RemoveWatcher(AssetID.GetAssetPath(asset), watcher);
        }
        
        /// <summary>
        /// Adds the specified dependency to an asset. If the asset is being watched for changes, then any
        /// changes to the specified dependency will also cause the main asset to be reloaded.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to add a dependency.</param>
        /// <param name="dependency">The asset path of the dependency to add to the specified asset.</param>
        public void AddAssetDependency(String asset, String dependency)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            if (GloballySuppressDependencyTracking || suppressDependencyTracking)
                return;

            dependency = Path.GetFullPath(ResolveAssetFilePath(dependency, true));

            lock (cacheSyncObject)
            {
                if (!assetDependencies.TryGetValue(dependency, out var dependents))
                    assetDependencies[dependency] = dependents = new HashSet<String>();
                
                dependents.Add(asset);
            }
        }

        /// <summary>
        /// Adds the specified dependency to an asset. If the asset is being watched for changes, then any
        /// changes to the specified dependency will also cause the main asset to be reloaded.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to add a dependency.</param>
        /// <param name="dependency">The asset identifier of the dependency to add to the specified asset.</param>
        public void AddAssetDependency(AssetID asset, AssetID dependency)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Ensure<ArgumentException>(dependency.IsValid, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            AddAssetDependency(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency));
        }

        /// <summary>
        /// Clears all of the registered dependencies for the specified asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to clear dependencies.</param>
        public void ClearAssetDependencies(String asset)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
            {
                if (assetDependencies == null)
                    return;
                
                foreach (var dependents in assetDependencies)
                    dependents.Value.Remove(asset);
            }
        }

        /// <summary>
        /// Clears all of the registered dependencies for the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to clear dependencies.</param>
        public void ClearAssetDependencies(AssetID asset)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            ClearAssetDependencies(AssetID.GetAssetPath(asset));
        }

        /// <summary>
        /// Clears all of the registered dependencies for this content manager.
        /// </summary>
        public void ClearAssetDependencies()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (cacheSyncObject)
            {
                if (assetDependencies != null)
                    assetDependencies.Clear();
            }
        }

        /// <summary>
        /// Removes the specified dependency from an asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset from which to remove a dependency.</param>
        /// <param name="dependency">The asset path of the dependency to remove from the specified asset.</param>
        /// <returns></returns>
        public Boolean RemoveAssetDependency(String asset, String dependency)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            dependency = Path.GetFullPath(ResolveAssetFilePath(dependency, true));

            lock (cacheSyncObject)
            {
                if (assetDependencies == null)
                    return false;

                if (assetDependencies.TryGetValue(dependency, out var dependents))
                    return dependents.Remove(asset);
            }

            return false;
        }

        /// <summary>
        /// Removes the specified dependency from an asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to remove a dependency.</param>
        /// <param name="dependency">The asset identifier of the dependency to remove from the specified asset.</param>
        public Boolean RemoveAssetDependency(AssetID asset, AssetID dependency)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Ensure<ArgumentException>(dependency.IsValid, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return RemoveAssetDependency(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency));
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset path of the main asset to evaluate.</param>
        /// <param name="dependency">The asset path of the dependency asset to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependency(String asset, String dependency)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            dependency = Path.GetFullPath(ResolveAssetFilePath(dependency, true));

            lock (cacheSyncObject)
            {
                if (assetDependencies == null)
                    return false;
                
                if (assetDependencies.TryGetValue(dependency, out var dependents))
                    return dependents.Contains(asset);
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the main asset to evaluate.</param>
        /// <param name="dependency">The asset identifier of the dependency asset to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependency(AssetID asset, AssetID dependency)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Ensure<ArgumentException>(dependency.IsValid, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependency(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency));
        }

        /// <summary>
        /// Gets a value indicating whether the specified file path is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset path of the main asset to evaluate.</param>
        /// <param name="dependency">The file path of the dependency asset to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependencyPath(String asset, String dependency)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            dependency = Path.GetFullPath(dependency);

            lock (cacheSyncObject)
            {
                if (assetDependencies == null)
                    return false;

                if (!assetDependencies.TryGetValue(dependency, out var dependents))
                    return false;
                
                return dependents.Contains(asset);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the main asset to evaluate.</param>
        /// <param name="dependency">The file path of the dependency asset to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependencyPath(AssetID asset, String dependency)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependencyPath(AssetID.GetAssetPath(asset), dependency);
        }

        /// <summary>
        /// Gets a <see cref="WatchedAsset{T}"/> which watches the specified asset. The <see cref="WatchedAsset{T}"/> which is returned
        /// is owned by the content manager and shared between all callers of this method. If the watched asset has not already been
        /// loaded, it will be loaded and added to the content manager's internal cache.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The path to the asset to load.</param>
        /// <returns>The <see cref="WatchedAsset{T}"/> instance which this content manager uses to watch the specified asset.</returns>
        public WatchedAsset<TOutput> GetSharedWatchedAsset<TOutput>(String asset)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            CreateFileSystemWatchers();

            lock (cacheSyncObject)
            {
                if (sharedWatchedAssets.TryGetValue(asset, out var watcher))
                    return (WatchedAsset<TOutput>)watcher;

                watcher = new WatchedAsset<TOutput>(this, asset);
                sharedWatchedAssets[asset] = watcher;
                return (WatchedAsset<TOutput>)watcher;
            }
        }

        /// <summary>
        /// Gets a <see cref="WatchedAsset{T}"/> which watches the specified asset. The <see cref="WatchedAsset{T}"/> which is returned
        /// is owned by the content manager and shared between all callers of this method. If the watched asset has not already been
        /// loaded, it will be loaded and added to the content manager's internal cache.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The <see cref="WatchedAsset{T}"/> instance which this content manager uses to watch the specified asset.</returns>
        public WatchedAsset<TOutput> GetSharedWatchedAsset<TOutput>(AssetID asset)
        {
            return GetSharedWatchedAsset<TOutput>(AssetID.GetAssetPath(asset));
        }

        /// <summary>
        /// Loads the specified asset file.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling Load() multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="asset">The path to the asset to load.</param>
        /// <param name="cache">A value indicating whether to add the asset to the manager's cache.</param>
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The asset that was loaded from the specified file.</returns>
        public TOutput Load<TOutput>(String asset, Boolean cache = true, Boolean fromsln = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return LoadImpl<TOutput>(asset, cache, fromsln);
        }

        /// <summary>
        /// Loads the specified asset file.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling Load() multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="asset">The path to the asset to load.</param>
        /// <param name="cache">A value indicating whether to add the asset to the manager's cache.</param>
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The asset that was loaded from the specified file.</returns>
        public TOutput Load<TOutput>(AssetID asset, Boolean cache = true, Boolean fromsln = false)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            return LoadImpl<TOutput>(AssetID.GetAssetPath(asset), cache, fromsln);
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
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(String asset, Boolean fromsln = false)
        {
            return Import<TOutput>(asset, fromsln, out var outputType);
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
            return Import<TOutput>(asset, false, out outputType);
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <param name="outputType">The output type of the content importer which was used.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(String asset, Boolean fromsln, out Type outputType)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var metadata = GetAssetMetadata(asset, false, true, fromsln);
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
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(AssetID asset, Boolean fromsln = false)
        {
            return Import<TOutput>(asset, fromsln, out var outputType);
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

            return Import<TOutput>(AssetID.GetAssetPath(asset), false, out outputType);
        }

        /// <summary>
        /// Imports the specified asset, but does not process it.
        /// </summary>
        /// <typeparam name="TOutput">The type of the intermediate object produced by the content importer.</typeparam>
        /// <param name="asset">The path to the asset to import.</param>
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <param name="outputType">The output type of the content importer which was used.</param>
        /// <returns>The imported asset in its intermediate form.</returns>
        public TOutput Import<TOutput>(AssetID asset, Boolean fromsln, out Type outputType)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            return Import<TOutput>(AssetID.GetAssetPath(asset), fromsln, out outputType);
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
            var assetmeta = (metadata == null) ? AssetMetadata.InMemoryMetadata : new AssetMetadata(null, null, null, null, metadata, false, false, false, false);
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
            var assetmeta = (metadata == null) ? AssetMetadata.InMemoryMetadata : new AssetMetadata(null, null, null, null, metadata, false, false, false, false);
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

            return PreprocessInternal(asset, typeof(TOutput), delete);
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

            return PreprocessInternal(AssetID.GetAssetPath(asset), typeof(TOutput), delete);
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
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The full path to the file that represents the specified asset.</returns>
        public String ResolveAssetFilePath(String asset, Boolean fromsln = false)
        {
            Contract.RequireNotEmpty(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var metadata = GetAssetMetadata(NormalizeAssetPath(asset), true, false, fromsln);
            if (metadata == null)
                throw new FileNotFoundException(asset);

            return fileSystemService.GetFullPath(metadata.AssetFilePath);
        }

        /// <summary>
        /// Resolves the full path to the file that represents the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier for which to resolve a file path.</param>
        /// <param name="fromsln">A value indicating whether asset resolution should search the Visual Studio solution
        /// directory, rather than the directory containing the application binaries. This is useful primarily for reloading
        /// assets while the application is being debugged, and should mostly be avoided unless you know what you're doing.</param>
        /// <returns>The full path to the file that represents the specified asset.</returns>
        public String ResolveAssetFilePath(AssetID asset, Boolean fromsln = false)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var assetPath = AssetID.GetAssetPath(asset);

            var metadata = GetAssetMetadata(NormalizeAssetPath(assetPath), true, false, fromsln);
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
        /// Gets or sets a value indicating whether dependency tracking is globally suppressed, reducing memory usage.
        /// As a side effect, modifying a dependency of an asset will no longer cause the parent asset to be reloaded
        /// in an application which makes use of watched assets.
        /// </summary>
        /// <remarks>Setting this property to <see langword="true"/> will not purge any existing dependency tracking caches,
        /// so if you turn this on, be sure to do it before loading any content assets.</remarks>
        public static Boolean GloballySuppressDependencyTracking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dependency tracking is suppressed for this content manager, reducing memory usage.
        /// As a side effect, modifying a dependency of an asset will no longer cause the parent asset to be reloaded
        /// in an application which makes use of watched assets.
        /// </summary>
        /// <remarks>Setting this property to <see langword="true"/> will not purge any existing dependency tracking caches,
        /// so if you turn this on, be sure to do it before loading any content assets.</remarks>
        public Boolean SuppressDependencyTracking
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return suppressDependencyTracking;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                suppressDependencyTracking = value;
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
        /// Gets the content manager's root directory.
        /// </summary>
        public String RootDirectory
        {
            get { return rootDirectory; }
        }

        /// <summary>
        /// Gets the full path to the content manager's root directory.
        /// </summary>
        public String FullRootDirectory
        {
            get { return fullRootDirectory; }
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
        /// Implements the <see cref="Load"/> method.
        /// </summary>
        internal TOutput LoadImpl<TOutput>(String asset, Boolean cache, Boolean fromsln)
        {
            return LoadImpl<TOutput>(asset, cache, fromsln, null, default(TOutput));
        }
        
        /// <summary>
        /// Implements the <see cref="Load"/> method.
        /// </summary>
        internal TOutput LoadImpl<TOutput>(String asset, Boolean cache, Boolean fromsln, IAssetWatcherCollection watchers, TOutput lastKnownGood)
        {
            var cachedInstance = default(Object);
            var cacheMiss = false;

            lock (cacheSyncObject)
                cacheMiss = !assetCache.TryGetValue(asset, out cachedInstance);

            if (cacheMiss)
            {
                LoadInternal(asset, typeof(TOutput), cache, fromsln, watchers, lastKnownGood, out var result);
                return (result is ContentCacheData ccd) ? (TOutput)ccd.Asset : (TOutput)result;
            }
            else
            {
                return (cachedInstance is ContentCacheData ccd) ? (TOutput)ccd.Asset : (TOutput)cachedInstance;
            }
        }
        
        /// <summary>
        /// Called when a change in the file system is detected.
        /// </summary>
        internal void OnFileSystemChanged(Object sender, FileSystemEventArgs e)
        {
            OnFileReloaded(e.FullPath);
        }

        /// <summary>
        /// Called when a change in a file is detected.
        /// </summary>
        internal void OnFileReloaded(String fullPath)
        {
            if (watchers != null && watchers.TryGetValue(fullPath, out var list))
                list.OnChanged(fullPath);

            if (assetDependencies != null && assetDependencies.TryGetValue(fullPath, out var dependents))
            {
                foreach (var dependent in dependents)
                {
                    var dependentFullPath = Path.GetFullPath(ResolveAssetFilePath(dependent, true));
                    OnFileReloaded(dependentFullPath);
                }
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

                lock (cacheSyncObject)
                {
                    foreach (var instance in assetCache)
                    {
                        var disposable = ((instance.Value is ContentCacheData ccd) ? ccd.Asset : instance.Value) as IDisposable;
                        disposable?.Dispose();
                    }

                    assetCache.Clear();
                    assetFlags.Clear();

                    foreach (var kvp in sharedWatchedAssets)
                        ((IDisposable)kvp.Value).Dispose();

                    sharedWatchedAssets.Clear();

                }
                if (rootFileSystemWatcher != null)
                    rootFileSystemWatcher.Dispose();

                OverrideDirectories.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a set of file system watchers to monitor the content manager's
        /// file system for changes.
        /// </summary>
        private void CreateFileSystemWatchers()
        {
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
                return;

            if (rootFileSystemWatcher == null)
            {
                var rootdir = FindSolutionDirectory() ?? RootDirectory;
                rootFileSystemWatcher = new FileSystemWatcher(rootdir);
                rootFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                rootFileSystemWatcher.IncludeSubdirectories = true;
                rootFileSystemWatcher.EnableRaisingEvents = true;
                rootFileSystemWatcher.Changed += OnFileSystemChanged;
            }

            OverrideDirectories.CreateFileSystemWatchers();
        }

        /// <summary>
        /// Lists the assets which can serve as substitutions for the specified asset.
        /// </summary>
        /// <param name="rootdir">The root directory from which content is being loaded.</param>
        /// <param name="path">The file path of the asset for which to list substitutions.</param>
        /// <param name="maxDensityBucket">The maximum density bucket to consider.</param>
        /// <returns>A collection containing the specified asset's possible substitution assets.</returns>
        private IEnumerable<String> ListPossibleSubstitutions(String rootdir, String path, ScreenDensityBucket maxDensityBucket)
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
                select fileSystemService.GetRelativePath(rootdir, bucketpath);

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
        /// Loads the specified content file.
        /// </summary>
        private Boolean LoadInternal(String asset, Type type, Boolean cache, Boolean fromsln, out Object result)
        {
            return LoadInternal(asset, type, cache, fromsln, null, null, out result);
        }
        
        /// <summary>
        /// Loads the specified content file.
        /// </summary>
        private Boolean LoadInternal(String asset, Type type, Boolean cache, Boolean fromsln, IAssetWatcherCollection watchers, Object lastKnownGood, out Object result)
        {
            result = null;

            var normalizedAsset = NormalizeAssetPath(asset);

            var metadata = GetAssetMetadata(normalizedAsset, true, true, fromsln);
            var preprocessed = IsPreprocessedFile(metadata.AssetFilePath);
            var importer = default(IContentImporter);
            var processor = default(IContentProcessor);
            var instance = default(Object);
            var changed = true;
            
            try
            {
                instance = preprocessed ?
                    LoadInternalPreprocessed(type, normalizedAsset, metadata.AssetFilePath, metadata.OverrideDirectory, out importer, out processor) :
                    LoadInternalRaw(type, normalizedAsset, metadata, out importer, out processor);
            }
            catch (Exception e)
            {
                if (watchers == null || watchers.Count == 0 || lastKnownGood == null)
                    throw;

                Debug.WriteLine(UltravioletStrings.ExceptionDuringContentReloading);
                Debug.WriteLine(e);

                instance = lastKnownGood;
                changed = false;
            }

            if (changed)
            {
                if (cache)
                    UpdateCache(asset, metadata, ref instance);

                if (watchers != null)
                {
                    var validated = true;

                    for (var i = 0; i < watchers.Count; i++)
                    {
                        if (!watchers[i].OnValidating(asset, instance))
                        {
                            if (instance is IDisposable disposable)
                                disposable.Dispose();

                            validated = false;
                            instance = lastKnownGood;

                            if (cache)
                                UpdateCache(asset, metadata, ref instance);

                            for (int j = 0; j <= i; j++)
                                watchers[i].OnValidationComplete(asset, instance, false);

                            validated = false;
                            changed = false;

                            break;
                        }
                    }

                    if (validated)
                    {
                        for (int i = 0; i < watchers.Count; i++)
                            watchers[i].OnValidationComplete(asset, instance, true);
                    }
                }

                if (changed)
                {
                    ClearAssetDependencies(asset);

                    foreach (var dependency in metadata.AssetDependencies)
                        AddAssetDependency(asset, dependency);
                }
            }

            result = instance;
            return true;
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
        /// <param name="overridedir">The override directory from which to load the asset.</param>
        /// <param name="importer">The content importer for the asset.</param>
        /// <param name="processor">The content processor for the asset.</param>
        /// <returns>The asset that was loaded.</returns>
        private Object LoadInternalPreprocessed(Type type, String asset, String path, String overridedir, out IContentImporter importer, out IContentProcessor processor)
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

                    var metadata = new AssetMetadata(overridedir, asset, fileSystemService.GetFullPath(path), null, null, true, false, false, false);
                    return processor.ImportPreprocessed(this, metadata, reader);
                }
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
                        PreprocessInternal(asset.AbsolutePath, asset.Type, delete);
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
        /// Preprocesses an asset.
        /// </summary>
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
        /// Preprocesses the specified asset by saving it in a binary format designed for fast deserialization.
        /// If the asset's content importer does not support a binary data format, this method has no effect.
        /// </summary>
        private Boolean PreprocessInternal(String asset, Type type, Boolean delete)
        {
            Object result;

            var assetDirectory = String.Empty;
            var assetOveridden = false;
            var assetPath = GetAssetPath(asset, null, out assetDirectory, out assetOveridden, AssetResolutionFlags.None);

            if (IsPreprocessedFile(assetPath))
                return true;

            var substitutions = ListPossibleSubstitutions(assetDirectory, assetPath, ScreenDensityBucket.ExtraExtraExtraHigh);
            foreach (var substitution in substitutions)
            {
                if (!PreprocessInternal(substitution, type, delete, false, out result))
                {
                    return false;
                }
            }

            return PreprocessInternal(asset, type, delete, false, out result);
        }

        /// <summary>
        /// Preprocesses the specified asset by saving it in a binary format designed for fast deserialization.
        /// If the asset's content importer does not support a binary data format, this method has no effect.
        /// </summary>
        private Boolean PreprocessInternal(String asset, Type type, Boolean delete, Boolean fromsln, out Object result)
        {
            result = null;

            var normalizedAsset = NormalizeAssetPath(asset);

            var metadata = GetAssetMetadata(normalizedAsset, false, true, fromsln);
            var preprocessed = IsPreprocessedFile(metadata.AssetFilePath);
            if (preprocessed)
                return true;

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
        /// Attempts to find the directory that contains the application solution file, if 
        /// we have reason to believe that we're running in debug mode.
        /// </summary>
        /// <returns>The solution directory, if it was found; otherwise, <see langword="null"/>.</returns>
        private String FindSolutionDirectory()
        {
            if (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS)
                return null;

            var asm = Assembly.GetEntryAssembly();
            if (asm == null)
                return null;

            var asmDebuggableAttr = (DebuggableAttribute)asm.GetCustomAttributes(typeof(DebuggableAttribute), false).FirstOrDefault();
            if (asmDebuggableAttr == null || !asmDebuggableAttr.IsJITOptimizerDisabled)
                return null;

            var asmDir = new DirectoryInfo(Path.GetDirectoryName(asm.Location));
            if (asmDir == null || !asmDir.Exists)
                return null;

            // NOTE: I have absolutely no idea whether these directory names are
            // localized in non-English versions of Visual Studio, so I won't
            // bother checking them... let's just assume we're in the right place
            // unless we have a specific reason to think otherwise.

            // Break out of "Debug"/"Release"/etc.
            asmDir = asmDir.Parent;
            if (asmDir == null || !asmDir.Exists)
                return null;

            // Break out of "bin"   
            asmDir = asmDir.Parent;
            if (asmDir == null || !asmDir.Exists)
                return null;

            // Is there a directory here with the same structure as our content root?
            var projectContentRoot = new DirectoryInfo(Path.Combine(asmDir.FullName, RootDirectory));
            if (projectContentRoot == null || !projectContentRoot.Exists)
                return null;

            return projectContentRoot.FullName;
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
        /// <param name="overridden">A value indicating whether the asset was loaded from an override directory.</param>
        /// <param name="flags">A collection of <see cref="AssetResolutionFlags"/> values indicating how to resolve the asset path.</param>
        /// <returns>The path of the specified asset.</returns>
        private String GetAssetPath(String asset, String extension, out String directory, out Boolean overridden, AssetResolutionFlags flags = AssetResolutionFlags.Default)
        {
            var specifiedExtension = Path.GetExtension(asset);
            if (extension == null && !String.IsNullOrWhiteSpace(specifiedExtension))
                extension = specifiedExtension;

            var rootdir = (flags & AssetResolutionFlags.LoadFromSolutionDirectory) == AssetResolutionFlags.LoadFromSolutionDirectory ?
                FindSolutionDirectory() ?? RootDirectory : RootDirectory;
            var path = GetAssetPathFromDirectory(rootdir, asset, ref extension, flags);
            directory = rootdir;
            overridden = false;

            foreach (var dir in OverrideDirectories)
            {
                var dirPath = GetAssetPathFromDirectory(dir, asset, ref extension, flags);
                if (dirPath != null)
                {
                    directory = dir;
                    path = dirPath;
                    overridden = true;
                }
            }

            var performSubstitution = (flags & AssetResolutionFlags.PerformSubstitution) == AssetResolutionFlags.PerformSubstitution;
            if (performSubstitution && path != null && !Path.HasExtension(asset))
            {
                var primaryDisplay = Ultraviolet.GetPlatform().Displays.FirstOrDefault();
                if (primaryDisplay != null)
                {
                    var substitution = ListPossibleSubstitutions(directory, path, primaryDisplay.DensityBucket)
                        .Take(1).SingleOrDefault();

                    if (substitution != null)
                    {
                        flags &= ~AssetResolutionFlags.PerformSubstitution;
                        path = GetAssetPathFromDirectory(directory, substitution, ref extension, flags);
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
        /// <param name="fromsln">A value indicating whether to attempt to load the asset from the Visual Studio solution directory.</param>
        /// <returns>The metadata for the specified asset.</returns>
        private AssetMetadata GetAssetMetadata(String asset, Boolean includePreprocessedFiles, Boolean includeDetailedMetadata, Boolean fromsln)
        {
            // If we're given a full filename with extension, return that.
            var assetDirectory = String.Empty;
            var assetOverridden = false;
            var assetExtension = Path.GetExtension(asset);
            if (!String.IsNullOrEmpty(assetExtension))
            {
                var assetPath = GetAssetPath(asset, assetExtension, out assetDirectory, out assetOverridden, AssetResolutionFlags.Default |
                    (fromsln ? AssetResolutionFlags.LoadFromSolutionDirectory : AssetResolutionFlags.None));
                if (assetPath != null)
                {
                    if (includePreprocessedFiles || !IsPreprocessedFile(asset))
                        return CreateMetadataFromFile(asset, assetPath, assetDirectory, assetOverridden, includeDetailedMetadata, fromsln);
                }
                throw new FileNotFoundException(UltravioletStrings.FileNotFound.Format(asset));
            }

            // Find the highest-ranking preprocessed file, if one exists.
            if (includePreprocessedFiles)
            {
                var assetPathPreprocessed = GetAssetPath(asset, PreprocessedFileExtension, out assetDirectory, out assetOverridden, AssetResolutionFlags.Default |
                    (fromsln ? AssetResolutionFlags.LoadFromSolutionDirectory : AssetResolutionFlags.None));
                if (assetPathPreprocessed != null)
                {
                    return CreateMetadataFromFile(asset, assetPathPreprocessed, assetDirectory, assetOverridden, includeDetailedMetadata, fromsln);
                }
            }

            // Find the highest-ranking metadata file, if one exists.
            var assetPathMetadata = GetAssetPath(asset, MetadataFileExtensionXml, out assetDirectory, out assetOverridden, AssetResolutionFlags.PerformSubstitution |
                (fromsln ? AssetResolutionFlags.LoadFromSolutionDirectory : AssetResolutionFlags.None));
            if (assetPathMetadata != null)
            {
                return CreateMetadataFromFile(asset, assetPathMetadata, assetDirectory, assetOverridden, includeDetailedMetadata, fromsln);
            }

            // Find the highest-ranking raw file.
            var assetPathRaw = GetAssetPath(asset, null, out assetDirectory, out assetOverridden, AssetResolutionFlags.PerformSubstitution | 
                (fromsln ? AssetResolutionFlags.LoadFromSolutionDirectory : AssetResolutionFlags.None));
            if (assetPathRaw != null)
            {
                return CreateMetadataFromFile(asset, assetPathRaw, assetDirectory, assetOverridden, includeDetailedMetadata, fromsln);
            }

            // If we still have no matches, we can't find the file.
            throw new FileNotFoundException(UltravioletStrings.FileNotFound.Format(asset));
        }

        /// <summary>
        /// Creates an asset metadata object from the specified asset file.
        /// </summary>
        /// <param name="asset">The normalized asset path.</param>
        /// <param name="filename">The filename of the file from which to create asset metadata.</param>
        /// <param name="rootdir">The root directory from which the file is being loaded.</param>
        /// <param name="overridden">A value indicating whether the asset was loaded from an override directory.</param>
        /// <param name="includeDetailedMetadata">A value indicating whether to include detailed metadata loaded from .uvmeta files.</param>
        /// <param name="fromsln">A value indicating whether to attempt to load the asset from the Visual Studio solution directory.</param>
        /// <returns>The asset metadata for the specified asset file.</returns>
        private AssetMetadata CreateMetadataFromFile(String asset, String filename, String rootdir, Boolean overridden, Boolean includeDetailedMetadata, Boolean fromsln)
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

                    wrappedFilename = xml.Root.ElementValueString("Asset");
                    importerMetadata = xml.Root.Element("ImporterMetadata");
                    processorMetadata = xml.Root.Element("ProcessorMetadata");
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
                var wrappedAssetOverridden = false;
                wrappedAssetPath = GetAssetPath(relative, Path.GetExtension(relative), out wrappedAssetDirectory, out wrappedAssetOverridden);

                if (!fileSystemService.FileExists(wrappedAssetPath))
                    throw new InvalidDataException(UltravioletStrings.AssetMetadataFileNotFound);

                return new AssetMetadata(wrappedAssetOverridden ? wrappedAssetDirectory : null,
                    asset, wrappedAssetPath, importerMetadata, processorMetadata, true, false, isJson, fromsln);
            }
            return new AssetMetadata(overridden ? rootdir : null, asset, filename, null, null, true, false, false, fromsln);
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
        /// Updates the content manager's internal cache with the specified object instance.
        /// </summary>
        private void UpdateCache(String asset, AssetMetadata metadata, ref Object instance)
        {
            lock (cacheSyncObject)
            {
                if (metadata.IsOverridden)
                {
                    instance = new ContentCacheData(instance, metadata.OverrideDirectory);
                }
                assetCache[asset] = instance;

                if (!assetFlags.ContainsKey(asset))
                    assetFlags[asset] = AssetFlags.None;
            }
        }

        // Property values.
        private readonly String rootDirectory;
        private readonly String fullRootDirectory;

        // State values.
        private readonly ContentOverrideDirectoryCollection overrideDirectories;
        private readonly Dictionary<String, Object> assetCache = new Dictionary<String, Object>();
        private readonly Dictionary<String, AssetFlags> assetFlags = new Dictionary<String, AssetFlags>();
        private readonly Dictionary<String, HashSet<String>> assetDependencies = new Dictionary<String, HashSet<String>>();
        private readonly Dictionary<String, Object> sharedWatchedAssets = new Dictionary<String, Object>();
        private readonly FileSystemService fileSystemService;
        private readonly Object cacheSyncObject = new Object();
        private Boolean suppressDependencyTracking;

        // File watching.
        private FileSystemWatcher rootFileSystemWatcher;
        private Dictionary<String, IAssetWatcherCollection> watchers;

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
