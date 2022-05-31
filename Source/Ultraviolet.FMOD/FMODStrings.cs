using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class FMODStrings
    {
        /// <summary>
        /// Initializes the <see cref="FMODStrings"/> type.
        /// </summary>
        static FMODStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.FMOD.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource FMODVersionMismatch         = new StringResource(StringDatabase, "FMOD_VERSION_MISMATCH");
        public static readonly StringResource NotCurrentlyValid           = new StringResource(StringDatabase, "NOT_CURRENTLY_VALID");
        public static readonly StringResource CannotFindPlatformShimClass = new StringResource(StringDatabase, "CANNOT_FIND_PLATFORM_SHIM_CLASS");
#pragma warning restore 1591
    }
}
