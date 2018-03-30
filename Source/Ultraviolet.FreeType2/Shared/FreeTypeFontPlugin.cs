using System;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Contains methods for managing the lifetime of the FreeType2 Font Plugin for Ultraviolet.
    /// </summary>
    public static class FreeTypeFontPlugin
    {
        /// <summary>
        /// Initializes the FreeType2 Font Plugin for Ultraviolet.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public static void Initialize(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var content = uv.GetContent();
            var existing = content.Importers.FindImporter(".ttf");
            if (existing != null)
            {
                if (existing.GetType() == typeof(FreeTypeFontImporter))
                {
                    throw new InvalidOperationException(FreeTypeStrings.PluginAlreadyInitialized);
                }
                else
                {
                    throw new InvalidOperationException(FreeTypeStrings.AlternativePluginAlreadyInitialized);
                }
            }

            content.RegisterImportersAndProcessors(typeof(FreeTypeFontPlugin).Assembly);
        }
    }
}
