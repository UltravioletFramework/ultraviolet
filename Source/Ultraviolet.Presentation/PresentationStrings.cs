using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation
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
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.Presentation.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.Presentation.Resources.Strings_Commands.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource StyleSheetParserError                = new StringResource(StringDatabase, "STYLE_SHEET_PARSER_ERROR");
        public static readonly StringResource IsNotDependencyObject                = new StringResource(StringDatabase, "IS_NOT_DEPENDENCY_OBJECT");
        public static readonly StringResource PropertyHasNoGetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_GETTER");
        public static readonly StringResource PropertyHasNoSetter                  = new StringResource(StringDatabase, "PROPERTY_HAS_NO_SETTER");
        public static readonly StringResource DependencyPropertyAlreadyRegistered  = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_ALREADY_REGISTERED");
        public static readonly StringResource DependencyPropertyIsReadOnly         = new StringResource(StringDatabase, "DEPENDENCY_PROPERTY_IS_READ_ONLY");
        public static readonly StringResource RoutedEventAlreadyRegistered         = new StringResource(StringDatabase, "ROUTED_EVENT_ALREADY_REGISTERED");
        public static readonly StringResource CannotResolveBindingExpression       = new StringResource(StringDatabase, "CANNOT_RESOLVE_BINDING_EXPRESSION");
        public static readonly StringResource InvalidBindingExpression             = new StringResource(StringDatabase, "INVALID_BINDING_EXPRESSION");
        public static readonly StringResource BindingIsReadOnly                    = new StringResource(StringDatabase, "BINDING_IS_READ_ONLY");
        public static readonly StringResource BindingIsWriteOnly                   = new StringResource(StringDatabase, "BINDING_IS_WRITE_ONLY");
        public static readonly StringResource UIElementInvalidCtor                 = new StringResource(StringDatabase, "UIELEMENT_INVALID_CTOR");
        public static readonly StringResource ViewModelTypeNotFound                = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_NOT_FOUND");
        public static readonly StringResource KnownTypeAlreadyRegistered           = new StringResource(StringDatabase, "KNOWN_TYPE_ALREADY_REGISTERED");
        public static readonly StringResource UnrecognizedType                     = new StringResource(StringDatabase, "UNRECOGNIZED_TYPE");
        public static readonly StringResource InvalidDefaultProperty               = new StringResource(StringDatabase, "INVALID_DEFAULT_PROPERTY");
        public static readonly StringResource NoViewModel                          = new StringResource(StringDatabase, "NO_VIEW_MODEL");
        public static readonly StringResource DuplicateNamescopeName               = new StringResource(StringDatabase, "DUPLICATE_NAMESCOPE_NAME");
        public static readonly StringResource AmbiguousDependencyProperty          = new StringResource(StringDatabase, "AMBIGUOUS_DEPENDENCY_PROPERTY");
        public static readonly StringResource InvalidChildElements                 = new StringResource(StringDatabase, "INVALID_CHILD_ELEMENTS");
        public static readonly StringResource InvalidElementPropertyValue          = new StringResource(StringDatabase, "INVALID_ELEMENT_PROPERTY_VALUE");
        public static readonly StringResource ComponentRootAlreadyLoaded           = new StringResource(StringDatabase, "COMPONENT_ROOT_ALREADY_LOADED");
        public static readonly StringResource CollectionContainsInvalidElements    = new StringResource(StringDatabase, "COLLECTION_CONTAINS_INVALID_ELEMENTS");
        public static readonly StringResource CollectionHasNoAddMethod             = new StringResource(StringDatabase, "COLLECTION_HAS_NO_ADD_METHOD");
        public static readonly StringResource CollectionCannotBeCleared            = new StringResource(StringDatabase, "COLLECTION_CANNOT_BE_CLEARED");
        public static readonly StringResource CollectionCannotBeCreated            = new StringResource(StringDatabase, "COLLECTION_CANNOT_BE_CREATED");
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
        public static readonly StringResource ElementIsNotAnAncestor               = new StringResource(StringDatabase, "ELEMENT_IS_NOT_AN_ANCESTOR");
        public static readonly StringResource ElementIsNotADescendant              = new StringResource(StringDatabase, "ELEMENT_IS_NOT_A_DESCENDANT");
        public static readonly StringResource InvalidTransformation                = new StringResource(StringDatabase, "INVALID_TRANSFORMATION");
        public static readonly StringResource DrawingContextDoesNotHaveSpriteBatch = new StringResource(StringDatabase, "DRAWING_CONTEXT_DOES_NOT_HAVE_SPRITEBATCH");
        public static readonly StringResource CompiledExpressionsAssemblyNotFound  = new StringResource(StringDatabase, "COMPILED_EXPRESSIONS_ASSEMBLY_NOT_FOUND");
        public static readonly StringResource CompiledExpressionNotFound           = new StringResource(StringDatabase, "COMPILED_EXPRESSION_NOT_FOUND");
        public static readonly StringResource ExpressionCompilerNotFound           = new StringResource(StringDatabase, "EXPRESSION_COMPILER_NOT_FOUND");
        public static readonly StringResource ExpressionCompilerTypeNotValid       = new StringResource(StringDatabase, "EXPRESSION_COMPILER_TYPE_NOT_VALID");
        public static readonly StringResource CannotFindDependencyPropertyField    = new StringResource(StringDatabase, "CANNOT_FIND_DEPENDENCY_PROPERTY_FIELD");
        public static readonly StringResource InvalidBindingExpressionCompilerAsm  = new StringResource(StringDatabase, "INVALID_BINDING_EXPRESSION_COMPILER_ASM");
        public static readonly StringResource AttachablePropertyNotFound           = new StringResource(StringDatabase, "ATTACHABLE_PROPERTY_NOT_FOUND");
        public static readonly StringResource IncompatibleType                     = new StringResource(StringDatabase, "INCOMPATIBLE_TYPE");
        public static readonly StringResource FactoryMethodMissing                 = new StringResource(StringDatabase, "FACTORY_METHOD_MISSING");
        public static readonly StringResource FactoryMethodInvalidResult           = new StringResource(StringDatabase, "FACTORY_METHOD_INVALID_RESULT");
        public static readonly StringResource InvalidCompilerOptions               = new StringResource(StringDatabase, "INVALID_COMPILER_OPTIONS");
        public static readonly StringResource CannotFindViewModelWrapper           = new StringResource(StringDatabase, "CANNOT_FIND_VIEW_MODEL_WRAPPER");
        public static readonly StringResource VersionedStringSourceIsStale         = new StringResource(StringDatabase, "VERSIONED_STRING_SOURCE_IS_STALE");
        public static readonly StringResource ViewModelMismatch                    = new StringResource(StringDatabase, "VIEW_MODEL_MISMATCH");
        public static readonly StringResource InvalidEventHandler                  = new StringResource(StringDatabase, "INVALID_EVENT_HANDLER");
        public static readonly StringResource CollectionTypeNotSupported           = new StringResource(StringDatabase, "COLLECTION_TYPE_NOT_SUPPORTED");
        public static readonly StringResource TemplateTypeMismatch                 = new StringResource(StringDatabase, "TEMPLATE_TYPE_MISMATCH");
        public static readonly StringResource NamedElementDoesNotExist             = new StringResource(StringDatabase, "NAMED_ELEMENT_DOES_NOT_EXIST");
        public static readonly StringResource InvalidLinkColorizer                 = new StringResource(StringDatabase, "INVALID_LINK_COLORIZER");
        public static readonly StringResource InvalidLinkStateEvaluator            = new StringResource(StringDatabase, "INVALID_LINK_STATE_EVALUATOR");
        public static readonly StringResource InvalidLinkClickHandler              = new StringResource(StringDatabase, "INVALID_LINK_CLICK_HANDLER");
        public static readonly StringResource CollectionContainsInvalidResources   = new StringResource(StringDatabase, "COLLECTION_CONTAINS_INVALID_RESOURCES");
        public static readonly StringResource PooledResourceAlreadyReleased        = new StringResource(StringDatabase, "POOLED_RESOURCE_ALREADY_RELEASED");
        public static readonly StringResource TemplateMustSpecifyViewModelType     = new StringResource(StringDatabase, "TEMPLATE_MUST_SPECIFY_VIEW_MODEL_TYPE");
        public static readonly StringResource ElementDoesNotBelongToView           = new StringResource(StringDatabase, "ELEMENT_DOES_NOT_BELONG_TO_VIEW");
        public static readonly StringResource DelegateCommandParamTypeMismatch     = new StringResource(StringDatabase, "DELEGATE_COMMAND_PARAM_TYPE_MISMATCH");
        public static readonly StringResource ElementDoesNotHaveNamescope          = new StringResource(StringDatabase, "ELEMENT_DOES_NOT_HAVE_NAMESCOPE");
#pragma warning restore 1591
    }
}
