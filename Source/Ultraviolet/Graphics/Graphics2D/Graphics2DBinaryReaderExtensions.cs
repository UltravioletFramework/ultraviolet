using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains extension methods for the <see cref="BinaryReader"/> class.
    /// </summary>
    public static class Graphics2DBinaryReaderExtensions
    {
        /// <summary>
        /// Reads a sprite animation identifier from the stream using the content manifest registry
        /// belonging to the current Ultraviolet context.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> which to read the sprite animation identifier.</param>
        /// <returns>The <see cref="SpriteAnimationID"/> that was read from the stream.</returns>
        public static SpriteAnimationID ReadSpriteAnimationID(this BinaryReader reader)
        {
            Contract.Require(reader, nameof(reader));

            return ReadSpriteAnimationID(reader, UltravioletContext.DemandCurrent().GetContent().Manifests);
        }

        /// <summary>
        /// Reads a nullable asset identifier from the stream using the content manifest registry
        /// belonging to the current Ultraviolet context.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the sprite animation identifier.</param>
        /// <returns>The <see cref="Nullable{SpriteAnimationID}"/> identifier that was read from the stream.</returns>
        public static SpriteAnimationID? ReadNullableSpriteAnimationID(this BinaryReader reader)
        {
            Contract.Require(reader, nameof(reader));

            return ReadNullableSpriteAnimationID(reader, UltravioletContext.DemandCurrent().GetContent().Manifests);
        }

        /// <summary>
        /// Reads a sprite animation identifier from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the sprite animation identifier.</param>
        /// <param name="manifests">The <see cref="ContentManifestRegistry"/> that contains the application's loaded manifests.</param>
        /// <returns>The <see cref="SpriteAnimationID"/> that was read from the stream.</returns>
        public static SpriteAnimationID ReadSpriteAnimationID(this BinaryReader reader, ContentManifestRegistry manifests)
        {
            Contract.Require(reader, nameof(reader));
            Contract.Require(manifests, nameof(manifests));

            var valid = reader.ReadBoolean();
            if (valid)
            {
                var spriteAssetID = reader.ReadAssetID();
                var animationName = reader.ReadString();
                var animationIndex = reader.ReadInt32();

                return String.IsNullOrEmpty(animationName) ? 
                    new SpriteAnimationID(spriteAssetID, animationIndex) :
                    new SpriteAnimationID(spriteAssetID, animationName);
            }
            return SpriteAnimationID.Invalid;
        }

        /// <summary>
        /// Reads a nullable asset identifier from the stream.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> from which to read the sprite animation identifier.</param>
        /// <param name="manifests">The <see cref="ContentManifestRegistry"/> that contains the application's loaded manifests.</param>
        /// <returns>The <see cref="SpriteAnimationID"/> that was read from the stream.</returns>
        public static SpriteAnimationID? ReadNullableSpriteAnimationID(this BinaryReader reader, ContentManifestRegistry manifests)
        {
            Contract.Require(reader, nameof(reader));
            Contract.Require(manifests, nameof(manifests));

            var hasValue = reader.ReadBoolean();
            if (hasValue)
            {
                return reader.ReadSpriteAnimationID(manifests);
            }
            return null;
        }
    }
}
