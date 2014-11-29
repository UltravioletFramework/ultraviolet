using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Contains the layout provider's string resources.
    /// </summary>
    public static class LayoutStrings
    {
        /// <summary>
        /// Initializes the <see cref="LayoutStrings"/> type.
        /// </summary>
        static LayoutStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.Layout.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource StylesheetSyntaxError                = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_ERROR");
        public static readonly StringResource StylesheetSyntaxUnterminatedString   = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNTERMINATED_STRING");
        public static readonly StringResource StylesheetSyntaxUnterminatedSequence = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNTERMINATED_SEQUENCE");
#pragma warning restore 1591
    }
}
