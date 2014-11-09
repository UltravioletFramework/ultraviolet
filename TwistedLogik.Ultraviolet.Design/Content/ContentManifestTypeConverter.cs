using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Design.Content
{
    /// <summary>
    /// Represents a type converter which produces a list of asset identifier values corresponding
    /// to the contents of a particular content manifest group.
    /// </summary>
    public abstract class ContentManifestTypeConverter : ObjectResolverTypeConverter<AssetID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifestTypeConverter"/> class.
        /// </summary>
        /// <param name="manifestName">The name of the manifest from which to retrieve values.</param>
        /// <param name="manifestGroupName">The name of the manifest group from which to retrieve values.</param>
        public ContentManifestTypeConverter(String manifestName, String manifestGroupName)
        {
            Contract.RequireNotEmpty(manifestName, "manifestName");
            Contract.RequireNotEmpty(manifestGroupName, "manifestGroupName");

            values = GetStandardValuesForManifestGroup(manifestName, manifestGroupName);
        }

        /// <summary>
        /// Gets a value indicating whether this type converter supports standard values.
        /// </summary>
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether only standard values are allowed.
        /// </summary>
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets the type converter's collection of standard values.
        /// </summary>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return values;
        }

        /// <summary>
        /// Gets a <see cref="StandardValuesCollection"/> containing the asset identifiers defined by the specified content manifest group.
        /// </summary>
        /// <param name="manifestName">The name of the manifest from which to retrieve values.</param>
        /// <param name="manifestGroupName">The name of the manifest group from which to retrieve values.</param>
        /// <returns>The <see cref="StandardValuesCollection"/> which was created.</returns>
        private static StandardValuesCollection GetStandardValuesForManifestGroup(String manifestName, String manifestGroupName)
        {
            var uv            = UltravioletContext.DemandCurrent();
            var manifestGroup = uv.GetContent().Manifests[manifestName][manifestGroupName];

            StandardValuesCollection values;
            if (!StandardValuesCache.TryGetValue(manifestGroup, out values))
            {
                var ids = from asset in manifestGroup select asset.CreateAssetID();

                values = new StandardValuesCollection(ids.ToList());
                StandardValuesCache[manifestGroup] = values;
            }
            return values;
        }

        // The cache of standard values associated with each manifest group.
        private static readonly Dictionary<ContentManifestGroup, StandardValuesCollection> StandardValuesCache =
            new Dictionary<ContentManifestGroup, StandardValuesCollection>();

        // The standard values for this converter.
        private StandardValuesCollection values;
    }
}
