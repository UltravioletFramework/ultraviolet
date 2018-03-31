using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Platform;

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
            var fontMetadata = metadata.As<FreeTypeFontImporterMetadata>();

            var faceRegularAsset = metadata.AssetFilePath;
            var faceBoldAsset = String.IsNullOrEmpty(fontMetadata.BoldFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.BoldFace);
            var faceItalicAsset = String.IsNullOrEmpty(fontMetadata.ItalicFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.ItalicFace);
            var faceBoldItalicAsset = String.IsNullOrEmpty(fontMetadata.BoldItalicFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.BoldItalicFace);

            var faceDataRegular = new Byte[stream.Length];
            stream.Read(faceDataRegular, 0, faceDataRegular.Length);

            var fileSystemService = new FileSystemService();

            var faceBoldData = default(Byte[]);
            if (faceBoldAsset != null)
            {
                using (var faceBoldStream = fileSystemService.OpenRead(faceBoldAsset))
                {
                    faceBoldData = new Byte[faceBoldStream.Length];
                    faceBoldStream.Read(faceBoldData, 0, faceBoldData.Length);
                }
            }

            var faceItalicData = default(Byte[]);
            if (faceItalicAsset != null)
            {
                using (var faceItalicStream = fileSystemService.OpenRead(faceItalicAsset))
                {
                    faceItalicData = new Byte[faceItalicStream.Length];
                    faceItalicStream.Read(faceItalicData, 0, faceItalicData.Length);
                }
            }

            var faceBoldItalicData = default(Byte[]);
            if (faceBoldItalicAsset != null)
            {
                using (var faceBoldItalicStream = fileSystemService.OpenRead(faceBoldItalicAsset))
                {
                    faceBoldItalicData = new Byte[faceBoldItalicStream.Length];
                    faceBoldItalicStream.Read(faceBoldItalicData, 0, faceBoldItalicData.Length);
                }
            }

            return new FreeTypeFontInfo(fontMetadata.SizeInPoints, faceDataRegular, faceBoldData, faceItalicData, faceBoldItalicData);
        }
    }
}
