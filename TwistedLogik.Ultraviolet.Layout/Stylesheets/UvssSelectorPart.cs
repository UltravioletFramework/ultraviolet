using System;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents one part of a selector in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelectorPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPart"/> class.
        /// </summary>
        /// <param name="element">The name of the element type that matches this selector part.</param>
        /// <param name="id">The identifier of the element that matches this selector part.</param>
        /// <param name="classes">The list of classes which match this selector part.</param>
        internal UvssSelectorPart(String element, String id, IEnumerable<String> classes)
        {
            this.element = element;
            this.id      = id;
            this.classes = classes.ToList();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0}{1}{2}", element, id, String.Join(String.Empty, classes));
        }

        /// <summary>
        /// Gets the name of the element type that matches this selector part.
        /// </summary>
        public String Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the identifier of the element that matches this selector part.
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the list of classes which match this selector part.
        /// </summary>
        public IEnumerable<String> Classes
        {
            get { return classes; }
        }

        // Property values.
        private readonly String element;
        private readonly String id;
        private readonly IEnumerable<String> classes;
    }
}
