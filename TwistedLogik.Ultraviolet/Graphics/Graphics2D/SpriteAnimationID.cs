using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a value which identifies a particular sprite animation.
    /// </summary>
    public struct SpriteAnimationID : IEquatable<SpriteAnimationID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationID"/> structure from the specified animation name.
        /// </summary>
        /// <param name="spriteAssetID">The <see cref="AssetID"/> that represents the sprite that contains the animation.</param>
        /// <param name="animationName">The name of the referenced animation.</param>
        internal SpriteAnimationID(AssetID spriteAssetID, String animationName)
        {
            this.spriteAssetID = spriteAssetID;
            this.animationName = animationName;
            this.animationIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationID"/> structure from the specified animation name.
        /// </summary>
        /// <param name="spriteAssetID">The <see cref="AssetID"/> that represents the sprite that contains the animation.</param>
        /// <param name="animationIndex">The index of the referenced animation.</param>
        internal SpriteAnimationID(AssetID spriteAssetID, Int32 animationIndex)
        {
            this.spriteAssetID = spriteAssetID;
            this.animationName = null;
            this.animationIndex = animationIndex;
        }

        /// <summary>
        /// Compares two identifiers for equality.
        /// </summary>
        /// <param name="id1">The first identifier to compare.</param>
        /// <param name="id2">The second identifier to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(SpriteAnimationID id1, SpriteAnimationID id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Compares two identifiers for inequality.
        /// </summary>
        /// <param name="id1">The first identifier to compare.</param>
        /// <param name="id2">The second identifier to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(SpriteAnimationID id1, SpriteAnimationID id2)
        {
            return !id1.Equals(id2);
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of the SpriteAnimationID structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <returns>An instance of the SpriteAnimationID structure that is equivalent to the specified string.</returns>
        public static SpriteAnimationID Parse(String s)
        {
            Contract.Require(s, "s");

            SpriteAnimationID value;
            if (!TryParseInternal(UltravioletContext.DemandCurrent().GetContent().Manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of the SpriteAnimationID structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <param name="value">An instance of the SpriteAnimationID structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out SpriteAnimationID value)
        {
            Contract.Require(s, "s");

            var uv = UltravioletContext.DemandCurrent();
            return TryParseInternal(uv.GetContent().Manifests, s, out value);
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of the SpriteAnimationID structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <returns>An instance of the SpriteAnimationID structure that is equivalent to the specified string.</returns>
        public static SpriteAnimationID Parse(ContentManifestRegistry manifests, String s)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            SpriteAnimationID value;
            if (!TryParseInternal(manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation identifier to an instance of the SpriteAnimationID structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the sprite animation identifier to convert.</param>
        /// <param name="value">An instance of the SpriteAnimationID structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(ContentManifestRegistry manifests, String s, out SpriteAnimationID value)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            return TryParseInternal(manifests, s, out value);
        }

        /// <summary>
        /// Gets the asset identifier of the sprite that contains the specified animation.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The asset identifier of the sprite that contains the specified animation.</returns>
        public static AssetID GetSpriteAssetID(SpriteAnimationID id)
        {
            return id.spriteAssetID;
        }

        /// <summary>
        /// Gets the asset identifier of the sprite that contains the specified animation.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The asset identifier of the sprite that contains the specified animation.</returns>
        public static AssetID GetSpriteAssetIDRef(ref SpriteAnimationID id)
        {
            return id.spriteAssetID;
        }

        /// <summary>
        /// Gets the name of the specified animation.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The name of the specified animation.</returns>
        public static String GetAnimationName(SpriteAnimationID id)
        {
            return id.animationName;
        }
        
        /// <summary>
        /// Gets the name of the specified animation.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The name of the specified animation.</returns>
        public static String GetAnimationNameRef(ref SpriteAnimationID id)
        {
            return id.animationName;
        }

        /// <summary>
        /// Gets the name of the specified animation within its sprite's animation list.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The name of the specified animation within its sprite's animation list.</returns>
        public static Int32 GetAnimationIndex(SpriteAnimationID id)
        {
            return id.animationIndex;
        }

        /// <summary>
        /// Gets the name of the specified animation within its sprite's animation list.
        /// </summary>
        /// <param name="id">The identifier of the animation to evaluate.</param>
        /// <returns>The name of the specified animation within its sprite's animation list.</returns>
        public static Int32 GetAnimationIndexRef(ref SpriteAnimationID id)
        {
            return id.animationIndex;
        }

        /// <summary>
        /// Retrieves the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + spriteAssetID.GetHashCode();
                hash = hash * 23 + (animationName == null ? 0 : animationName.GetHashCode());
                hash = hash * 23 + animationIndex.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("{0}:{1}", 
                spriteAssetID, String.IsNullOrEmpty(animationName) ? animationIndex.ToString() : animationName);
        }

        /// <summary>
        /// Determines whether this object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns><c>true</c> if this object is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return obj is SpriteAnimationID && Equals((SpriteAnimationID)obj);
        }

        /// <summary>
        /// Determines whether this object is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this object.</param>
        /// <returns><c>true</c> if this object is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(SpriteAnimationID other)
        {
            return
                this.spriteAssetID  == other.spriteAssetID &&
                this.animationName  == other.animationName &&
                this.animationIndex == other.animationIndex;
        }

        /// <summary>
        /// Gets an invalid animation identifier.
        /// </summary>
        public static SpriteAnimationID Invalid
        {
            get { return new SpriteAnimationID(); }
        }

        /// <summary>
        /// Gets a value indicating whether this is a valid sprite animation identifier.
        /// </summary>
        public Boolean IsValid
        {
            get { return spriteAssetID.IsValid && (!String.IsNullOrEmpty(animationName) || animationIndex <= 0); }
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the SpriteAnimationID structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the SpriteAnimationID structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        private static Boolean TryParseInternal(ContentManifestRegistry manifests, String s, out SpriteAnimationID value)
        {
            value = default(SpriteAnimationID);

            var delimiterIndex = s.LastIndexOf(':');
            if (delimiterIndex < 0)
                return false;

            var assetID = AssetID.Invalid;
            if (!AssetID.TryParse(manifests, s.Substring(0, delimiterIndex), out assetID))
                return false;

            var animation = s.Substring(delimiterIndex + 1);
            if (String.IsNullOrEmpty(animation))
                return false;

            var animationIndex = 0;
            var animationIndexIsValid = Int32.TryParse(animation, out animationIndex);

            value = animationIndexIsValid ?
                new SpriteAnimationID(assetID, animationIndex) :
                new SpriteAnimationID(assetID, animation);

            return true;
        }

        // Property values.
        private readonly AssetID spriteAssetID;
        private readonly String animationName;
        private readonly Int32 animationIndex;
    }
}
