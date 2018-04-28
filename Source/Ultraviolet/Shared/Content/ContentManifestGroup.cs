using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a group of related assets within a content manifest.
    /// </summary>
    public sealed class ContentManifestGroup : UltravioletNamedCollection<ContentManifestAsset>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifestGroup"/> class.
        /// </summary>
        /// <param name="manifest">The content manifest that owns the group.</param>
        /// <param name="element">The XML element that defines the content manifest group.</param>
        internal ContentManifestGroup(ContentManifest manifest, XElement element)
        {
            Contract.Require(manifest, nameof(manifest));
            Contract.Require(element, nameof(element));

            var name = element.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupName);

            var directory = element.AttributeValueString("Directory");

            var typeName = element.AttributeValueString("Type");
            if (String.IsNullOrEmpty(typeName))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupType.Format(name));

            var type = FindTypeByName(typeName);
            
            this.Manifest = manifest;
            this.Name = name;
            this.Type = type ?? throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupType.Format(name));
            this.Directory = directory;

            var assets = element.Elements("Asset");
            foreach (var asset in assets)
            {
                AddInternal(new ContentManifestAsset(this, asset));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifestGroup"/> class.
        /// </summary>
        /// <param name="manifest">The content manifest that owns the group.</param>
        /// <param name="desc">The manifest group description.</param>
        internal ContentManifestGroup(ContentManifest manifest, ContentManifestGroupDescription desc)
        {
            Contract.Require(manifest, nameof(manifest));
            Contract.Require(desc, nameof(desc));

            if (String.IsNullOrEmpty(desc.Name))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupName);

            if (String.IsNullOrEmpty(desc.Type))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupType.Format(desc.Name));

            var type = Type.GetType(desc.Type, false);
            if (type == null)
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestGroupType.Format(desc.Name));

            this.Manifest = manifest;
            this.Name = desc.Name;
            this.Type = type;
            this.Directory = desc.Directory;

            if (desc.Assets != null)
            {
                foreach (var asset in desc.Assets)
                {
                    AddInternal(new ContentManifestAsset(this, asset));
                }
            }
        }

        /// <summary>
        /// Populates the specified asset library type with the manifest group's asset identifiers.
        /// </summary>
        /// <param name="type">The type to populate with asset identifiers.</param>
        /// <remarks>This method will populate the values of any publicly-accessible static fields
        /// and properties of type <see cref="AssetID"/> which match the names of assets within this manifest group.</remarks>
        public void PopulateAssetLibrary(Type type)
        {
            Contract.Require(type, nameof(type));

            // Populate the library's fields.
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(AssetID))
                {
                    var asset = this[field.Name];
                    if (asset != null)
                    {
                        field.SetValue(null, asset.CreateAssetID());
                    }
                }
            }

            // Populate the library's properties.
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(AssetID))
                {
                    var asset = this[property.Name];
                    if (asset != null)
                    {
                        property.SetValue(null, asset.CreateAssetID(), null);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the index of the specified asset within the manifest group.
        /// </summary>
        /// <param name="asset">The asset for which to retrieve an index.</param>
        /// <returns>The index of the specified asset within the manifest group, or -1 if 
        /// the asset does not exist in the manifest group.</returns>
        public Int32 IndexOf(ContentManifestAsset asset)
        {
            Contract.Require(asset, nameof(asset));

            return Storage.IndexOf(asset);
        }

        /// <summary>
        /// Gets the <see cref="ContentManifest"/> that owns this group.
        /// </summary>
        public ContentManifest Manifest
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content group's name.
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the root directory of assets within the content group.
        /// </summary>
        public String Directory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of assets within the content group.
        /// </summary>
        public Type Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected override String GetName(ContentManifestAsset item)
        {
            return item.Name;
        }

        /// <summary>
        /// Attempts to find the type with the specified name.
        /// </summary>
        private static Type FindTypeByName(String name)
        {
            if (name.Contains("{"))
            {
                var nameType = name.Trim();
                var nameAsm = String.Empty;

                var ixComma = name.LastIndexOf(',');
                if (ixComma >= 0)
                {
                    nameType = name.Substring(0, ixComma).Trim();
                    nameAsm = name.Substring(ixComma + 1).Trim();
                }

                var nameMatch = regexTypeName.Match(nameType);
                if (nameMatch.Success)
                {
                    var asm = String.IsNullOrEmpty(nameAsm) ? default(Assembly) : Assembly.Load(nameAsm);

                    var genericTypeParamNames = nameMatch.Groups["typeparam"].Value.Split(',').Select(x => x.Trim());
                    var genericTypeParams = genericTypeParamNames.Select(x => asm?.GetType(x, false) ?? Type.GetType(x, false)).ToArray();
                    if (genericTypeParams.Any(x => x == null))
                        return null;

                    var genericTypeName = $"{nameMatch.Groups["typename"].Value}`{genericTypeParams.Length}";
                    var genericType = asm?.GetType(genericTypeName, false) ?? Type.GetType(genericTypeName, false);
                    if (genericType == null)
                        return null;

                    return genericType.MakeGenericType(genericTypeParams);
                }

                return null;
            }

            return Type.GetType(name, false);
        }

        // Regex for matching generic types.
        private static readonly Regex regexTypeName = new Regex(@"^(?<typename>[^{}]+){(?<typeparam>.+)}$", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
