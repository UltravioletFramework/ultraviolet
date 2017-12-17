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
        public static readonly StringResource SurfaceIsNotComplete                = new StringResource(StringDatabase, "SURFACE_IS_NOT_COMPLETE");
        public static readonly StringResource SurfaceMustHaveSquareLayers         = new StringResource(StringDatabase, "SURFACE_MUST_HAVE_SQUARE_LAYERS");
#pragma warning restore 1591
    }
}
