using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads 3D texture assets.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLTexture3DProcessor : ContentProcessor<PlatformNativeSurface, Texture3D>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, PlatformNativeSurface input, Boolean delete)
        {
            var mdat = metadata.As<OpenGLTexture3DProcessorMetadata>();
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;
            var srgbEncoded = (mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForTexture3D) && caps.SrgbEncodingEnabled;

            using (var surface = manager.Process<PlatformNativeSurface, Surface3D>(input, metadata.AssetDensity))
            {
                var flipdir = caps.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None;
                for (int i = 0; i < surface.Depth; i++)
                {
                    var layer = surface.GetLayer(i);
                    layer.SrgbEncoded = srgbEncoded;
                    layer.FlipAndProcessAlpha(flipdir, mdat.PremultiplyAlpha, mdat.Opaque ? null : (Color?)Color.Magenta);
                }

                writer.Write(Int32.MaxValue);
                writer.Write(1u);
                writer.Write(surface.Depth);
                writer.Write(srgbEncoded);

                for (int i = 0; i < surface.Depth; i++)
                {
                    using (var memstream = new MemoryStream())
                    {
                        var layer = surface.GetLayer(i);
                        layer.SaveAsPng(memstream);
                        writer.Write((int)memstream.Length);
                        writer.Write(memstream.ToArray());
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override Texture3D ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;

            var version = 0u;
            var depth = reader.ReadInt32();
            if (depth == Int32.MaxValue)
                version = reader.ReadUInt32();

            if (version > 0u)
                depth = reader.ReadInt32();

            var srgbEncoded = false;
            if (version > 0u)
                srgbEncoded = reader.ReadBoolean() && caps.SrgbEncodingEnabled;

            var layerSurfaces = new List<SurfaceSource>();
            var layerPointers = new List<IntPtr>();
            try
            {
                for (int i = 0; i < depth; i++)
                {
                    var length = reader.ReadInt32();
                    var bytes = reader.ReadBytes(length);

                    using (var stream = new MemoryStream(bytes))
                    {
                        var surfaceSource = SurfaceSource.Create(stream);
                        layerSurfaces.Add(surfaceSource);
                        layerPointers.Add(surfaceSource.Data);
                    }
                }

                var layerWidth = layerSurfaces[0].Width;
                var layerHeight = layerSurfaces[0].Height;

                var internalformat = OpenGLTextureUtil.GetInternalFormatFromBytesPerPixel(4, srgbEncoded);
                var format = (layerSurfaces[0].DataFormat == SurfaceSourceDataFormat.RGBA) ? gl.GL_RGBA : gl.GL_BGRA;

                return new OpenGLTexture3D(manager.Ultraviolet, internalformat, layerWidth, layerHeight, format, 
                    gl.GL_UNSIGNED_BYTE, layerPointers, true);
            }
            finally
            {
                foreach (var layerSurface in layerSurfaces)
                    layerSurface.Dispose();
            }
        }

        /// <inheritdoc/>
        public override Texture3D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var mdat = metadata.As<OpenGLTexture3DProcessorMetadata>();
            var caps = manager.Ultraviolet.GetGraphics().Capabilities;
            var srgbEncoded = (mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForTexture3D) && caps.SrgbEncodingEnabled;

            using (var surface = manager.Load<Surface3D>(metadata.AssetPath, false, metadata.IsLoadedFromSolution))
            {
                var flipdir = manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures ? SurfaceFlipDirection.Vertical : SurfaceFlipDirection.None;
                for (int i = 0; i < surface.Depth; i++)
                {
                    var layer = surface.GetLayer(i);
                    layer.SrgbEncoded = srgbEncoded;
                    layer.FlipAndProcessAlpha(flipdir, mdat.PremultiplyAlpha, mdat.Opaque ? null : (Color?)Color.Magenta);
                }
                return surface.CreateTexture(unprocessed: true);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing { get; } = true;
    }
}
