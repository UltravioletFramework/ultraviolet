using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains extensoin methods for the <see cref="BinaryWriter"/> class.
    /// </summary>
    public static class Graphics2DBinaryWriterExtensions
    {
        /// <summary>
        /// Writes a sprite animation identifier to the stream.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> with which to write the sprite animation identifier.</param>
        /// <param name="id">The <see cref="SpriteAnimationID"/> to write to the stream.</param>
        public static void Write(this BinaryWriter writer, SpriteAnimationID id)
        {
            writer.Write(id.IsValid);
            if (id.IsValid)
            {
                writer.Write(SpriteAnimationID.GetSpriteAssetIDRef(ref id));
                writer.Write(SpriteAnimationID.GetAnimationNameRef(ref id));
                writer.Write(SpriteAnimationID.GetAnimationIndexRef(ref id));
            }
        }

        /// <summary>
        /// Writes a sprite animation identifier to the stream.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryReader"/> with which to write the sprite animation identifier.</param>
        /// <param name="id">The <see cref="System.Nullable{SpriteAnimationID}"/> to write to the stream.</param>
        public static void Write(this BinaryWriter writer, SpriteAnimationID? id)
        {
            writer.Write(id.HasValue);
            if (id.HasValue)
            {
                writer.Write(id.GetValueOrDefault());
            }
        }
    }
}
