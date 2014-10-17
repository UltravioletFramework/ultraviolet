using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an attribute which marks a class as a content importer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ContentImporterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the ContentImporterAttribute class.
        /// </summary>
        /// <param name="extension">The file extension for which the importer is used.</param>
        public ContentImporterAttribute(String extension)
        {
            Contract.RequireNotEmpty(extension, "extension");
            Contract.Ensure(extension.StartsWith("."), "extension");

            this.Extension = extension;
        }

        /// <summary>
        /// Gets the file extensions for which the importer is used.
        /// </summary>
        public String Extension
        {
            get;
            private set;
        }
    }
}
