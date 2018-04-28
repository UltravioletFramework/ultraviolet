using System;

namespace Ultraviolet.Presentation.Documents
{
    /// <summary>
    /// Represents an <see cref="AdornerDecorator"/> which does not connect its child to the logical tree.
    /// </summary>
    [UvmlKnownType]
    internal class NonLogicalAdornerDecorator : AdornerDecorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonLogicalAdornerDecorator"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public NonLogicalAdornerDecorator(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        internal override Boolean HooksChildIntoLogicalTree
        {
            get { return false; }
        }
    }
}
