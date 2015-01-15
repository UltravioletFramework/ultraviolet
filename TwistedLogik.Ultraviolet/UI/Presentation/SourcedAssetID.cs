using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an asset identifier which is flagged as being either globally- or locally-sourced.
    /// </summary>
    public struct SourcedAssetID : IEquatable<SourcedAssetID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedAssetID"/> class.
        /// </summary>
        /// <param name="assetID">The asset's identifier.</param>
        /// <param name="assetSource">The asset's source.</param>
        public SourcedAssetID(AssetID assetID, AssetSource assetSource)
        {
            this.assetID     = assetID;
            this.assetSource = assetSource;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified asset identifiers are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedAssetID"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedAssetID"/> to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(SourcedAssetID id1, SourcedAssetID id2)
        {
            return id1.assetID.Equals(id2.assetID) && id1.assetSource == id2.assetSource;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified asset identifiers are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedAssetID"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedAssetID"/> to compare.</param>
        /// <returns><c>true</c> if the specified identifiers are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(SourcedAssetID id1, SourcedAssetID id2)
        {
            return !id1.assetID.Equals(id2.assetID) || id1.assetSource != id2.assetSource;
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</returns>
        public static SourcedAssetID Parse(String s)
        {
            Contract.Require(s, "s");

            SourcedAssetID value;
            if (!TryParseInternal(null, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="SourcedAssetID"/> structure
        /// using the content manifest registry provided by the current Ultraviolet context.
        /// </summary>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedAssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out SourcedAssetID value)
        {
            Contract.Require(s, "s");

            return TryParseInternal(null, s, out value);
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="SourcedAssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <returns>An instance of the <see cref="SourcedAssetID"/> structure that is equivalent to the specified string.</returns>
        public static SourcedAssetID Parse(ContentManifestRegistry manifests, String s)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            SourcedAssetID value;
            if (!TryParseInternal(manifests, s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="SourcedAssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedAssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(ContentManifestRegistry manifests, String s, out SourcedAssetID value)
        {
            Contract.Require(manifests, "manifests");
            Contract.Require(s, "s");

            return TryParseInternal(manifests, s, out value);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + assetID.GetHashCode();
                hash = hash * 23 + assetSource.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0} {1}", AssetID, AssetSource.ToString().ToLowerInvariant());
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            return obj is SourcedAssetID && Equals((SourcedAssetID)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(SourcedAssetID other)
        {
            return
                this.assetID.Equals(other.assetID) &&
                this.assetSource == other.assetSource;
        }

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

        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="SourcedAssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="SourcedAssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.</returns>
        private static Boolean TryParseInternal(ContentManifestRegistry manifests, String s, out SourcedAssetID value)
        {
            var parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2)
                throw new FormatException();

            // Parse the asset identifier
            var assetID       = default(AssetID);
            var assetIDParsed = false;

            if (manifests == null)
            {
                assetIDParsed = AssetID.TryParse(parts[0], out assetID);
            }
            else
            {
                assetIDParsed = AssetID.TryParse(manifests, parts[0], out assetID);
            }

            if (!assetIDParsed)
            {
                value = default(SourcedAssetID);
                return false;
            }
         
            // Parse the asset source
            AssetSource assetSource = AssetSource.Global;
            if (parts.Length == 2)
            {
                if (!Enum.TryParse(parts[1], true, out assetSource))
                {
                    value = default(SourcedAssetID);
                    return false;
                }
            }

            value = new SourcedAssetID(assetID, assetSource);
            return true;
        }

        // Property values.
        private readonly AssetID assetID;
        private readonly AssetSource assetSource;
    }
}
