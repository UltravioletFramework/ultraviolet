using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Profiler.dotTrace
{
    /// <summary>
    /// Contains Ultraviolet's string resources.
    /// </summary>
    public static class dotTraceStrings
    {
        /// <summary>
        /// Initializes the <see cref="dotTraceStrings"/> type.
        /// </summary>
        static dotTraceStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.Profiler.dotTrace.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource SectionAlreadyActive      = new StringResource(StringDatabase, "SECTION_ALREADY_ACTIVE");
        public static readonly StringResource SectionNotActive          = new StringResource(StringDatabase, "SECTION_NOT_ACTIVE");
        public static readonly StringResource SectionAlreadyEnabled     = new StringResource(StringDatabase, "SECTION_ALREADY_ENABLED");
        public static readonly StringResource SectionNotEnabled         = new StringResource(StringDatabase, "SECTION_NOT_ENABLED");
        public static readonly StringResource SnapshotAlreadyInProgress = new StringResource(StringDatabase, "SNAPSHOT_ALREADY_IN_PROGRESS");
        public static readonly StringResource SnapshotNotInProgress     = new StringResource(StringDatabase, "SNAPSHOT_NOT_IN_PROGRESS");
        public static readonly StringResource SnapshottingFrame         = new StringResource(StringDatabase, "SNAPSHOTTING_FRAME");
#pragma warning restore 1591
    }
}
