using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represent a UVML reference to a named element.
    /// </summary>
    internal sealed class UvmlElementReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlElementReference"/> class.
        /// </summary>
        /// <param name="name">The name of the referenced element.</param>
        public UvmlElementReference(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Retrieves the referenced element from the specified namescope.
        /// </summary>
        /// <param name="namescope">The namescope from which to retrieve the referenced element.</param>
        /// <returns></returns>
        public IInputElement GetReferencedElement(Namescope namescope)
        {
            Contract.Require(namescope, nameof(namescope));

            var element = namescope.GetElementByName(Name);
            if (element == null)
                throw new UvmlException(PresentationStrings.NamedElementDoesNotExist.Format(Name));

            return element;
        }
        
        /// <summary>
        /// Gets the name of the referenced element.
        /// </summary>
        public String Name { get; }
    }
}
