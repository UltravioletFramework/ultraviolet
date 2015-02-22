using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains the Ultraviolet Presentation Foundation's string resources.
    /// </summary>
    public static class PresentationStrings
    {
        /// <summary>
        /// Initializes the <see cref="PresentationStrings"/> type.
        /// </summary>
        static PresentationStrings()
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
        public static readonly StringResource StylesheetSyntaxInvalidStyleArgs     = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_INVALID_STYLE_ARGS");
        public static readonly StringResource StylesheetSyntaxExpectedToken        = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_EXPECTED_TOKEN");
        public static readonly StringResource StylesheetSyntaxUnexpectedToken      = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNEXPECTED_TOKEN");
        public static readonly StringResource StylesheetSyntaxExpectedValue        = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_EXPECTED_VALUE");
        public static readonly StringResource StylesheetSyntaxUnexpectedValue      = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNEXPECTED_VALUE");
        public static readonly StringResource StylesheetSyntaxUnexpectedEOF        = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_UNEXPECTED_EOF");
        public static readonly StringResource StylesheetInvalidCharacter           = new StringResource(StringDatabase, "STYLESHEET_SYNTAX_INVALID_CHARACTER");
        public static readonly StringResource IsNotDependencyObject                = new StringResource(StringDatabase, "IS_NOT_DEPENDENCY_OBJECT");
        public static readonly StringResource PropertyDoesNotExist                 = new StringResource(StringDatabase, "PROPERTY_DOES_NOT_EXIST");
        public static readonly StringResource PropertyHasNoGetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_GETTER");
        public static readonly StringResource PropertyHasNoSetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_SETTER");
        public static readonly StringResource DependencyPropertyDoesNotExist       = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_DOES_NOT_EXIST");
        public static readonly StringResource DependencyPropertyAlreadyRegistered  = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_ALREADY_REGISTERED");
        public static readonly StringResource CannotResolveBindingExpression       = new StringResource(StringDatabase, "CANNOT_RESOLVE_BINDING_EXPRESSION");
        public static readonly StringResource InvalidBindingExpression             = new StringResource(StringDatabase, "INVALID_BINDING_EXPRESSION");
        public static readonly StringResource InvalidBindingContext                = new StringResource(StringDatabase, "INVALID_BINDING_CONTEXT");
        public static readonly StringResource BindingIsReadOnly                    = new StringResource(StringDatabase, "BINDING_IS_READ_ONLY");
        public static readonly StringResource BindingIsWriteOnly                   = new StringResource(StringDatabase, "BINDING_IS_WRITE_ONLY");
        public static readonly StringResource UIElementInvalidCtor                 = new StringResource(StringDatabase, "UIELEMENT_INVALID_CTOR");
        public static readonly StringResource ViewModelTypeNotFound                = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_NOT_FOUND");
        public static readonly StringResource UnrecognizedUIElement                = new StringResource(StringDatabase, "UNRECOGNIZED_UIELEMENT");
        public static readonly StringResource InvalidDefaultProperty               = new StringResource(StringDatabase, "INVALID_DEFAULT_PROPERTY");
        public static readonly StringResource NoViewModel                          = new StringResource(StringDatabase, "NO_VIEW_MODEL");
        public static readonly StringResource ElementWithIDAlreadyExists           = new StringResource(StringDatabase, "ELEMENT_WITH_ID_ALREADY_EXISTS");
        public static readonly StringResource AmbiguousDependencyProperty          = new StringResource(StringDatabase, "AMBIGUOUS_DEPENDENCY_PROPERTY");
        public static readonly StringResource UserControlDoesNotDefineType         = new StringResource(StringDatabase, "USER_CONTROL_DOES_NOT_DEFINE_TYPE");
        public static readonly StringResource InvalidUserControlDefinition         = new StringResource(StringDatabase, "INVALID_USER_CONTROL_DEFINITION");
        public static readonly StringResource InvalidUserControlType               = new StringResource(StringDatabase, "INVALID_USER_CONTROL_TYPE");
        public static readonly StringResource InvalidUIElementType                 = new StringResource(StringDatabase, "INVALID_UI_ELEMENT_TYPE");
        public static readonly StringResource InvalidChildElements                 = new StringResource(StringDatabase, "INVALID_CHILD_ELEMENTS");
        public static readonly StringResource InvalidElementPropertyValue          = new StringResource(StringDatabase, "INVALID_ELEMENT_PROPERTY_VALUE");
        public static readonly StringResource ComponentRootAlreadyLoaded           = new StringResource(StringDatabase, "COMPONENT_ROOT_ALREADY_LOADED");
        public static readonly StringResource CollectionContainsInvalidElements    = new StringResource(StringDatabase, "COLLECTION_CONTAINS_INVALID_ELEMENTS");
        public static readonly StringResource CollectionHasNoAddMethod             = new StringResource(StringDatabase, "COLLECTION_HAS_NO_ADD_METHOD");
        public static readonly StringResource CollectionCannotBeCleared            = new StringResource(StringDatabase, "COLLECTION_CANNOT_BE_CLEARED");
        public static readonly StringResource ContentPresenterIsNotAComponent      = new StringResource(StringDatabase, "CONTENT_PRESENTER_IS_NOT_A_COMPONENT");
        public static readonly StringResource MeasureMustProduceFiniteDesiredSize  = new StringResource(StringDatabase, "MEASURE_MUST_PRODUCE_FINITE_DESIRED_SIZE");
        public static readonly StringResource CollectionIsBoundToItemsSource       = new StringResource(StringDatabase, "COLLECTION_IS_BOUND_TO_ITEMS_SOURCE");
#pragma warning restore 1591
    }
}
