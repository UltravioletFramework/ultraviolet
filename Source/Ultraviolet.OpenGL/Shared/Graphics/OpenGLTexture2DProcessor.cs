using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads 2D texture assets.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class OpenGLTexture2DProcessor : ContentProcessor<PlatformNativeSurface, Texture2D>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, PlatformNativeSurface input, Boolean delete)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();

            using (var surface = Surface2D.Create(input))
            {
                surface.PrepareForTextureExport(mdat.PremultiplyAlpha, manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures);

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
                    var imgTexture = new OpenGLTexture2D(manager.Ultraviolet, gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8,
                        source.Width, source.Height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, source.Data, true);

                    return imgTexture;
                }
            }
        }

        /// <inheritdoc/>
        public override Texture2D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();

            using (var surface = Surface2D.Create(input))
            {
                return surface.CreateTexture(mdat.PremultiplyAlpha, manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
