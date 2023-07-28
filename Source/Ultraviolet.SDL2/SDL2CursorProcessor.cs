using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Graphics;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Loads a cursor from an image.
    /// </summary>
    [ContentProcessor]
    public sealed class SDL2CursorProcessor : ContentProcessor<PlatformNativeSurface, Cursor>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Cursor Process(ContentManager manager, IContentProcessorMetadata metadata, PlatformNativeSurface input)
        {
            using (var surface = new SDL2Surface2D(manager.Ultraviolet, input.CreateCopy(), SurfaceOptions.SrgbColor))
            {
                return new SDL2Cursor(manager.Ultraviolet, surface, 0, 0);
            }
        }
    }
}
