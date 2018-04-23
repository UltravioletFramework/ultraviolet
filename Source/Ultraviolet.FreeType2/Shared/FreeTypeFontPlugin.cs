using System;
using Ultraviolet.Core;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Contains methods for managing the lifetime of the FreeType2 Font Plugin for Ultraviolet.
    /// </summary>
    public unsafe class FreeTypeFontPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            Contract.Require(uv, nameof(uv));

            if (Library != IntPtr.Zero)
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
        internal static IntPtr Library { get; private set; }

        /// <summary>
        /// Initializes the FreeType2 API.
        /// </summary>
        private static void FT_Init(UltravioletContext uv)
        {
            var lib = default(IntPtr);
            var err = FT_Init_FreeType((IntPtr)(&lib));
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

            Library = IntPtr.Zero;
        }
    }
}
