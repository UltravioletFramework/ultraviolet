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
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SDL_Surface input, Boolean delete)
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public unsafe override Texture2D Process(ContentManager manager, IContentProcessorMetadata metadata, SDL_Surface input)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();

            using (var surface = new OpenGLSurface2D(manager.Ultraviolet, input))
            {
                return surface.CreateTexture(mdat.PremultiplyAlpha);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
