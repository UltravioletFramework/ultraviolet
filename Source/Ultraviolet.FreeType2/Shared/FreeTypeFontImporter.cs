using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Imports font files.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentImporter(".ttf")]
    [ContentImporter(".otf")]
    public sealed class FreeTypeFontImporter : ContentImporter<FreeTypeFontInfo>
    {
        /// <inheritdoc/>
        public override FreeTypeFontInfo Import(IContentImporterMetadata metadata, Stream stream)
        {
            var fontPath = metadata.AssetFilePath;
            var fontType = default(FreeTypeFontType);

            switch (Path.GetExtension(fontPath))
            {
                case ".ttf":
                    fontType = FreeTypeFontType.TrueType;
                    break;

                case ".otf":
                    fontType = FreeTypeFontType.OpenType;
                    break;

                default:
                    throw new NotSupportedException();
            }

            return new FreeTypeFontInfo(fontPath, fontType);
        }
    }
}
