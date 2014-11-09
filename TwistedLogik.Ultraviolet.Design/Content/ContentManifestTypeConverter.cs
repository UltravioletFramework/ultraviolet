using System;
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

        /// <inheritdoc/>
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return values;
        }

        /// <summary>
        /// Gets a <see cref="System.ComponentModel.TypeConverter.StandardValuesCollection"/> containing the asset identifiers defined by the specified content manifest group.
        /// </summary>
        /// <param name="manifestName">The name of the manifest from which to retrieve values.</param>
        /// <param name="manifestGroupName">The name of the manifest group from which to retrieve values.</param>
        /// <returns>The <see cref="System.ComponentModel.TypeConverter.StandardValuesCollection"/> which was created.</returns>
        private static StandardValuesCollection GetStandardValuesForManifestGroup(String manifestName, String manifestGroupName)
        {
            var uv            = UltravioletContext.DemandCurrent();
            var manifestGroup = uv.GetContent().Manifests[manifestName][manifestGroupName];

            var ids = (from asset in manifestGroup select asset.CreateAssetID()).ToList();

            ids.Insert(0, AssetID.Invalid);

            var values = new StandardValuesCollection(ids.ToList());
            return values;
        }

        // The standard values for this converter.
        private StandardValuesCollection values;
    }
}
