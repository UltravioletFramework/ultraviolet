using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Contains the Xilium layout engine's string resources.
    /// </summary>
    public static class XiliumStrings
    {
        /// <summary>
        /// Initializes the XiliumStrings type.
        /// </summary>
        static XiliumStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.Layout.XiliumCef.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

        public static readonly StringResource OnlySupportedOnWindows     = new StringResource(StringDatabase, "ONLY_SUPPORTED_ON_WINDOWS");
        public static readonly StringResource CefRuntimeError            = new StringResource(StringDatabase, "RUNTIME_ERROR");
        public static readonly StringResource LayoutNotInitialized       = new StringResource(StringDatabase, "LAYOUT_NOT_INITIALIZED");
        public static readonly StringResource LayoutNotReady             = new StringResource(StringDatabase, "LAYOUT_NOT_READY");
        public static readonly StringResource NotADelegate               = new StringResource(StringDatabase, "NOT_A_DELEGATE");
        public static readonly StringResource ApiRegistryAlreadyCreated  = new StringResource(StringDatabase, "API_REGISTRY_ALREADY_CREATED");
        public static readonly StringResource ApiRegistryDoesNotExist    = new StringResource(StringDatabase, "API_REGISTRY_DOES_NOT_EXIST");
        public static readonly StringResource UnableToBindApiCall        = new StringResource(StringDatabase, "UNABLE_TO_BIND_API_CALL");
        public static readonly StringResource InvalidScriptResult        = new StringResource(StringDatabase, "INVALID_SCRIPT_RESULT");
        public static readonly StringResource InvalidLayoutSource        = new StringResource(StringDatabase, "INVALID_LAYOUT_SOURCE");
        public static readonly StringResource ScriptMustReturnVoidOrTask = new StringResource(StringDatabase, "SCRIPT_MUST_RETURN_VOID_OR_TASK");
        public static readonly StringResource UnrecognizedTaskID         = new StringResource(StringDatabase, "UNRECOGNIZED_TASK_ID");
    }
}
