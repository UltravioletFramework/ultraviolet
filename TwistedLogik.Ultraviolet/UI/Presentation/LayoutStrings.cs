using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.UI.Presentation.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource StylesheetSyntaxError                = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_ERROR");
        public static readonly StringResource StylesheetSyntaxUnterminatedString   = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNTERMINATED_STRING");
        public static readonly StringResource StylesheetSyntaxUnterminatedSequence = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNTERMINATED_SEQUENCE");
        public static readonly StringResource IsNotDependencyObject                = new StringResource(StringDatabase, "IS_NOT_DEPENDENCY_OBJECT");
        public static readonly StringResource DependencyPropertyDoesNotExist       = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_DOES_NOT_EXIST");
        public static readonly StringResource DependencyPropertyAlreadyRegistered  = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_ALREADY_REGISTERED");
        public static readonly StringResource InvalidBindingExpression             = new StringResource(StringDatabase, "INVALID_BINDING_EXPRESSION");
        public static readonly StringResource BindingAssignmentToValueType         = new StringResource(StringDatabase, "BINDING_ASSIGNMENT_TO_VALUE_TYPE");
        public static readonly StringResource BindingIsReadOnly                    = new StringResource(StringDatabase, "BINDING_IS_READ_ONLY");
        public static readonly StringResource BindingIsWriteOnly                   = new StringResource(StringDatabase, "BINDING_IS_WRITE_ONLY");
        public static readonly StringResource IncompatibleViewModel                = new StringResource(StringDatabase, "INCOMPATIBLE_VIEW_MODEL");
        public static readonly StringResource UIElementInvalidCtor                 = new StringResource(StringDatabase, "UIELEMENT_INVALID_CTOR");
        public static readonly StringResource ViewModelTypeNotFound                = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_NOT_FOUND");
        public static readonly StringResource UnrecognizedUIElement                = new StringResource(StringDatabase, "UNRECOGNIZED_UIELEMENT");
        public static readonly StringResource InvalidDefaultProperty               = new StringResource(StringDatabase, "INVALID_DEFAULT_PROPERTY");
        public static readonly StringResource NoViewModel                          = new StringResource(StringDatabase, "NO_VIEW_MODEL");
        public static readonly StringResource ElementWithIDAlreadyExists           = new StringResource(StringDatabase, "ELEMENT_WITH_ID_ALREADY_EXISTS");
        public static readonly StringResource AmbiguousDependencyProperty          = new StringResource(StringDatabase, "AMBIGUOUS_DEPENDENCY_PROPERTY");
#pragma warning restore 1591
    }
}
