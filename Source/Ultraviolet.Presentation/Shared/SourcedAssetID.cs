using System;
using Newtonsoft.Json;
using Ultraviolet.Content;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset identifier which is flagged as being either globally- or locally-sourced.
    /// </summary>
    [JsonConverter(typeof(ObjectResolverJsonConverter))]
    public partial struct SourcedAssetID : IEquatable<SourcedAssetID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedAssetID"/> class.
        /// </summary>
        /// <param name="assetID">The asset's identifier.</param>
        /// <param name="assetSource">The asset's source.</param>
        public SourcedAssetID(AssetID assetID, AssetSource assetSource)
        {
            this.assetID = assetID;
            this.assetSource = assetSource;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{AssetID} {AssetSource.ToString().ToLowerInvariant()}";

        /// <summary>
        /// Gets the asset's identifier.
        /// </summary>
        public AssetID AssetID
        {
            get { return assetID; }
        }

        /// <summary>
        /// Gets the asset's source.
        /// </summary>
        public AssetSource AssetSource
        {
            get { return assetSource; }
        }

        // Property values.
        private readonly AssetID assetID;
        private readonly AssetSource assetSource;
    }
}
