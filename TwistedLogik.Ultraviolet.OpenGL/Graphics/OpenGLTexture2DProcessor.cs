using System;
using System.IO;
using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads 2D texture assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLTexture2DProcessor : ContentProcessor<SDL_Surface, Texture2D>
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SDL_Surface input)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();

            using (var surface = new OpenGLSurface2D(manager.Ultraviolet, input))
            {
                surface.PrepareForTextureExport(mdat.PremultiplyAlpha);

                using (var memstream = new MemoryStream())
                {
                    surface.SaveAsPng(memstream);
                    writer.Write((int)memstream.Length);
                    writer.Write(memstream.ToArray());
                }
            }
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        public override Texture2D ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var bytes = reader.ReadBytes(length);

            using (var stream = new MemoryStream(bytes))
            {
                using (var source = SurfaceSource.Create(stream))
                {
                    using (var imgSurface = new SDL_Surface(source.Width, source.Height))
                    {
                        var imgTexture = new OpenGLTexture2D(manager.Ultraviolet, gl.GL_RGBA8, 
                            source.Width, source.Height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, source.Data);

                        return imgTexture;
                    }
                }
            }
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public unsafe override Texture2D Process(ContentManager manager, IContentProcessorMetadata metadata, SDL_Surface input)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();

            using (var surface = new OpenGLSurface2D(manager.Ultraviolet, input))
            {
                return surface.CreateTexture(mdat.PremultiplyAlpha);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
