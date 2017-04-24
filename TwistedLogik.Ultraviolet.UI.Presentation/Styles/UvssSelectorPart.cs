using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents one part of a selector in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelectorPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorPart"/> class.
        /// </summary>
        /// <param name="qualifier">The qualifier value for this selector part, if it has one.</param>
        /// <param name="type">The name of the element type that matches this selector part.</param>
        /// <param name="typeIsExact">A value indicating whether the element type that matches this 
        /// selector part must be an exact match.</param>
        /// <param name="name">The name of the element that matches this selector part.</param>
        /// <param name="pseudoClass">The selector part's pseudo-class, if any.</param>
        /// <param name="classes">The list of classes which match this selector part.</param>
        internal UvssSelectorPart(UvssSelectorPartQualifier qualifier, String type, Boolean typeIsExact,
            String name, String pseudoClass, IEnumerable<String> classes)
        {
            var rawName = (name != null && name.StartsWith("#")) ? name.Substring(1) : name;
            var rawPseudoClass = (pseudoClass != null && pseudoClass.StartsWith(":")) ? pseudoClass.Substring(1) : pseudoClass;
            var rawClassNames = from c in classes select c.StartsWith(".") ? c.Substring(1) : c;

            this.qualifier = qualifier;
            this.type = type;
            this.typeIsExact = typeIsExact;
            this.name = rawName;
            this.pseudoClass = rawPseudoClass;
            this.classes = new UvssClassCollection(rawClassNames);
            this.priority = CalculatePriority();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var partQualifier = String.Empty;

            switch (qualifier)
            {
                case UvssSelectorPartQualifier.VisualChild:
                    partQualifier = " > ";
                    break;

                case UvssSelectorPartQualifier.LogicalChild:
                    partQualifier = " >? ";
                    break;

                case UvssSelectorPartQualifier.TemplatedChild:
                    partQualifier = " >> ";
                    break;
            }

            var partElement = Universal ? "*" : type;
            var partName = HasName ? "#" + Name : null;
            var partClasses = String.Join(String.Empty, classes.Select(x => "." + x));

            return String.Format("{0}{1}{2}{3}", partQualifier, partElement, partName, partClasses);
        }

        /// <summary>
        /// Gets the selector part's contribution to its selector's overall priority.
        /// </summary>
        public Int32 Priority
        {
            get { return priority; }
        }

        /// <summary>
        /// Gets a <see cref="UvssSelectorPartQualifier"/> value that specifies this selector part's qualifier. This is most commonly used to
        /// indicate whether this selector part must be a child of the previous part.
        /// </summary>
        public UvssSelectorPartQualifier Qualifier
        {
            get { return qualifier; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a universal selector.
        /// </summary>
        public Boolean Universal
        {
            get { return !HasType && !HasName && !HasClasses; }
        }
        
        /// <summary>
        /// Gets a value indicating whether this selector part includes an element type.
        /// </summary>
        public Boolean HasType
        {
            get { return !String.IsNullOrEmpty(Type); }
        }

        /// <summary>
        /// Gets a value indicating whether this selelector part must exactly match element types.
        /// </summary>
        public Boolean HasExactType
        {
            get { return HasType && typeIsExact; }
        }

        /// <summary>
        /// Gets a value indicating whether this selector part includes an element name.
        /// </summary>
        public Boolean HasName
        {
            get { return !String.IsNullOrEmpty(Name); }
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
        public String Type
        {
            get { return type; }
        }

        /// <summary>
        /// Gets the identifier of the element that matches this selector part.
        /// </summary>
        public String Name
        {
            get { return name; }
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

            if (!String.IsNullOrEmpty(type))
            {
                if (String.Equals(type, "document", StringComparison.OrdinalIgnoreCase))
                {
                    priority += IdentifierPriority;
                }
                else
                {
                    priority += ElementPriority;
                }
            }

            priority += ClassPriority * classes.Count();

            if (!String.IsNullOrEmpty(name))
            {
                priority += IdentifierPriority;
            }

            return priority;
        }

        // Property values.
        private readonly Int32 priority;
        private readonly UvssSelectorPartQualifier qualifier;
        private readonly Boolean typeIsExact;
        private readonly String type;
        private readonly String name;
        private readonly String pseudoClass;
        private readonly UvssClassCollection classes;
    }
}
