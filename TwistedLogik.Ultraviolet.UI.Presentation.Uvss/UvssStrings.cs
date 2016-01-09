using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Contains the Ultraviolet Presentation Foundation's string resources.
    /// </summary>
    public static class UvssStrings
    {
        /// <summary>
        /// Initializes the <see cref="UvssStrings"/> type.
        /// </summary>
        static UvssStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(
                "TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource LexerInvalidToken  = new StringResource(StringDatabase, "LEXER_INVALID_TOKEN");
#pragma warning restore 1591
    }
}
