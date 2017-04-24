using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Graphics;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Loads a cursor from an image.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class OpenGLCursorProcessor : ContentProcessor<SDL_Surface, Cursor>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Cursor Process(ContentManager manager, IContentProcessorMetadata metadata, SDL_Surface input)
        {
            using (var surface = new OpenGLSurface2D(manager.Ultraviolet, input.CreateCopy()))
            {
                return new OpenGLCursor(manager.Ultraviolet, surface, 0, 0);
            }
        }
    }
}
