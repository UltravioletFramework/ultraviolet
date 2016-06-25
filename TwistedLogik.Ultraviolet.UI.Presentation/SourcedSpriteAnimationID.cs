using System;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an animation identifier which is flagged as being either globally- or locally-sourced.
    /// </summary>
    [JsonConverter(typeof(ObjectResolverJsonConverter))]
    public struct SourcedSpriteAnimationID : IEquatable<SourcedSpriteAnimationID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedSpriteAnimationID"/> class.
        /// </summary>
        /// <param name="spriteAnimationID">The sprite animation's identifier.</param>
        /// <param name="spriteSource">The sprite asset's source.</param>
        [Preserve]
        public SourcedSpriteAnimationID(SpriteAnimationID spriteAnimationID, AssetSource spriteSource)
        {
            this.spriteAnimationID = spriteAnimationID;
            this.spriteSource = spriteSource;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the specified sprite animation identifiers are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedSpriteAnimationID"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedSpriteAnimationID"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified identifiers are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(SourcedSpriteAnimationID id1, SourcedSpriteAnimationID id2)
        {
            return id1.spriteAnimationID.Equals(id2.spriteAnimationID) && id1.spriteSource == id2.spriteSource;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the specified sprite animation identifiers are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedSpriteAnimationID"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedSpriteAnimationID"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified identifiers are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(SourcedSpriteAnimationID id1, SourcedSpriteAnimationID id2)
        {
            return !id1.spriteAnimationID.Equals(id2.spriteAnimationID) || id1.spriteSource != id2.spriteSource;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of
        /// the <see cref="SourcedSpriteAnimationID"/> structure using the content manifest registry 
        /// provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <returns>An instance of the <see cref="SourcedSpriteAnimationID"/> structure that is equivalent to the specified string.</returns>
        [Preserve]
        public static SourcedSpriteAnimationID Parse(String s)
        {
            Contract.Require(s, nameof(s));

            SourcedSpriteAnimationID value;
            if (!TryParseInternal(null, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of 
        /// the <see cref="SourcedSpriteAnimationID"/> structure using the content manifest registry 
        /// provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedSpriteAnimationID"/> structure that is equivalent to the specified string.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out SourcedSpriteAnimationID value)
        {
            Contract.Require(s, nameof(s));

            return TryParseInternal(null, s, out value);
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of 
        /// the <see cref="SourcedSpriteAnimationID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <returns>An instance of the <see cref="SourcedSpriteAnimationID"/> structure that is equivalent to the specified string.</returns>
        [Preserve]
        public static SourcedSpriteAnimationID Parse(ContentManifestRegistry manifests, String s)
        {
            Contract.Require(manifests, nameof(manifests));
            Contract.Require(s, nameof(s));

            SourcedSpriteAnimationID value;
            if (!TryParseInternal(manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of an sprite animation identifier to an instance of 
        /// the <see cref="SourcedAssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedAssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(ContentManifestRegistry manifests, String s, out SourcedSpriteAnimationID value)
        {
            Contract.Require(manifests, nameof(manifests));
            Contract.Require(s, nameof(s));

            return TryParseInternal(manifests, s, out value);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + spriteAnimationID.GetHashCode();
                hash = hash * 23 + spriteSource.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0} {1}", SpriteAnimationID, SpriteSource.ToString().ToLowerInvariant());
        }

        /// <inheritdoc/>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            return obj is SourcedAssetID && Equals((SourcedAssetID)obj);
        }

        /// <inheritdoc/>
        [Preserve]
        public Boolean Equals(SourcedSpriteAnimationID other)
        {
            return
                this.spriteAnimationID.Equals(other.spriteAnimationID) &&
                this.spriteSource == other.spriteSource;
        }

        /// <summary>
        /// Gets the sprite animation's identifier.
        /// </summary>
        public SpriteAnimationID SpriteAnimationID
        {
            get { return spriteAnimationID; }
        }

        /// <summary>
        /// Gets the sprite's source.
        /// </summary>
        public AssetSource SpriteSource
        {
            get { return spriteSource; }
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of 
        /// the <see cref="SourcedSpriteAnimationID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedSpriteAnimationID"/> structure that is equivalent to the specified string.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        private static Boolean TryParseInternal(ContentManifestRegistry manifests, String s, out SourcedSpriteAnimationID value)
        {
            var parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2)
                throw new FormatException();

            // Parse the asset identifier
            var spriteAnimationID = default(SpriteAnimationID);
            var assetIDParsed = false;

            if (manifests == null)
            {
                assetIDParsed = Graphics.Graphics2D.SpriteAnimationID.TryParse(parts[0], out spriteAnimationID);
            }
            else
            {
                assetIDParsed = Graphics.Graphics2D.SpriteAnimationID.TryParse(manifests, parts[0], out spriteAnimationID);
            }

            if (!assetIDParsed)
            {
                value = default(SourcedSpriteAnimationID);
                return false;
            }

            // Parse the asset source
            AssetSource spriteSource = AssetSource.Global;
            if (parts.Length == 2)
            {
                if (!Enum.TryParse(parts[1], true, out spriteSource))
                {
                    value = default(SourcedSpriteAnimationID);
                    return false;
                }
            }

            value = new SourcedSpriteAnimationID(spriteAnimationID, spriteSource);
            return true;
        }

        // Property values.
        private readonly SpriteAnimationID spriteAnimationID;
        private readonly AssetSource spriteSource;
    }
}
