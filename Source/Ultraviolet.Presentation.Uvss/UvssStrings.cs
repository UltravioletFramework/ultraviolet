using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation.Uvss
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
                "Ultraviolet.Presentation.Uvss.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource LexerInvalidToken                      = new StringResource(StringDatabase, "LEXER_INVALID_TOKEN");
        public static readonly StringResource SyntaxNodeTypeNeedsTypeID              = new StringResource(StringDatabase, "SYNTAX_NODE_NEEDS_TYPE_ID");
        public static readonly StringResource SyntaxNodeTypeHasDuplicateTypeID       = new StringResource(StringDatabase, "SYNTAX_NODE_HAS_DUPLICATE_TYPE_ID");
        public static readonly StringResource SyntaxNodeTypeNeedsDeserializationCtor = new StringResource(StringDatabase, "SYNTAX_NODE_NEEDS_DESERIALIZATION_CTOR");
        public static readonly StringResource UnrecognizedSyntaxNodeType             = new StringResource(StringDatabase, "UNRECOGNIZED_SYNTAX_NODE_TYPE");
        public static readonly StringResource UnrecognizedSyntaxNodeTypeID           = new StringResource(StringDatabase, "UNRECOGNIZED_SYNTAX_NODE_TYPE_ID");
#pragma warning restore 1591
    }
}
