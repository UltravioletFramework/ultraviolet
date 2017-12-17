using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Texture3D ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Texture3D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var mdat = metadata.As<OpenGLTexture3DProcessorMetadata>();

            using (var surface = manager.Load<Surface3D>(metadata.AssetPath, false, metadata.IsLoadedFromSolution))
            {
                return surface.CreateTexture(mdat.PremultiplyAlpha, manager.Ultraviolet.GetGraphics().Capabilities.FlippedTextures);
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            // TODO
            get { return false; }
        }
    }
}
