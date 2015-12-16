using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Gluon
{
    /// <summary>
    /// Contains Gluon's string resources.
    /// </summary>
    public static class GluonStrings
    {
        /// <summary>
        /// Initializes the GluonStrings type.
        /// </summary>
        static GluonStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Gluon.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource CouldNotLoadFunction        = new StringResource(StringDatabase, "COULD_NOT_LOAD_FUNCTION");
        public static readonly StringResource LoadedOpenGLVersion         = new StringResource(StringDatabase, "LOADED_OPENGL_VERSION");
        public static readonly StringResource UnloadedOpenGL              = new StringResource(StringDatabase, "UNLOADED_OPENGL");
        public static readonly StringResource OpenGLNotInitialized        = new StringResource(StringDatabase, "OPENGL_NOT_INITIALIZED");
        public static readonly StringResource OpenGLAlreadyInitialized    = new StringResource(StringDatabase, "OPENGL_ALREADY_INITIALIZED");
        public static readonly StringResource FunctionNotProvidedByDriver = new StringResource(StringDatabase, "FUNCTION_NOT_PROVIDED_BY_DRIVER");
        public static readonly StringResource MissingRequiredExtension    = new StringResource(StringDatabase, "MISSING_REQUIRED_EXTENSION");
        public static readonly StringResource DriverReturnedNullPointer   = new StringResource(StringDatabase, "DRIVER_RETURNED_NULL_POINTER");
#pragma warning restore 1591
    }
}
