using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a value which identifies an asset within one of the application's content manifests.
    /// </summary>
    [JsonConverter(typeof(UltravioletJsonConverter))]
    public partial struct AssetID : IEquatable<AssetID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifestName">The name of the content manifest that contains the asset.</param>
        /// <param name="manifestGroup">The name of the content manifest group that contains the asset.</param>
        /// <param name="assetName">The asset's name within its content manifest group.</param>
        /// <param name="assetPath">The asset's path as specified by its content manifest.</param>
        /// <param name="assetIndex">The asset's index within its content manifest group.</param>
        internal AssetID(String manifestName, String manifestGroup, String assetName, String assetPath, Int32 assetIndex)
        {
            Contract.RequireNotEmpty(manifestName, nameof(manifestName));
            Contract.RequireNotEmpty(manifestGroup, nameof(manifestGroup));
            Contract.RequireNotEmpty(assetName, nameof(assetName));
            Contract.EnsureRange(assetIndex >= 0, nameof(assetIndex));

            this.manifestName = manifestName;
            this.manifestGroup = manifestGroup;
            this.assetName = assetName;
            this.assetPath = assetPath;
            this.assetIndex = assetIndex;
        }
        
        /// <summary>
        /// Gets the name of the content manifest that contains the specified asset.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The name of the content manifest that contains the specified asset.</returns>
        public static String GetManifestName(AssetID id)
        {
            return id.manifestName;
        }

        /// <summary>
        /// Gets the name of the content manifest that contains the specified asset.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The name of the content manifest that contains the specified asset.</returns>
        public static String GetManifestNameRef(ref AssetID id)
        {
            return id.manifestName;
        }

        /// <summary>
        /// Gets the name of the content manifest group that contains the specified asset.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The name of the content manifest group that contains the specified asset.</returns>
        public static String GetManifestGroup(AssetID id)
        {
            return id.manifestGroup;
        }

        /// <summary>
        /// Gets the name of the content manifest group that contains the specified asset.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The name of the content manifest group that contains the specified asset.</returns>
        public static String GetManifestGroupRef(ref AssetID id)
        {
            return id.manifestGroup;
        }

        /// <summary>
        /// Gets the specified asset's name within its content manifest group.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's name within its content manifest group.</returns>
        public static String GetAssetName(AssetID id)
        {
            return id.assetName;
        }

        /// <summary>
        /// Gets the specified asset's name within its content manifest group.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's name within its content manifest group.</returns>
        public static String GetAssetNameRef(ref AssetID id)
        {
            return id.assetName;
        }

        /// <summary>
        /// Gets the specified asset's full path relative to the content root directory.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's full path relative to the content root directory.</returns>
        public static String GetAssetPath(AssetID id)
        {
            return id.assetPath;
        }

        /// <summary>
        /// Gets the specified asset's full path relative to the content root directory.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's full path relative to the content root directory.</returns>
        public static String GetAssetPathRef(ref AssetID id)
        {
            return id.assetPath;
        }

        /// <summary>
        /// Gets the specified asset's index within its content manifest group.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's index within its content manifest group.</returns>
        public static Int32 GetAssetIndex(AssetID id)
        {
            return id.assetIndex;
        }

        /// <summary>
        /// Gets the specified asset's index within its content manifest group.
        /// </summary>
        /// <param name="id">The identifier of the asset to evaluate.</param>
        /// <returns>The specified asset's index within its content manifest group.</returns>
        public static Int32 GetAssetIndexRef(ref AssetID id)
        {
            return id.assetIndex;
        }
        
        /// <summary>
        /// Gets an invalid asset identifier.
        /// </summary>
        public static AssetID Invalid
        {
            get { return new AssetID(); }
        }

        /// <inheritdoc/>
        public override String ToString() => IsValid ? $"#{manifestName}:{manifestGroup}:{assetName}" : "#INVALID";

        /// <summary>
        /// Gets a value indicating whether this is a valid asset identifier.
        /// </summary>
        public Boolean IsValid
        {
            get { return assetPath != null; }
        }
        
        // Property values.
        private readonly String manifestName;
        private readonly String manifestGroup;
        private readonly String assetName;
        private readonly String assetPath;
        private readonly Int32 assetIndex;
    }
}
