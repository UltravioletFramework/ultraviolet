using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Contains Ultraviolet's string resources.
    /// </summary>
    public static class UltravioletStrings
    {
        /// <summary>
        /// Initializes the <see cref="UltravioletStrings"/> type.
        /// </summary>
        static UltravioletStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource DefaultWindowCaption                 = new StringResource(StringDatabase, "DEFAULT_WINDOW_CAPTION");
        public static readonly StringResource MissingCompatibilityShim             = new StringResource(StringDatabase, "MISSING_COMPATIBILITY_SHIM");
        public static readonly StringResource InvalidDependency                    = new StringResource(StringDatabase, "INVALID_DEPENDENCY");
        public static readonly StringResource InvalidResource                      = new StringResource(StringDatabase, "INVALID_RESOURCE");
        public static readonly StringResource ContextMissing                       = new StringResource(StringDatabase, "CONTEXT_MISSING");
        public static readonly StringResource ContextNotCreated                    = new StringResource(StringDatabase, "CONTEXT_NOT_CREATED");
        public static readonly StringResource ContextAlreadyExists                 = new StringResource(StringDatabase, "CONTEXT_ALREADY_EXISTS");
        public static readonly StringResource FactoryMethodInvalidDelegate         = new StringResource(StringDatabase, "FACTORY_METHOD_INVALID_DELEGATE");
        public static readonly StringResource FactoryMethodAlreadyRegistered       = new StringResource(StringDatabase, "FACTORY_METHOD_ALREADY_REGISTERED");
        public static readonly StringResource NamedFactoryMethodAlreadyRegistered  = new StringResource(StringDatabase, "NAMED_FACTORY_METHOD_ALREADY_REGISTERED");
        public static readonly StringResource MissingFactoryMethod                 = new StringResource(StringDatabase, "MISSING_FACTORY_METHOD");
        public static readonly StringResource MissingNamedFactoryMethod            = new StringResource(StringDatabase, "MISSING_NAMED_FACTORY_METHOD");
        public static readonly StringResource NoNamedFactoryMethods                = new StringResource(StringDatabase, "NO_NAMED_FACTORY_METHODS");
        public static readonly StringResource ImporterAlreadyRegistered            = new StringResource(StringDatabase, "IMPORTER_ALREADY_REGISTERED");
        public static readonly StringResource ProcessorAlreadyRegistered           = new StringResource(StringDatabase, "PROCESSOR_ALREADY_REGISTERED");
        public static readonly StringResource ImporterRequiresCtor                 = new StringResource(StringDatabase, "IMPORTER_REQUIRES_CTOR");
        public static readonly StringResource ProcessorRequiresCtor                = new StringResource(StringDatabase, "PROCESSOR_REQUIRES_CTOR");
        public static readonly StringResource NoValidImporter                      = new StringResource(StringDatabase, "NO_VALID_IMPORTER");
        public static readonly StringResource NoValidProcessor                     = new StringResource(StringDatabase, "NO_VALID_PROCESSOR");
        public static readonly StringResource ProcessorInvalidBaseClass            = new StringResource(StringDatabase, "PROCESSOR_INVALID_BASE_CLASS");
        public static readonly StringResource ImporterInvalidBaseClass             = new StringResource(StringDatabase, "IMPORTER_INVALID_BASE_CLASS");
        public static readonly StringResource ImporterNeedsExtension               = new StringResource(StringDatabase, "IMPORTER_NEEDS_EXTENSION");
        public static readonly StringResource ImporterOutputInvalid                = new StringResource(StringDatabase, "IMPORTER_OUTPUT_INVALID");
        public static readonly StringResource FileNotFound                         = new StringResource(StringDatabase, "FILE_NOT_FOUND");
        public static readonly StringResource FileAmbiguous                        = new StringResource(StringDatabase, "FILE_AMBIGUOUS");
        public static readonly StringResource SpriteBatchNestingError              = new StringResource(StringDatabase, "SPRITEBATCH_NESTING_ERROR");
        public static readonly StringResource SpriteBatchDemandImbalance           = new StringResource(StringDatabase, "SPRITEBATCH_DEMAND_IMBALANCE");
        public static readonly StringResource UnsupportedVertexFormat              = new StringResource(StringDatabase, "UNSUPPORTED_VERTEX_FORMAT");
        public static readonly StringResource BeginCannotBeCalledAgain             = new StringResource(StringDatabase, "BEGIN_CANNOT_BE_CALLED_AGAIN");
        public static readonly StringResource BeginMustBeCalledBeforeEnd           = new StringResource(StringDatabase, "BEGIN_MUST_BE_CALLED_BEFORE_END");
        public static readonly StringResource BeginMustBeCalledBeforeDraw          = new StringResource(StringDatabase, "BEGIN_MUST_BE_CALLED_BEFORE_DRAW");
        public static readonly StringResource StateIsImmutableAfterBind            = new StringResource(StringDatabase, "STATE_IS_IMMUTABLE_AFTER_BIND");
        public static readonly StringResource InvalidHandle                        = new StringResource(StringDatabase, "INVALID_HANDLE");
        public static readonly StringResource InvalidIdentifier                    = new StringResource(StringDatabase, "INVALID_IDENTIFIER");
        public static readonly StringResource InvalidToken                         = new StringResource(StringDatabase, "INVALID_TOKEN");
        public static readonly StringResource UnrecognizedFont                     = new StringResource(StringDatabase, "UNRECOGNIZED_FONT");
        public static readonly StringResource UnrecognizedIcon                     = new StringResource(StringDatabase, "UNRECOGNIZED_ICON");
        public static readonly StringResource UnrecognizedStyle                    = new StringResource(StringDatabase, "UNRECOGNIZED_STYLE");
        public static readonly StringResource InvalidLayoutSettings                = new StringResource(StringDatabase, "INVALID_LAYOUT_SETTINGS");
        public static readonly StringResource InvalidFontTexture                   = new StringResource(StringDatabase, "INVALID_FONT_TEXTURE");
        public static readonly StringResource InvalidFontFaces                     = new StringResource(StringDatabase, "INVALID_FONT_FACES");
        public static readonly StringResource InvalidViewModelProperty             = new StringResource(StringDatabase, "INVALID_VIEW_MODEL_PROPERTY");
        public static readonly StringResource InvalidCursorImage                   = new StringResource(StringDatabase, "INVALID_CURSOR_IMAGE");
        public static readonly StringResource InvalidCursorName                    = new StringResource(StringDatabase, "INVALID_CURSOR_NAME");
        public static readonly StringResource GeometryStreamAlreadyHasIndices      = new StringResource(StringDatabase, "GEOM_STREAM_ALREADY_HAS_INDICES");
        public static readonly StringResource CannotQueueWorkItems                 = new StringResource(StringDatabase, "CANNOT_QUEUE_WORK_ITEMS");
        public static readonly StringResource CannotSpawnTasks                     = new StringResource(StringDatabase, "CANNOT_SPAWN_TASKS");
        public static readonly StringResource WorkItemsMustBeProcessedOnMainThread = new StringResource(StringDatabase, "WORK_ITEMS_MUST_BE_PROCESSED_ON_MAIN_THREAD");
        public static readonly StringResource NoValidConstructor                   = new StringResource(StringDatabase, "NO_VALID_CONSTRUCTOR");
        public static readonly StringResource InputActionAlreadyExists             = new StringResource(StringDatabase, "INPUT_ACTION_ALREADY_EXISTS");
        public static readonly StringResource FailedToPackTextureAtlas             = new StringResource(StringDatabase, "FAILED_TO_PACK_TEXTURE_ATLAS");
        public static readonly StringResource TextureAtlasWildcardsCannotBeNamed   = new StringResource(StringDatabase, "TEXTURE_ATLAS_WILDCARDS_CANNOT_BE_NAMED");
        public static readonly StringResource InvalidTextureAtlasImagePath         = new StringResource(StringDatabase, "INVALID_TEXTURE_ATLAS_IMAGE_PATH");
        public static readonly StringResource TextureAtlasAlreadyContainsCell      = new StringResource(StringDatabase, "TEXTURE_ATLAS_ALREADY_CONTAINS_CELL");
        public static readonly StringResource SpriteContainsInvalidAtlasCell       = new StringResource(StringDatabase, "SPRITE_CONTAINS_INVALID_ATLAS_CELL");
        public static readonly StringResource SpriteContainsBothTextureAndAtlas    = new StringResource(StringDatabase, "SPRITE_CONTAINS_BOTH_TEXTURE_AND_ATLAS");
        public static readonly StringResource SpriteContainsAtlasButNoCell         = new StringResource(StringDatabase, "SPRITE_CONTAINS_ATLAS_BUT_NO_CELL");
        public static readonly StringResource SpriteContainsCellButNoAtlas         = new StringResource(StringDatabase, "SPRITE_CONTAINS_CELL_BUT_NO_ATLAS");
        public static readonly StringResource TextureAtlasContainsNoImages         = new StringResource(StringDatabase, "TEXTURE_ATLAS_CONTAINS_NO_IMAGES");
        public static readonly StringResource AssetMetadataHasInvalidFilename      = new StringResource(StringDatabase, "ASSET_METADATA_HAS_INVALID_FILENAME");
        public static readonly StringResource AssetMetadataFileNotFound            = new StringResource(StringDatabase, "ASSET_METADATA_FILE_NOT_FOUND");
        public static readonly StringResource NoLayoutProvider                     = new StringResource(StringDatabase, "NO_LAYOUT_PROVIDER");
        public static readonly StringResource LayoutProviderNotFound               = new StringResource(StringDatabase, "LAYOUT_PROVIDER_NOT_FOUND");
        public static readonly StringResource LayoutProviderInvalidCtor            = new StringResource(StringDatabase, "LAYOUT_PROVIDER_INVALID_CTOR");
        public static readonly StringResource ContentManifestAlreadyContainsAsset  = new StringResource(StringDatabase, "CONTENT_MANIFEST_ALREADY_CONTAINS_ASSET");
        public static readonly StringResource ContentManifestDoesNotExist          = new StringResource(StringDatabase, "CONTENT_MANIFEST_DOES_NOT_EXIST");
        public static readonly StringResource ContentManifestGroupDoesNotExist     = new StringResource(StringDatabase, "CONTENT_MANIFEST_GROUP_DOES_NOT_EXIST");
        public static readonly StringResource AssetDoesNotExistWithinManifest      = new StringResource(StringDatabase, "ASSET_DOES_NOT_EXIST_WITHIN_MANIFEST");
        public static readonly StringResource InvalidContentManifestName           = new StringResource(StringDatabase, "INVALID_CONTENT_MANIFEST_NAME");
        public static readonly StringResource InvalidContentManifestGroupName      = new StringResource(StringDatabase, "INVALID_CONTENT_MANIFEST_GROUP_NAME");
        public static readonly StringResource InvalidContentManifestGroupType      = new StringResource(StringDatabase, "INVALID_CONTENT_MANIFEST_GROUP_TYPE");
        public static readonly StringResource InvalidContentManifestAssetName      = new StringResource(StringDatabase, "INVALID_CONTENT_MANIFEST_ASSET_NAME");
        public static readonly StringResource InvalidContentManifestAssetPath      = new StringResource(StringDatabase, "INVALID_CONTENT_MANIFEST_ASSET_PATH");
        public static readonly StringResource InvalidSpriteAnimationReference      = new StringResource(StringDatabase, "INVALID_SPRITE_ANIMATION_REFERENCE");
        public static readonly StringResource InvalidCurveData                     = new StringResource(StringDatabase, "INVALID_CURVE_DATA");
        public static readonly StringResource AssetPathMustBeRelative              = new StringResource(StringDatabase, "ASSET_PATH_MUST_BE_RELATIVE");
        public static readonly StringResource AssetPathCannotTraverseDirectories   = new StringResource(StringDatabase, "ASSET_PATH_CANNOT_TRAVERSE_DIRECTORIES");
        public static readonly StringResource ScreenAlreadyInStack                 = new StringResource(StringDatabase, "SCREEN_ALREADY_IN_STACK");
        public static readonly StringResource ScreenNotInStack                     = new StringResource(StringDatabase, "SCREEN_NOT_IN_STACK");
        public static readonly StringResource InvalidWindow                        = new StringResource(StringDatabase, "INVALID_WINDOW");
        public static readonly StringResource NoPrimaryWindow                      = new StringResource(StringDatabase, "NO_PRIMARY_WINDOW");
        public static readonly StringResource InvalidNamedCollectionName           = new StringResource(StringDatabase, "INVALID_NAMED_COLLECTION_NAME");
        public static readonly StringResource RectanglePackerOutOfSpace            = new StringResource(StringDatabase, "RECTANGLE_PACKER_OUT_OF_SPACE");
        public static readonly StringResource InputActionCollectionAlreadyCreated  = new StringResource(StringDatabase, "INPUT_ACTION_COLLECTION_ALREADY_CREATED");
        public static readonly StringResource StackIsEmpty                         = new StringResource(StringDatabase, "STACK_IS_EMPTY");
        public static readonly StringResource ContentLoaderAlreadyLoading          = new StringResource(StringDatabase, "CONTENT_LOADER_ALREADY_LOADING");
        public static readonly StringResource NoContentManagerSpecified            = new StringResource(StringDatabase, "NO_CONTENT_MANAGER_SPECIFIED");
        public static readonly StringResource ContentHandlersAlreadyRegistered     = new StringResource(StringDatabase, "CONTENT_HANDLERS_ALREADY_REGISTERED");
        public static readonly StringResource CannotSeekPastBeginningOfStream      = new StringResource(StringDatabase, "CANNOT_SEEK_PAST_BEGINNING_OF_STREAM");
        public static readonly StringResource InvalidContentArchive                = new StringResource(StringDatabase, "INVALID_CONTENT_ARCHIVE");
        public static readonly StringResource ContentManagerNotBatchingDeletes     = new StringResource(StringDatabase, "CONTENT_MANAGER_NOT_BATCHING_DELETES");
        public static readonly StringResource ContentManagerRequiresBatch          = new StringResource(StringDatabase, "CONTENT_MANAGER_REQUIRES_BATCH");
        public static readonly StringResource StretchableImageNotLoaded            = new StringResource(StringDatabase, "STRETCHABLE_IMAGE_NOT_LOADED");
        public static readonly StringResource InvalidViewProviderAssembly          = new StringResource(StringDatabase, "INVALID_VIEW_PROVIDER_ASSEMBLY");
        public static readonly StringResource IncompatibleViewModel                = new StringResource(StringDatabase, "INCOMPATIBLE_VIEW_MODEL");
#pragma warning restore 1591
    }
}
