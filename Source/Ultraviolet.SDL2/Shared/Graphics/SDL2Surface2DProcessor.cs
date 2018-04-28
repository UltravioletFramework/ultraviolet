using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Loads 2D surface assets.
    /// </summary>
    [ContentProcessor]
    public sealed class SDL2Surface2DProcessor : ContentProcessor<PlatformNativeSurface, Surface2D>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Surface2D Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            var mdat = metadata.As<SDL2Surface2DProcessorMetadata>();
            var srgbEncoded = mdat.SrgbEncoded ?? manager.Ultraviolet.Properties.SrgbDefaultForSurface2D;
            var surfOptions = srgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;

            var copy = input.CreateCopy();
            var result = new SDL2Surface2D(manager.Ultraviolet, copy, surfOptions);

            return result;
        }
    }
}
