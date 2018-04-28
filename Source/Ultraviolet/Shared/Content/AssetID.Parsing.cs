using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    partial struct AssetID
    {        
        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <returns>An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</returns>
        public static AssetID Parse(ContentManifestRegistry manifests, String s)
        {
            Contract.Require(manifests, nameof(manifests));
            Contract.Require(s, nameof(s));
            
            if (!TryParseInternal(manifests, s, out AssetID value))
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
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(ContentManifestRegistry manifests, String s, out AssetID value)
        {
            Contract.Require(manifests, nameof(manifests));
            Contract.Require(s, nameof(s));

            return TryParseInternal(manifests, s, out value);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="AssetID"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out AssetID v)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="AssetID"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static AssetID Parse(String s)
        {
            var v = default(AssetID);
            if (!TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="AssetID"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out AssetID v)
        {
            return TryParseInternal(UltravioletContext.DemandCurrent().GetContent().Manifests, s, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="AssetID"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static AssetID Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(AssetID);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of an asset identifier to an instance of the <see cref="AssetID"/> structure.
        /// </summary>
        /// <param name="manifests">The content manifest registry that contains the currently-loaded content manifests.</param>
        /// <param name="s">A string containing the asset identifier to convert.</param>
        /// <param name="value">An instance of the <see cref="AssetID"/> structure that is equivalent to the specified string.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
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
    }
}
