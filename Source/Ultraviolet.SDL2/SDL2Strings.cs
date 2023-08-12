using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class SDL2Strings
    {
        /// <summary>
        /// Initializes the SDL2Strings type.
        /// </summary>
        static SDL2Strings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.SDL2.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource CannotCreateHeadlessContextOnMobile = new StringResource(StringDatabase, "CANNOT_CREATE_HEADLESS_CONTEXT_ON_MOBILE");
        public static readonly StringResource BufferIsTooSmall                    = new StringResource(StringDatabase, "BUFFER_IS_TOO_SMALL");
        public static readonly StringResource SurfaceAlreadyPreparedForExport     = new StringResource(StringDatabase, "SURFACE_ALREADY_PREPARED_FOR_EXPORT");
        public static readonly StringResource SurfaceMustHaveSquareLayers         = new StringResource(StringDatabase, "SURFACE_MUST_HAVE_SQUARE_LAYERS");
        public static readonly StringResource MissingGraphicsAssembly             = new StringResource(StringDatabase, "MISSING_GRAPHICS_ASSEMBLY");
        public static readonly StringResource InvalidGraphicsAssembly             = new StringResource(StringDatabase, "INVALID_GRAPHICS_ASSEMBLY");
        public static readonly StringResource MissingAudioAssembly                = new StringResource(StringDatabase, "MISSING_AUDIO_ASSEMBLY");
        public static readonly StringResource InvalidAudioAssembly                = new StringResource(StringDatabase, "INVALID_AUDIO_ASSEMBLY");
        public static readonly StringResource MissingGraphicsFactory              = new StringResource(StringDatabase, "MISSING_GRAPHICS_FACTORY");
        public static readonly StringResource MissingAudioFactory                 = new StringResource(StringDatabase, "MISSING_AUDIO_FACTORY");
#pragma warning restore 1591
    }
}
