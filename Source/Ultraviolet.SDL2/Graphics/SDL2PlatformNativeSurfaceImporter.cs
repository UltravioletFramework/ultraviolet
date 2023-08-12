using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Imports .bmp, .png, and .jpg files.
    /// </summary>
    [ContentImporter(".bmp")]
    [ContentImporter(".png")]
    [ContentImporter(".jpg")]
    [ContentImporter(".jpeg")]
    public unsafe sealed class SDL2PlatformNativeSurfaceImporter : ContentImporter<PlatformNativeSurface>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".bmp", ".png", ".jpg", ".jpeg" };

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2PlatformNativeSurfaceImporter"/> class.
        /// </summary>
        public SDL2PlatformNativeSurfaceImporter() { }

        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The stream that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override PlatformNativeSurface Import(IContentImporterMetadata metadata, Stream stream)
        {
            var data = new Byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var mstream = new MemoryStream(data))
            using (var source = SurfaceSource.Create(mstream))
            {
                return new SDL2PlatformNativeSurface(source);
            }
        }
    }
}
