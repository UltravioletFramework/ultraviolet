using System;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Core;

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

            library.InitializeResource();

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

            content.Importers.RegisterImporter<FreeTypeFontImporter>(FreeTypeFontImporter.SupportedExtensions);

            content.Processors.RegisterProcessor<FreeTypeFontProcessor>();
            content.Processors.RegisterProcessor<UltravioletFontProcessorFromFreeType>();
            content.Processors.RegisterProcessor<UltravioletFontProcessorFromJObject>();
            content.Processors.RegisterProcessor<UltravioletFontProcessorFromXDocument>();

            factory.SetFactoryMethod<TextShaperFactory>((uvctx, capacity) => new HarfBuzzTextShaper(uvctx, capacity));
        }

        /// <summary>
        /// Gets the pointer to the FreeType2 library handle.
        /// </summary>
        internal static IntPtr Library => library.Value.Native;

        // The native FreeType2 library object.
        private static readonly UltravioletSingleton<FreeTypeLibrary> library = 
            new UltravioletSingleton<FreeTypeLibrary>(uv => new FreeTypeLibrary(uv));
    }
}
