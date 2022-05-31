using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads 2D texture assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLTexture2DProcessor : ContentProcessor<PlatformNativeSurface, Texture2D>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, PlatformNativeSurface input, Boolean delete)
        {
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;
            var srgbEncoded = mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForTexture2D;
            var surfOptions = srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;

            using (var surface = Surface2D.Create(input, surfOptions))
            {
                var flipdir = caps.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None;
                surface.FlipAndProcessAlpha(flipdir, mdat.PremultiplyAlpha, mdat.Opaque ? null : (Color?)Color.Magenta);

                using (var memstream = new MemoryStream())
                {
                    surface.SaveAsPng(memstream);
                    writer.Write(Int32.MaxValue);
                    writer.Write(1u);
                    writer.Write(surface.SrgbEncoded);
                    writer.Write((int)memstream.Length);
                    writer.Write(memstream.ToArray());
                }
            }
        }

        /// <inheritdoc/>
        public override Texture2D ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;

            var version = 0u;
            var length = reader.ReadInt32();
            if (length == Int32.MaxValue)
                version = reader.ReadUInt32();

            if (version > 0u)
                length = reader.ReadInt32();

            var srgbEncoded = false;
            if (version > 0u)
                srgbEncoded = reader.ReadBoolean() && caps.SrgbEncodingEnabled;

            var bytes = reader.ReadBytes(length);

            using (var stream = new MemoryStream(bytes))
            {
                using (var source = SurfaceSource.Create(stream))
                {
                    var format = (source.DataFormat == SurfaceSourceDataFormat.RGBA) ? gl.GL_RGBA : gl.GL_BGRA;
                    var internalformat = OpenGLTextureUtil.GetInternalFormatFromBytesPerPixel(4, srgbEncoded);

                    return new OpenGLTexture2D(manager.Ultraviolet, internalformat,
                        source.Width, source.Height, format, gl.GL_UNSIGNED_BYTE, source.Data, true);
                }
            }
        }

        /// <inheritdoc/>
        public override Texture2D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;
            var mdat = metadata.As<OpenGLTexture2DProcessorMetadata>();
            var srgbEncoded = mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForTexture2D;
            var surfOptions = srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;

            using (var surface = Surface2D.Create(input, surfOptions))
            {
                var flipdir = manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None;
                surface.FlipAndProcessAlpha(flipdir, mdat.PremultiplyAlpha, mdat.Opaque ? null : (Color?)Color.Magenta);

                return surface.CreateTexture(unprocessed: true);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing { get; } = true;
    }
}
