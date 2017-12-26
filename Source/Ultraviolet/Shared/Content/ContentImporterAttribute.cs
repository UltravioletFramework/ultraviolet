using System;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an attribute which marks a class as a content importer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ContentImporterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentImporterAttribute"/> class.
        /// </summary>
        /// <param name="extension">The file extension for which the importer is used.</param>
        public ContentImporterAttribute(String extension)
        {
            Contract.RequireNotEmpty(extension, nameof(extension));
            Contract.Ensure(extension.StartsWith("."), nameof(extension));

            this.Extension = extension;
        }

        /// <summary>
        /// Gets the file extension which is handled by the importer.
        /// </summary>
        public String Extension
        {
            get;
            private set;
        }
    }
}
