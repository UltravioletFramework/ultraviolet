using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a value which identifies an asset within one of the application's content manifests.
    /// </summary>
    public struct AssetID : IEquatable<AssetID>
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
            Contract.RequireNotEmpty(manifestName, "manifest");
            Contract.RequireNotEmpty(manifestGroup, "manifestGroup");
            Contract.RequireNotEmpty(assetName, "assetName");
            Contract.EnsureRange(assetIndex >= 0, "assetIndex");

            this.manifestName = manifestName;
            this.manifestGroup = manifestGroup;
            this.assetName = assetName;
            this.assetPath = assetPath;
            this.assetIndex = assetIndex;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified asset identifiers are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="AssetID"/> to compare.</param>
        /// <param name="id2">The second <see cref="AssetID"/> to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(AssetID id1, AssetID id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified asset identifiers are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="AssetID"/> to compare.</param>
        /// <param name="id2">The second <see cref="AssetID"/> to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(AssetID id1, AssetID id2)
        {
            return !id1.Equals(id2);
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</returns>
        public static AssetID Parse(String s)
        {
            Contract.Require(s, "s");

            AssetID value;
            if (!TryParseInternal(UltravioletContext.DemandCurrent().GetContent().Manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out AssetID value)
        {
            Contract.Require(s, "s");

            return TryParseInternal(UltravioletContext.DemandCurrent().GetContent().Manifests, s, out value);
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</returns>
        public static AssetID Parse(ContentManifestRegistry manifests, String s)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            AssetID value;
            if (!TryParseInternal(manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(ContentManifestRegistry manifests, String s, out AssetID value)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            return TryParseInternal(manifests, s, out value);
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
        /// Retrieves the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + manifestName.GetHashCode();
                hash = hash * 23 + manifestGroup.GetHashCode();
                hash = hash * 23 + assetName.GetHashCode();
                hash = hash * 23 + assetPath.GetHashCode();
                hash = hash * 23 + assetIndex.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return IsValid ? String.Format("#{0}:{1}:{2}", manifestName, manifestGroup, assetName) : "#INVALID";
        }

        /// <summary>
        /// Determines whether this object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns><c>true</c> if this object is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return obj is AssetID && Equals((AssetID)obj);
        }

        /// <summary>
        /// Determines whether this object is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this object.</param>
        /// <returns><c>true</c> if this object is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(AssetID other)
        {
            return
                this.manifestName  == other.manifestName &&
                this.manifestGroup == other.manifestGroup &&
                this.assetName     == other.assetName &&
                this.assetPath     == other.assetPath &&
                this.assetIndex    == other.assetIndex;
        }

        /// <summary>
        /// Gets an invalid asset identifier.
        /// </summary>
        public static AssetID Invalid
        {
            get { return new AssetID(); }
        }

        /// <summary>
        /// Gets a value indicating whether this is a valid asset identifier.
        /// </summary>
        public Boolean IsValid
        {
            get { return assetPath != null; }
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        private static Boolean TryParseInternal(ContentManifestRegistry manifests, String s, out AssetID value)
        {
            value = default(AssetID);

            if (!s.StartsWith("#"))
                return false;

            if (s == "#INVALID")
            {
                value = AssetID.Invalid;
                return true;
            }

            var components = s.Substring(1).Split(':');
            if (components.Length != 3)
                return false;

            var manifest = manifests[components[0]];
            if (manifest == null)
                throw new AssetException(UltravioletStrings.ContentManifestDoesNotExist.Format(components[0]));

            var manifestGroup = manifest[components[1]];
            if (manifestGroup == null)
                throw new AssetException(UltravioletStrings.ContentManifestGroupDoesNotExist.Format(components[0], components[1]));

            var manifestAsset = manifestGroup[components[2]];
            if (manifestAsset == null)
                throw new AssetException(UltravioletStrings.AssetDoesNotExistWithinManifest.Format(components[0], components[1], components[2]));

            var manifestIndex = manifestGroup.IndexOf(manifestAsset);

            value = new AssetID(manifest.Name, manifestGroup.Name, manifestAsset.Name, manifestAsset.AbsolutePath, manifestIndex);
            return true;
        }

        // Property values.
        private readonly String manifestName;
        private readonly String manifestGroup;
        private readonly String assetName;
        private readonly String assetPath;
        private readonly Int32 assetIndex;
    }
}
