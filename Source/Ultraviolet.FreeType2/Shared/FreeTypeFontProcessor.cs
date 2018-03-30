using System;
using Ultraviolet.Content;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Loads FreeType fonts.
    /// </summary>
    [ContentProcessor]
    public class FreeTypeFontProcessor : ContentProcessor<FreeTypeFontInfo, FreeTypeFont>
    {
        /// <inheritdoc/>
        public override FreeTypeFont Process(ContentManager manager, IContentProcessorMetadata metadata, FreeTypeFontInfo input)
        {
            throw new NotImplementedException();
        }
    }
}
