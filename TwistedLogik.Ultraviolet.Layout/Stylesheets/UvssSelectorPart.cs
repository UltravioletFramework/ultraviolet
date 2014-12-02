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
            var rawID         = (id != null && id.StartsWith("#")) ? id.Substring(1) : id;
            var rawClassNames = from c in classes
                                select c.StartsWith(".") ? c.Substring(1) : c;

            this.element  = element;
            this.id       = rawID;
            this.classes  = rawClassNames.ToList();
            this.priority = CalculatePriority();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0}{1}{2}", element, (id == null) ? null : "#" + id, String.Join(String.Empty, classes.Select(x => "." + x)));
        }

        /// <summary>
        /// Gets the selector part's contribution to its selector's overall priority.
        /// </summary>
        public Int32 Priority
        {
            get { return priority; }
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

        /// <summary>
        /// Calculates the selector part's contribution to its selector's overall priority.
        /// </summary>
        /// <returns>The selector part's priority.</returns>
        private Int32 CalculatePriority()
        {
            var priority = 0;

            const Int32 ElementPriority = 1;
            if (!String.IsNullOrEmpty(element))
            {
                priority += ElementPriority;
            }

            const Int32 IdentifierPriority = 100;
            if (!String.IsNullOrEmpty(id))
            {
                priority += IdentifierPriority;
            }

            const Int32 ClassPriority = 10;
            priority += ClassPriority * classes.Count();

            return priority;
        }

        // Property values.
        private readonly Int32 priority;
        private readonly String element;
        private readonly String id;
        private readonly IEnumerable<String> classes;
    }
}
