using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.FMOD
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class FMODStrings
    {
        /// <summary>
        /// Initializes the FMODStrings type.
        /// </summary>
        static FMODStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.FMOD.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

        public static readonly StringResource FMODVersionMismatch = new StringResource(StringDatabase, "FMOD_VERSION_MISMATCH");
        public static readonly StringResource NotCurrentlyValid   = new StringResource(StringDatabase, "NOT_CURRENTLY_VALID");
    }
}
