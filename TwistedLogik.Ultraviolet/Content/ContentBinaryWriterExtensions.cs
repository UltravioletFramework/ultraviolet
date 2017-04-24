using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Contains extensoin methods for the <see cref="System.IO.BinaryReader"/> class.
    /// </summary>
    public static class ContentBinaryWriterExtensions
    {
        /// <summary>
        /// Writes an asset identifier to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the asset identifier.</param>
        /// <param name="id">The <see cref="AssetID"/> structure to write to the stream.</param>
        public static void Write(this BinaryWriter writer, AssetID id)
        {
            writer.Write(id.IsValid);
            if (id.IsValid)
            {
                writer.Write(AssetID.GetManifestNameRef(ref id));
                writer.Write(AssetID.GetManifestGroupRef(ref id));
                writer.Write(AssetID.GetAssetNameRef(ref id));
            }
        }

        /// <summary>
        /// Writes an asset identifier to the stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to write the asset identifier.</param>
        /// <param name="id">The <see cref="System.Nullable{AssetID}"/> structure to write to the stream.</param>
        public static void Write(this BinaryWriter writer, AssetID? id)
        {
            writer.Write(id.HasValue);
            if (id.HasValue)
            {
                writer.Write(id.GetValueOrDefault());
            }
        }
    }
}
