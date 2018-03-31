using System;
using Ultraviolet.Core;
using Ultraviolet.FreeType2.Native;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Err;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Contains methods for managing the lifetime of the FreeType2 Font Plugin for Ultraviolet.
    /// </summary>
    public static unsafe class FreeTypeFontPlugin
    {
        /// <summary>
        /// Initializes the FreeType2 Font Plugin for Ultraviolet.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public static void Initialize(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            if (Library != null)
                throw new InvalidOperationException(FreeTypeStrings.PluginAlreadyInitialized);

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

            FT_Init(uv);
            uv.Shutdown += FT_Done;
        }

        /// <summary>
        /// Gets the pointer to the FreeType2 library handle.
        /// </summary>
        internal static FT_LibraryRec_* Library { get; private set; }

        /// <summary>
        /// Initializes the FreeType2 API.
        /// </summary>
        private static void FT_Init(UltravioletContext uv)
        {
            var lib = default(FT_LibraryRec_*);
            var err = FT_Init_FreeType(&lib);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            Library = lib;
        }

        /// <summary>
        /// Cleans up the FreeType2 API.
        /// </summary>
        private static void FT_Done(UltravioletContext uv)
        {
            var err = FT_Done_FreeType(Library);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            Library = null;
        }
    }
}
