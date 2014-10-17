using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.SDL2
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
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.SDL2.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource BufferIsTooSmall                = new StringResource(StringDatabase, "BUFFER_IS_TOO_SMALL");
        public static readonly StringResource SurfaceAlreadyPreparedForExport = new StringResource(StringDatabase, "SURFACE_ALREADY_PREPARED_FOR_EXPORT");
#pragma warning restore 1591
    }
}
