using System;
using System.IO;
using Ultraviolet.Core;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Manages asset dependency relationships for an instance of the <see cref="ContentManager"/> class.
    /// </summary>
    public class ContentDependencyManager : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDependencyManager"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="contentManager">The content manager that owns this dependency manager.</param>
        internal ContentDependencyManager(UltravioletContext uv, ContentManager contentManager)
            : base(uv)
        {
            Contract.Require(contentManager, nameof(contentManager));

            this.ContentManager = contentManager;
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            AddAssetDependencyInternal(asset, dependency, primaryDisplayDensity);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            AddAssetDependencyInternal(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency), primaryDisplayDensity);
        }

        /// <summary>
        /// Adds the specified dependency to an asset. If the asset is being watched for changes, then any
        /// changes to the specified dependency will also cause the main asset to be reloaded.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to add a dependency.</param>
        /// <param name="dependency">The asset path of the dependency to add to the specified asset.</param>
        /// <param name="density">The screen density of the assets for which to add a dependency.</param>
        public void AddAssetDependency(String asset, String dependency, ScreenDensityBucket density)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            AddAssetDependencyInternal(asset, dependency, density);
        }

        /// <summary>
        /// Adds the specified dependency to an asset. If the asset is being watched for changes, then any
        /// changes to the specified dependency will also cause the main asset to be reloaded.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to add a dependency.</param>
        /// <param name="dependency">The asset identifier of the dependency to add to the specified asset.</param>
        /// <param name="density">The screen density of the assets for which to add a dependency.</param>
        public void AddAssetDependency(AssetID asset, AssetID dependency, ScreenDensityBucket density)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Ensure<ArgumentException>(dependency.IsValid, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            AddAssetDependencyInternal(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency), density);
        }

        /// <summary>
        /// Clears all of the registered dependencies for the specified asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to clear dependencies.</param>
        public void ClearAssetDependencies(String asset)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (ContentManager.AssetCache.SyncObject)
            {
                if (assetDependencies == null)
                    return;

                foreach (var dependents in assetDependencies)
                    dependents.Value.DependentAssets.Remove(asset);
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

            lock (ContentManager.AssetCache.SyncObject)
                assetDependencies?.Clear();
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return RemoveAssetDependencyInternal(asset, dependency, primaryDisplayDensity);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return RemoveAssetDependencyInternal(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency), primaryDisplayDensity);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return IsAssetDependencyInternal(asset, dependency, primaryDisplayDensity);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return IsAssetDependencyInternal(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency), primaryDisplayDensity);
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset path of the main asset to evaluate.</param>
        /// <param name="dependency">The asset path of the dependency asset to evaluate.</param>
        /// <param name="density">The screen density for which to query dependency relationships.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependency(String asset, String dependency, ScreenDensityBucket density)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependencyInternal(asset, dependency, density);
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the main asset to evaluate.</param>
        /// <param name="dependency">The asset identifier of the dependency asset to evaluate.</param>
        /// <param name="density">The screen density for which to query dependency relationships.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependency(AssetID asset, AssetID dependency, ScreenDensityBucket density)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Ensure<ArgumentException>(dependency.IsValid, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependencyInternal(AssetID.GetAssetPath(asset), AssetID.GetAssetPath(dependency), density);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return IsAssetDependencyPathInternal(asset, dependency, primaryDisplayDensity);
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

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return IsAssetDependencyPathInternal(AssetID.GetAssetPath(asset), dependency, primaryDisplayDensity);
        }

        /// <summary>
        /// Gets a value indicating whether the specified file path is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset path of the main asset to evaluate.</param>
        /// <param name="dependency">The file path of the dependency asset to evaluate.</param>
        /// <param name="density">The screen density for which to query dependency relationships.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependencyPath(String asset, String dependency, ScreenDensityBucket density)
        {
            Contract.Require(asset, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependencyPathInternal(asset, dependency, density);
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the main asset to evaluate.</param>
        /// <param name="dependency">The file path of the dependency asset to evaluate.</param>
        /// <param name="density">The screen density for which to query dependency relationships.</param>
        /// <returns><see langword="true"/> if <paramref name="dependency"/> is a dependency of <paramref name="asset"/>; otherwise, <see langword="false"/>.</returns>
        public Boolean IsAssetDependencyPath(AssetID asset, String dependency, ScreenDensityBucket density)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));
            Contract.Require(dependency, nameof(dependency));
            Contract.EnsureNotDisposed(this, Disposed);

            return IsAssetDependencyPathInternal(AssetID.GetAssetPath(asset), dependency, density);
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> that owns this dependency manager.
        /// </summary>
        public ContentManager ContentManager { get; }

        /// <summary>
        /// Gets the collection of asset dependencies for the specified file.
        /// </summary>
        /// <param name="fullPath">The full path to the asset file.</param>
        /// <returns>An <see cref="IAssetDependencyCollection"/> that contains the file's asset dependencies.</returns>
        internal IAssetDependencyCollection GetAssetDependenciesForFile(String fullPath)
        {
            if (assetDependencies != null && assetDependencies.TryGetValue(fullPath, out var assetDependenciesForFile))
                return assetDependenciesForFile;

            return null;
        }

        /// <summary>
        /// Adds the specified dependency to an asset. If the asset is being watched for changes, then any
        /// changes to the specified dependency will also cause the main asset to be reloaded.
        /// </summary>
        private Boolean AddAssetDependencyInternal(String asset, String dependency, ScreenDensityBucket density)
        {
            if (IsDependencyTrackingSuppressed)
                return false;

            var dependencyAssetPath = dependency;
            var dependencyAssetResolvedPath = ContentManager.ResolveAssetFilePath(dependencyAssetPath, density, true);
            var dependencyAssetFilePath = Path.GetFullPath(dependencyAssetResolvedPath);

            lock (ContentManager.AssetCache.SyncObject)
            {
                if (assetDependencies == null)
                    assetDependencies = new Dictionary<String, IAssetDependencyCollection>();

                assetDependencies.TryGetValue(dependencyAssetFilePath, out var dependents);

                if (dependents == null)
                {
                    dependents = new AssetDependencyCollection(ContentManager, dependencyAssetPath, dependencyAssetFilePath);
                    assetDependencies[dependencyAssetFilePath] = dependents;
                }

                dependents.DependentAssets.Add(asset);
            }

            return true;
        }

        /// <summary>
        /// Removes the specified dependency from an asset.
        /// </summary>
        private Boolean RemoveAssetDependencyInternal(String asset, String dependency, ScreenDensityBucket density)
        {
            var dependencyAssetFilePath = Path.GetFullPath(ContentManager.ResolveAssetFilePath(dependency, density, true));

            lock (ContentManager.AssetCache.SyncObject)
            {
                if (assetDependencies == null)
                    return false;

                if (assetDependencies.TryGetValue(dependencyAssetFilePath, out var dependents))
                    return dependents.DependentAssets.Remove(asset);
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        private Boolean IsAssetDependencyInternal(String asset, String dependency, ScreenDensityBucket density)
        {
            var dependencyAssetResolvedPath = ContentManager.ResolveAssetFilePath(dependency, density, true);
            var dependencyAssetFilePath = Path.GetFullPath(dependencyAssetResolvedPath);

            lock (ContentManager.AssetCache.SyncObject)
            {
                if (assetDependencies == null)
                    return false;

                if (assetDependencies.TryGetValue(dependencyAssetFilePath, out var dependents))
                    return dependents.DependentAssets.Contains(asset);
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset is registered as a dependency of another asset.
        /// </summary>
        private Boolean IsAssetDependencyPathInternal(String asset, String dependency, ScreenDensityBucket density)
        {
            var dependencyAssetResolvedPath = ContentManager.ResolveAssetFilePath(dependency, density, true);
            var dependencyAssetFilePath = Path.GetFullPath(dependencyAssetResolvedPath);

            lock (ContentManager.AssetCache.SyncObject)
            {
                if (assetDependencies == null)
                    return false;

                if (assetDependencies.TryGetValue(dependencyAssetFilePath, out var dependents))
                    return dependents.DependentAssets.Contains(asset);
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether dependency tracking is suppressed for this content manager.
        /// </summary>
        private Boolean IsDependencyTrackingSuppressed
        {
            get
            {
                return (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS) ||
                    ContentManager.GloballySuppressDependencyTracking || ContentManager.SuppressDependencyTracking;
            }
        }

        // Content dependencies.
        private Dictionary<String, IAssetDependencyCollection> assetDependencies;
    }
}
