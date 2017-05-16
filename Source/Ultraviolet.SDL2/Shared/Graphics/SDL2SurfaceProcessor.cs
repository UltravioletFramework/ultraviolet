using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Loads 2D surface assets.
    /// </summary>
    [Preserve(AllMembers = true)]
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
            var copy = input.CreateCopy();
            return new SDL2Surface2D(manager.Ultraviolet, copy);
        }
    }
}
