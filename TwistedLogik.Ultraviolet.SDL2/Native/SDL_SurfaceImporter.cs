using System.IO;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Imports .bmp, .png, and .jpg files.
    /// </summary>
    [ContentImporter(".bmp")]
    [ContentImporter(".png")]
    [ContentImporter(".jpg")]
    [ContentImporter(".jpeg")]
    public unsafe sealed class SDL_SurfaceImporter : ContentImporter<SDL_Surface>
    {
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The stream that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override SDL_Surface Import(IContentImporterMetadata metadata, Stream stream)
        {
            return SDL_Surface.CreateFromStream(stream);
        }
    }
}
