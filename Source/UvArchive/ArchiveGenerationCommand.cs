namespace UvArchive
{
    /// <summary>
    /// Represents the commands which can be passed to the archive generation tool.
    /// </summary>
    public enum ArchiveGenerationCommand
    {
        /// <summary>
        /// Packs the specified list of directories into an archive.
        /// </summary>
        Pack,

        /// <summary>
        /// Lists the contents of the specified archive.
        /// </summary>
        List,
    }
}