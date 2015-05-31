using System;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents one part of a selector in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelectorPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPart"/> class.
        /// </summary>
        /// <param name="child">A value indicating whether this selector must be the immediate child of the previous selector.</param>
        /// <param name="element">The name of the element type that matches this selector part.</param>
        /// <param name="id">The identifier of the element that matches this selector part.</param>
        /// <param name="pseudoClass">The selector part's pseudo-class, if any.</param>
        /// <param name="classes">The list of classes which match this selector part.</param>
        internal UvssSelectorPart(Boolean child, String element, String id, String pseudoClass, IEnumerable<String> classes)
        {
            var rawID          = (id != null && id.StartsWith("#")) ? id.Substring(1) : id;
            var rawPseudoClass = (pseudoClass != null && pseudoClass.StartsWith(":")) ? pseudoClass.Substring(1) : pseudoClass;
            var rawClassNames  = from c in classes select c.StartsWith(".") ? c.Substring(1) : c;

            this.child       = child;
            this.element     = element;
            this.id          = rawID;
            this.pseudoClass = rawPseudoClass;
            this.classes     = new UvssClassCollection(rawClassNames);
            this.priority    = CalculatePriority();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var partChild   = Child ? " > " : String.Empty;
            var partElement = Universal ? "*" : element;
            var partID      = HasID ? "#" + ID : null;
            var partClasses = String.Join(String.Empty, classes.Select(x => "." + x));

            return String.Format("{0}{1}{2}{3}", partChild, partElement, partID, partClasses);
        }

        /// <summary>
        /// Gets the selector part's contribution to its selector's overall priority.
        /// </summary>
        public Int32 Priority
        {
            get { return priority; }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part is an immediate child of its predecessor.
        /// </summary>
        public Boolean Child
        {
            get { return child; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a universal selector.
        /// </summary>
        public Boolean Universal
        {
            get { return !HasElement && !HasID && !HasClasses; }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part includes an element type.
        /// </summary>
        public Boolean HasElement
        {
            get { return !String.IsNullOrEmpty(Element); }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part includes an ID.
        /// </summary>
        public Boolean HasID
        {
            get { return !String.IsNullOrEmpty(ID); }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part includes a pseudo-class.
        /// </summary>
        public Boolean HasPseudoClass
        {
            get { return !String.IsNullOrEmpty(PseudoClass); }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part includes any classes.
        /// </summary>
        public Boolean HasClasses
        {
            get { return classes.Count > 0; }
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
        /// Gets the selector part's pseudo-class.
        /// </summary>
        public String PseudoClass
        {
            get { return pseudoClass; }
        }

        /// <summary>
        /// Gets the list of classes which match this selector part.
        /// </summary>
        public UvssClassCollection Classes
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

            const Int32 ElementPriority    = 1;
            const Int32 ClassPriority      = 10;
            const Int32 IdentifierPriority = 100;

            if (!String.IsNullOrEmpty(element))
            {
                if (String.Equals(element, "document", StringComparison.OrdinalIgnoreCase))
                {
                    priority += IdentifierPriority;
                }
                else
                {
                    priority += ElementPriority;
                }
            }

            priority += ClassPriority * classes.Count();

            if (!String.IsNullOrEmpty(id))
            {
                priority += IdentifierPriority;
            }

            return priority;
        }

        // Property values.
        private readonly Int32 priority;
        private readonly Boolean child;
        private readonly String element;
        private readonly String id;
        private readonly String pseudoClass;
        private readonly UvssClassCollection classes;
    }
}
