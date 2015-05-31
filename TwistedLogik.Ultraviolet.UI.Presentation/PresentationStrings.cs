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
        public static readonly StringResource StyleSheetSyntaxError                = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_ERROR");
        public static readonly StringResource StyleSheetSyntaxUnterminatedString   = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_UNTERMINATED_STRING");
        public static readonly StringResource StyleSheetSyntaxUnterminatedSequence = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_UNTERMINATED_SEQUENCE");
        public static readonly StringResource StyleSheetSyntaxInvalidStyleArgs     = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_INVALID_STYLE_ARGS");
        public static readonly StringResource StyleSheetSyntaxExpectedToken        = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_EXPECTED_TOKEN");
        public static readonly StringResource StyleSheetSyntaxUnexpectedToken      = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_UNEXPECTED_TOKEN");
        public static readonly StringResource StyleSheetSyntaxExpectedValue        = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_EXPECTED_VALUE");
        public static readonly StringResource StyleSheetSyntaxUnexpectedValue      = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_UNEXPECTED_VALUE");
        public static readonly StringResource StyleSheetSyntaxUnexpectedEOF        = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_UNEXPECTED_EOF");
        public static readonly StringResource StyleSheetInvalidCharacter           = new StringResource(StringDatabase, "STYLE_SHEET_SYNTAX_INVALID_CHARACTER");
        public static readonly StringResource StyleSheetInvalidTriggerType         = new StringResource(StringDatabase, "STYLE_SHEET_INVALID_TRIGGER_TYPE");
        public static readonly StringResource IsNotDependencyObject                = new StringResource(StringDatabase, "IS_NOT_DEPENDENCY_OBJECT");
        public static readonly StringResource PropertyHasNoGetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_GETTER");
        public static readonly StringResource PropertyHasNoSetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_SETTER");
        public static readonly StringResource DependencyPropertyAlreadyRegistered  = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_ALREADY_REGISTERED");
        public static readonly StringResource DependencyPropertyIsReadOnly         = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_IS_READ_ONLY");
        public static readonly StringResource RoutedEventAlreadyRegistered         = new StringResource(StringDatabase, "ROUTED_EVENT_ALREADY_REGISTERED");
        public static readonly StringResource CannotResolveBindingExpression       = new StringResource(StringDatabase, "CANNOT_RESOLVE_BINDING_EXPRESSION");
        public static readonly StringResource InvalidBindingExpression             = new StringResource(StringDatabase, "INVALID_BINDING_EXPRESSION");
        public static readonly StringResource InvalidBindingContext                = new StringResource(StringDatabase, "INVALID_BINDING_CONTEXT");
        public static readonly StringResource BindingIsReadOnly                    = new StringResource(StringDatabase, "BINDING_IS_READ_ONLY");
        public static readonly StringResource BindingIsWriteOnly                   = new StringResource(StringDatabase, "BINDING_IS_WRITE_ONLY");
        public static readonly StringResource UIElementInvalidCtor                 = new StringResource(StringDatabase, "UIELEMENT_INVALID_CTOR");
        public static readonly StringResource ViewModelTypeNotFound                = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_NOT_FOUND");
        public static readonly StringResource KnownTypeAlreadyRegistered           = new StringResource(StringDatabase, "KNOWN_TYPE_ALREADY_REGISTERED");
        public static readonly StringResource UnrecognizedType                     = new StringResource(StringDatabase, "UNRECOGNIZED_TYPE");
        public static readonly StringResource InvalidDefaultProperty               = new StringResource(StringDatabase, "INVALID_DEFAULT_PROPERTY");
        public static readonly StringResource NoViewModel                          = new StringResource(StringDatabase, "NO_VIEW_MODEL");
        public static readonly StringResource ElementWithNameAlreadyExists         = new StringResource(StringDatabase, "ELEMENT_WITH_NAME_ALREADY_EXISTS");
        public static readonly StringResource AmbiguousDependencyProperty          = new StringResource(StringDatabase, "AMBIGUOUS_DEPENDENCY_PROPERTY");
        public static readonly StringResource UserControlDoesNotDefineType         = new StringResource(StringDatabase, "USER_CONTROL_DOES_NOT_DEFINE_TYPE");
        public static readonly StringResource InvalidUserControlDefinition         = new StringResource(StringDatabase, "INVALID_USER_CONTROL_DEFINITION");
        public static readonly StringResource InvalidUserControlType               = new StringResource(StringDatabase, "INVALID_USER_CONTROL_TYPE");
        public static readonly StringResource InvalidChildElements                 = new StringResource(StringDatabase, "INVALID_CHILD_ELEMENTS");
        public static readonly StringResource InvalidElementPropertyValue          = new StringResource(StringDatabase, "INVALID_ELEMENT_PROPERTY_VALUE");
        public static readonly StringResource ComponentRootAlreadyLoaded           = new StringResource(StringDatabase, "COMPONENT_ROOT_ALREADY_LOADED");
        public static readonly StringResource CollectionContainsInvalidElements    = new StringResource(StringDatabase, "COLLECTION_CONTAINS_INVALID_ELEMENTS");
        public static readonly StringResource CollectionHasNoAddMethod             = new StringResource(StringDatabase, "COLLECTION_HAS_NO_ADD_METHOD");
        public static readonly StringResource CollectionCannotBeCleared            = new StringResource(StringDatabase, "COLLECTION_CANNOT_BE_CLEARED");
        public static readonly StringResource ContentPresenterIsNotAComponent      = new StringResource(StringDatabase, "CONTENT_PRESENTER_IS_NOT_A_COMPONENT");
        public static readonly StringResource MeasureMustProduceFiniteDesiredSize  = new StringResource(StringDatabase, "MEASURE_MUST_PRODUCE_FINITE_DESIRED_SIZE");
        public static readonly StringResource CollectionIsBoundToItemsSource       = new StringResource(StringDatabase, "COLLECTION_IS_BOUND_TO_ITEMS_SOURCE");
        public static readonly StringResource ItemPresenterNotInItemsControl       = new StringResource(StringDatabase, "ITEM_PRESENTER_NOT_IN_ITEMS_CONTROL");
        public static readonly StringResource ItemPresenterNotCreatedCorrectly     = new StringResource(StringDatabase, "ITEM_PRESENTER_NOT_CREATED_CORRECTLY");
        public static readonly StringResource InvalidRoutedEventDelegate           = new StringResource(StringDatabase, "INVALID_ROUTED_EVENT_DELEGATE");
        public static readonly StringResource InvalidRoutingStrategy               = new StringResource(StringDatabase, "INVALID_ROUTING_STRATEGY");
        public static readonly StringResource EventOrPropertyDoesNotExist          = new StringResource(StringDatabase, "EVENT_OR_PROPERTY_DOES_NOT_EXIST");
        public static readonly StringResource ClassTypeMustBeSubclassOfOwnerType   = new StringResource(StringDatabase, "CLASS_TYPE_MUST_BE_SUBCLASS_OF_OWNER_TYPE");
        public static readonly StringResource KnownTypeMissingAttribute            = new StringResource(StringDatabase, "KNOWN_TYPE_MISSING_ATTRIBUTE");
        public static readonly StringResource HandlerTypeMismatch                  = new StringResource(StringDatabase, "HANDLER_TYPE_MISMATCH");
        public static readonly StringResource NotVisualObject                      = new StringResource(StringDatabase, "NOT_VISUAL_OBJECT");
        public static readonly StringResource ClipRectangleMustHaveValidDimensions = new StringResource(StringDatabase, "CLIP_RECTANGLE_MUST_HAVE_VALID_DIMENSIONS");
        public static readonly StringResource NotUIElement                         = new StringResource(StringDatabase, "NOT_UIELEMENT");
        public static readonly StringResource NotAnInputElement                    = new StringResource(StringDatabase, "NOT_AN_INPUT_ELEMENT");
        public static readonly StringResource VisualAlreadyHasAParent              = new StringResource(StringDatabase, "VISUAL_ALREADY_HAS_A_PARENT");
        public static readonly StringResource BeginInitAlreadyCalled               = new StringResource(StringDatabase, "BEGIN_INIT_ALREADY_CALLED");
        public static readonly StringResource BeginInitNotCalled                   = new StringResource(StringDatabase, "BEGIN_INIT_NOT_CALLED");
        public static readonly StringResource LocalPropertyCannotBeAppliedToType   = new StringResource(StringDatabase, "LOCAL_PROPERTY_CANNOT_BE_APPLIED_TO_TYPE");
        public static readonly StringResource TypeIsNotComparable                  = new StringResource(StringDatabase, "TYPE_IS_NOT_COMPARABLE");
        public static readonly StringResource InvalidTriggerComparison             = new StringResource(StringDatabase, "INVALID_TRIGGER_COMPARISON");
        public static readonly StringResource InvalidAssetIdentifier               = new StringResource(StringDatabase, "INVALID_ASSET_IDENTIFIER");
#pragma warning restore 1591
    }
}
