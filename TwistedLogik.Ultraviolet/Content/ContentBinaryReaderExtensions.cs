using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Contains extension methods for the <see cref="System.IO.BinaryReader"/> class.
    /// </summary>
    public static class ContentBinaryReaderExtensions
    {
        /// <summary>
        /// Reads an asset identifier from the stream using the content manifest registry
        /// belonging to the current Ultraviolet context.
        /// </summary>
        /// <param name="reader">The binary reader from which to read the asset identifier.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure representing the 
        /// asset identifier that was read from the stream.</returns>
        public static AssetID ReadAssetID(this BinaryReader reader)
        {
            Contract.Require(reader, nameof(reader));

            return ReadAssetID(reader, UltravioletContext.DemandCurrent().GetContent().Manifests);
        }

        /// <summary>
        /// Reads a nullable asset identifier from the stream using the content manifest registry
        /// belonging to the current Ultraviolet context.
        /// </summary>
        /// <param name="reader">The binary reader from which to read the asset identifier.</param>
        /// <returns>An instance of the <see cref="System.Nullable{AssetID}"/> structure representing the 
        /// asset identifier that was read from the stream.</returns>
        public static AssetID? ReadNullableAssetID(this BinaryReader reader)
        {
            Contract.Require(reader, nameof(reader));

            return ReadNullableAssetID(reader, UltravioletContext.DemandCurrent().GetContent().Manifests);
        }

        /// <summary>
        /// Reads an asset identifier from the stream.
        /// </summary>
        /// <param name="reader">The binary reader from which to read the asset identifier.</param>
        /// <param name="manifests">The registry that contains the application's loaded manifests.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure representing the
        /// asset identifier that was read from the stream.</returns>
        public static AssetID ReadAssetID(this BinaryReader reader, ContentManifestRegistry manifests)
        {
            Contract.Require(reader, nameof(reader));
            Contract.Require(manifests, nameof(manifests));

            var valid = reader.ReadBoolean();
            if (valid)
            {
                var manifestName = reader.ReadString();
                var manifestGroupName = reader.ReadString();
                var assetName = reader.ReadString();

                var manifest = manifests[manifestName];
                if (manifest == null)
                    return AssetID.Invalid;

                var manifestGroup = manifest[manifestGroupName];
                if (manifestGroup == null)
                    return AssetID.Invalid;

                var asset = manifestGroup[assetName];
                if (asset == null)
                    return AssetID.Invalid;

                return asset.CreateAssetID();
            }
            return AssetID.Invalid;
        }

        /// <summary>
        /// Reads a nullable asset identifier from the stream.
        /// </summary>
        /// <param name="reader">The binary reader from which to read the asset identifier.</param>
        /// <param name="manifests">The registry that contains the application's loaded manifests.</param>
        /// <returns>An instance of the <see cref="System.Nullable{AssetID}"/> structure representing the
        /// asset identifier that was read from the stream.</returns>
        public static AssetID? ReadNullableAssetID(this BinaryReader reader, ContentManifestRegistry manifests)
        {
            Contract.Require(reader, nameof(reader));
            Contract.Require(manifests, nameof(manifests));

            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadAssetID(manifests);
            }
            return null;
        }
    }
}
