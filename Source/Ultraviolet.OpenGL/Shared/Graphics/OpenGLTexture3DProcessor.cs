using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads 3D texture assets.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class OpenGLTexture3DProcessor : ContentProcessor<PlatformNativeSurface, Texture3D>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, PlatformNativeSurface input, Boolean delete)
        {
            var mdat = metadata.As<OpenGLTexture3DProcessorMetadata>();
            
            using (var surface = manager.Process<PlatformNativeSurface, Surface3D>(input, metadata.AssetDensity))
            {
                surface.PrepareForTextureExport(mdat.PremultiplyAlpha, manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures, mdat.Opaque);

                writer.Write(surface.Depth);
                for (int i = 0; i < surface.Depth; i++)
                {
                    using (var memstream = new MemoryStream())
                    {
                        surface.GetLayer(i).SaveAsPng(memstream);
                        writer.Write((int)memstream.Length);
                        writer.Write(memstream.ToArray());
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override Texture3D ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var depth = reader.ReadInt32();
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

                var imgInternalFormat = gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8;
                var imgFormat = (layerSurfaces[0].DataFormat == SurfaceSourceDataFormat.RGBA) ? gl.GL_RGBA : gl.GL_BGRA;

                return new OpenGLTexture3D(manager.Ultraviolet, imgInternalFormat, layerWidth, layerHeight, imgFormat, 
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

            using (var surface = manager.Load<Surface3D>(metadata.AssetPath, false, metadata.IsLoadedFromSolution))
            {
                return surface.CreateTexture(mdat.PremultiplyAlpha, manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures, mdat.Opaque);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
