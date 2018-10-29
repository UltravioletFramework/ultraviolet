using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class FreeTypeStrings
    {
        /// <summary>
        /// Initializes the <see cref="FreeTypeStrings"/> type.
        /// </summary>
        static FreeTypeStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.FreeType2.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource PluginAlreadyInitialized              = new StringResource(StringDatabase, "PLUGIN_ALREADY_INITIALIZED");
        public static readonly StringResource AlternativePluginAlreadyInitialized   = new StringResource(StringDatabase, "ALTERNATIVE_PLUGIN_ALREADY_INITIALIZED");
        public static readonly StringResource ContentRedirectionError               = new StringResource(StringDatabase, "CONTENT_REDIRECTION_ERROR");
        public static readonly StringResource GlyphTooBigForAtlas                   = new StringResource(StringDatabase, "GLYPH_TOO_BIG_FOR_ATLAS");
        public static readonly StringResource PixelFormatNotSupported               = new StringResource(StringDatabase, "PIXEL_FORMAT_NOT_SUPPORTED");
        public static readonly StringResource CannotSpecifyPointAndPixelSize        = new StringResource(StringDatabase, "CANNOT_SPECIFY_POINT_AND_PIXEL_SIZE");
        public static readonly StringResource NonScalableFontFaceRequiresPixelSize  = new StringResource(StringDatabase, "NON_SCALABLE_FONT_FACE_REQUIRES_PIXEL_SIZE");
        public static readonly StringResource FontDoesNotHaveBitmapStrikes          = new StringResource(StringDatabase, "FONT_DOES_NOT_HAVE_BITMAP_STRIKES");
        public static readonly StringResource NoMatchingPixelSize                   = new StringResource(StringDatabase, "NO_MATCHING_PIXEL_SIZE");
        public static readonly StringResource InvalidAtlasParameters                = new StringResource(StringDatabase, "INVALID_ATLAS_PARAMETERS");
        public static readonly StringResource TextShaperRequiresFreeTypeFont        = new StringResource(StringDatabase, "TEXT_SHAPER_REQUIRES_FREE_TYPE_FONT");
        public static readonly StringResource InvalidBufferDirection                = new StringResource(StringDatabase, "INVALID_BUFFER_DIRECTION");
        public static readonly StringResource InvalidBufferScript                   = new StringResource(StringDatabase, "INVALID_BUFFER_SCRIPT");
        public static readonly StringResource InvalidBufferLanguage                 = new StringResource(StringDatabase, "INVALID_BUFFER_LANGUAGE");
#pragma warning restore 1591
    }
}
