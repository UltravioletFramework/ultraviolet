using System;
using System.IO;
using System.Runtime.InteropServices;
using Ultraviolet.Content;
using Ultraviolet.Platform;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Imports font files.
    /// </summary>
    [ContentImporter(".ttf")]
    [ContentImporter(".ttc")]
    [ContentImporter(".otf")]
    [ContentImporter(".otc")]
    [ContentImporter(".fnt")]
    public sealed class FreeTypeFontImporter : ContentImporter<FreeTypeFontInfo>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".ttf", ".ttc", ".otf", ".otc", ".fnt" };

        /// <inheritdoc/>
        public override FreeTypeFontInfo Import(IContentImporterMetadata metadata, Stream stream)
        {
            var fileSystemService = new FileSystemService();

            var fontMetadata = metadata.As<FreeTypeFontImporterMetadata>();

            var faceBoldAsset = default(String);
            var faceItalicAsset = default(String);
            var faceBoldItalicAsset = default(String);
            if (metadata.IsFile)
            {
                var faceRegularAsset = metadata.AssetFilePath;
                faceBoldAsset = String.IsNullOrEmpty(fontMetadata.BoldFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.BoldFace);
                faceItalicAsset = String.IsNullOrEmpty(fontMetadata.ItalicFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.ItalicFace);
                faceBoldItalicAsset = String.IsNullOrEmpty(fontMetadata.BoldItalicFace) ? null : ResolveDependencyAssetFilePath(metadata, fontMetadata.BoldItalicFace);
            }

            var faceRegularData = ReadStreamIntoNativeMemory(stream, out var faceRegularDataLength);

            var faceBoldDataLength = 0;
            var faceBoldData = IntPtr.Zero;
            if (faceBoldAsset != null)
            {
                using (var faceBoldStream = fileSystemService.OpenRead(faceBoldAsset))
                    faceBoldData = ReadStreamIntoNativeMemory(faceBoldStream, out faceBoldDataLength);
            }

            var faceItalicDataLength = 0;
            var faceItalicData = IntPtr.Zero;
            if (faceItalicAsset != null)
            {
                using (var faceItalicStream = fileSystemService.OpenRead(faceItalicAsset))
                    faceItalicData = ReadStreamIntoNativeMemory(faceItalicStream, out faceItalicDataLength);
            }

            var faceBoldItalicDataLength = 0;
            var faceBoldItalicData = IntPtr.Zero;
            if (faceBoldItalicAsset != null)
            {
                using (var faceBoldItalicStream = fileSystemService.OpenRead(faceBoldItalicAsset))
                    faceBoldItalicData = ReadStreamIntoNativeMemory(faceBoldItalicStream, out faceBoldItalicDataLength);
            }

            return new FreeTypeFontInfo(
                faceRegularData, faceRegularDataLength,
                faceBoldData, faceBoldDataLength,
                faceItalicData, faceItalicDataLength,
                faceBoldItalicData, faceBoldItalicDataLength);
        }

        /// <summary>
        /// Reads the contents of the specified stream into a native memory buffer.
        /// </summary>
        private static IntPtr ReadStreamIntoNativeMemory(Stream stream, out Int32 length)
        {
            var buffer = Marshal.AllocHGlobal((Int32)stream.Length);
            var temp = new Byte[1024];

            length = 0;

            unsafe
            {
                while (stream.Position < stream.Length)
                {
                    var bytes = stream.Read(temp, 0, temp.Length);
                    Marshal.Copy(temp, 0, (IntPtr)((Byte*)buffer + length), bytes);
                    length += bytes;
                }
            }

            return buffer;
        }
    }
}
