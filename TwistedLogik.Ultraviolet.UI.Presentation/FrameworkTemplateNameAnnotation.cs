using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an annotation used to uniquely identify a <see cref="FrameworkTemplate"/> element
    /// within a particular UVML view definition.
    /// </summary>
    internal class FrameworkTemplateNameAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkTemplateNameAnnotation"/> class.
        /// </summary>
        /// <param name="name">The name of the framework template within its view definition.</param>
        public FrameworkTemplateNameAnnotation(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the framework template within its view definition.
        /// </summary>
        public String Name { get; }
    }
}
