using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Contains the Ultraviolet Presentation Foundation's string resources.
    /// </summary>
    public static class ImGuiStrings
    {
        /// <summary>
        /// Initializes the <see cref="ImGuiStrings"/> type.
        /// </summary>
        static ImGuiStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.ImGuiViewProvider.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource ViewModelTypeNotFound     = new StringResource(StringDatabase, "VIEW_MODEL_TYPE_NOT_FOUND");
        public static readonly StringResource ImGuiContextIsNotCurrent  = new StringResource(StringDatabase, "IMGUI_CONTEXT_IS_NOT_CURRENT");
        public static readonly StringResource RegistryIsLocked          = new StringResource(StringDatabase, "REGISTRY_IS_LOCKED");
#pragma warning restore 1591
    }
}
