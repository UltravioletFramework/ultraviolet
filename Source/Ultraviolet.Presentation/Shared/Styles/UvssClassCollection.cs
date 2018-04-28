using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of UVSS classes.
    /// </summary>
    public sealed partial class UvssClassCollection : IEnumerable<String>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssClassCollection"/> class.
        /// </summary>
        /// <param name="classes">A collection of class names with which to initialize the collection.</param>
        internal UvssClassCollection(IEnumerable<String> classes)
        {
            this.storage = (classes == null) ? new List<String>() : new List<String>(classes);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        // State values.
        private readonly List<String> storage;
    }
}
