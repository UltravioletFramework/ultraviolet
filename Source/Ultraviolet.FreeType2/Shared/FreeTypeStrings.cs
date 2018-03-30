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
#pragma warning restore 1591
    }
}
